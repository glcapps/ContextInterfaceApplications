using ContextInterfaceApplications.Web.Services;
using ContextInterfaceApplications.Web.Workflows.Demo;
using ContextInterfaceApplications.Runtime;
using Microsoft.Extensions.DependencyInjection;
using ContextInterfaceApplications.Web.Models;

namespace ContextInterfaceApplications.Tests;

public sealed class RenderingTests
{
    [Fact]
    public async Task AgentSurfaceRenderer_EmitsCanonicalWorkflowAndRuntimeMetadata()
    {
        var services = TestServiceFactory.CreateServices();
        var state = new DemoWorkflowDefinition().CreateInitialState();
        var substrate = new RuntimeSubstrateDescriptor(
            "Microsoft Agent Framework",
            "Microsoft.Agents.Hosting.AspNetCore",
            "1.3.176",
            "Runtime substrate for model execution and tool invocation.",
            "The framework is present as infrastructure only; application semantics remain in canonical state and authored surfaces.");
        var renderer = new AgentSurfaceRenderer(new BlazorComponentRenderer(services.GetRequiredService<IServiceScopeFactory>()));

        var artifact = await renderer.RenderAsync(state, substrate);

        Assert.Contains("<context-interface-application consumer=\"agent\">", artifact.Content);
        Assert.Contains("<workflow name=\"Item Review Workspace\">", artifact.Content);
        Assert.Contains("<review-item id=\"item-4821\" status=\"new\">", artifact.Content);
        Assert.Contains("Review Submitted Item", artifact.Content);
    }

    [Fact]
    public async Task HumanSurfaceRenderer_UsesSharedStateInsteadOfSeparateViewModel()
    {
        var services = TestServiceFactory.CreateServices();
        var stateStore = new WorkflowStateStore(new DemoWorkflowDefinition());
        var renderer = new HumanSurfaceRenderer(new BlazorComponentRenderer(services.GetRequiredService<IServiceScopeFactory>()));

        var initialHtml = await renderer.RenderAsync(stateStore.GetState());
        stateStore.AdvanceWorkflow();
        var updatedHtml = await renderer.RenderAsync(stateStore.GetState());

        Assert.Contains("Review a newly submitted item.", initialHtml);
        Assert.Contains("Evaluate the item in active review.", updatedHtml);
    }

    [Fact]
    public async Task ReplayStore_ReturnsLatestRecordedArtifact()
    {
        var services = TestServiceFactory.CreateServices();
        var replayStore = new InMemoryReplayArtifactStore();
        var artifact = await new AgentSurfaceRenderer(
            new BlazorComponentRenderer(services.GetRequiredService<IServiceScopeFactory>()))
            .RenderAsync(
                new DemoWorkflowDefinition().CreateInitialState(),
                new RuntimeSubstrateDescriptor(
                    "Microsoft Agent Framework",
                    "Microsoft.Agents.Hosting.AspNetCore",
                    "1.3.176",
                    "Runtime substrate for model execution and tool invocation.",
                    "The framework is present as infrastructure only; application semantics remain in canonical state and authored surfaces."));

        replayStore.Record(artifact);

        var latest = replayStore.GetLatest();

        Assert.NotNull(latest);
        Assert.Equal(artifact.Id, latest!.Id);
        Assert.Equal(artifact.Content, latest.Content);
    }

    [Fact]
    public void AgentInterfaceSnapshotBuilder_ProjectsWorkflowToolsActionsResultsAndRuntime()
    {
        var state = new DemoWorkflowDefinition().CreateInitialState();
        var substrate = new RuntimeSubstrateDescriptor(
            "Microsoft Agent Framework",
            "Microsoft.Agents.Hosting.AspNetCore",
            "1.3.176",
            "Runtime substrate for model execution and tool invocation.",
            "The framework is present as infrastructure only; application semantics remain in canonical state and authored surfaces.");
        var builder = new AgentInterfaceSnapshotBuilder(
            new CurrentInterfaceProjectionResolver(
                new DemoStepSurfaceMetadataResolver(),
                new DemoAuthoredAffordanceResolver()));

        var snapshot = builder.Build(state, substrate);

        Assert.Equal(ProjectionTarget.Agent, snapshot.Target);
        Assert.Equal("context-interface-application", snapshot.Root.NodeType);
        Assert.Contains(snapshot.Root.Properties, property => property is { Name: "consumer", Value: "agent" });

        var workflowNode = Assert.Single(snapshot.Root.Children, child => child.NodeType == "workflow");
        var stepNode = Assert.Single(workflowNode.Children, child => child.NodeType == "step");
        var itemNode = Assert.Single(workflowNode.Children, child => child.NodeType == "current-item");
        var sectionsNode = Assert.Single(workflowNode.Children, child => child.NodeType == "step-sections");
        var toolsNode = Assert.Single(workflowNode.Children, child => child.NodeType == "visible-tools");
        var actionsNode = Assert.Single(workflowNode.Children, child => child.NodeType == "available-actions");
        var resultsNode = Assert.Single(workflowNode.Children, child => child.NodeType == "projected-results");
        var runtimeNode = Assert.Single(snapshot.Root.Children, child => child.NodeType == "runtime-substrate");

        Assert.Equal("new-item", stepNode.Id);
        Assert.Equal("item-4821", itemNode.Id);
        Assert.Contains(sectionsNode.Children, child => child.Id == "agent-review-focus");
        Assert.Contains(sectionsNode.Children, child => child.Id == "shared-review-brief");
        Assert.DoesNotContain(sectionsNode.Children, child => child.Id == "reviewer-notes");
        Assert.Contains(toolsNode.Children, child => child.Id == "inspect-item-context");
        Assert.Contains(actionsNode.Children, child => child.Id == "start-review");
        Assert.Contains(toolsNode.Children, child => child.Properties.Any(property => property is { Name: "source-component", Value: "FoundationalDemoAction" }));
        Assert.Contains(actionsNode.Children, child => child.Properties.Any(property => property is { Name: "source-component", Value: "FoundationalDemoAction" }));
        Assert.NotEmpty(resultsNode.Children);
        Assert.Contains(runtimeNode.Properties, property => property is { Name: "package-id", Value: "Microsoft.Agents.Hosting.AspNetCore" });
    }

    [Fact]
    public void CurrentInterfaceProjectionResolver_ComposesCurrentVisibleProjection()
    {
        var state = new DemoWorkflowDefinition().CreateInitialState();
        var resolver = new CurrentInterfaceProjectionResolver(
            new DemoStepSurfaceMetadataResolver(),
            new DemoAuthoredAffordanceResolver());

        var projection = resolver.Resolve(state, ProjectionTarget.Agent);

        Assert.Equal(ProjectionTarget.Agent, projection.Target);
        Assert.Contains(projection.Sections, section => section.Id == "agent-review-focus");
        Assert.DoesNotContain(projection.Sections, section => section.Id == "reviewer-notes");
        Assert.Contains(projection.Tools, tool => tool.Id == "inspect-item-context");
        Assert.Contains(projection.Actions, action => action.ActionId == "start-review");
    }
}
