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
        Assert.Contains("<workflow name=\"Proof of Concept Bootstrap\">", artifact.Content);
        Assert.Contains("<name>Microsoft Agent Framework</name>", artifact.Content);
        Assert.Contains("application semantics remain in canonical state", artifact.Content);
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

        Assert.Contains("Anchor the proof of concept", initialHtml);
        Assert.Contains("Define shared canonical state and visible dual surfaces.", updatedHtml);
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
        var builder = new AgentInterfaceSnapshotBuilder(new DemoStepSurfaceMetadataResolver());

        var snapshot = builder.Build(state, substrate);

        Assert.Equal(ProjectionTarget.Agent, snapshot.Target);
        Assert.Equal("context-interface-application", snapshot.Root.NodeType);
        Assert.Contains(snapshot.Root.Properties, property => property is { Name: "consumer", Value: "agent" });

        var workflowNode = Assert.Single(snapshot.Root.Children, child => child.NodeType == "workflow");
        var stepNode = Assert.Single(workflowNode.Children, child => child.NodeType == "step");
        var sectionsNode = Assert.Single(workflowNode.Children, child => child.NodeType == "step-sections");
        var toolsNode = Assert.Single(workflowNode.Children, child => child.NodeType == "visible-tools");
        var actionsNode = Assert.Single(workflowNode.Children, child => child.NodeType == "available-actions");
        var resultsNode = Assert.Single(workflowNode.Children, child => child.NodeType == "projected-results");
        var runtimeNode = Assert.Single(snapshot.Root.Children, child => child.NodeType == "runtime-substrate");

        Assert.Equal("intent-anchoring", stepNode.Id);
        Assert.Contains(sectionsNode.Children, child => child.Id == "agent-only-illustration");
        Assert.Contains(sectionsNode.Children, child => child.Id == "shared-illustration");
        Assert.DoesNotContain(sectionsNode.Children, child => child.Id == "human-only-illustration");
        Assert.Contains(toolsNode.Children, child => child.Id == "runtime-substrate");
        Assert.Contains(actionsNode.Children, child => child.Id == "advance-workflow");
        Assert.NotEmpty(resultsNode.Children);
        Assert.Contains(runtimeNode.Properties, property => property is { Name: "package-id", Value: "Microsoft.Agents.Hosting.AspNetCore" });
    }
}
