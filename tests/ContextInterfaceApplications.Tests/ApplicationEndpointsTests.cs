using System.Net;
using System.Net.Http.Json;
using ContextInterfaceApplications.Web;
using ContextInterfaceApplications.Web.Models;
using Microsoft.AspNetCore.Mvc.Testing;

namespace ContextInterfaceApplications.Tests;

public sealed class ApplicationEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public ApplicationEndpointsTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    private HttpClient CreateClient() => _factory.WithWebHostBuilder(_ => { }).CreateClient();

    [Fact]
    public async Task Root_RendersHumanSurfaceThroughBlazorComponents()
    {
        using var client = CreateClient();

        var response = await client.GetAsync("/");
        var content = await response.Content.ReadAsStringAsync();

        response.EnsureSuccessStatusCode();
        Assert.Equal("text/html", response.Content.Headers.ContentType?.MediaType);
        Assert.Contains("<!DOCTYPE html>", content);
        Assert.Contains("Current Decision", content);
        Assert.Contains("Visible Tools", content);
        Assert.Contains("Human-Only Illustration", content);
        Assert.Contains("Shared Illustration", content);
        Assert.Contains("Foundational Demo Action", content);
        Assert.Contains("advance-workflow", content);
    }

    [Fact]
    public async Task AgentSurface_RendersAndRecordsReplayArtifact()
    {
        using var client = CreateClient();

        var agentResponse = await client.GetAsync("/agent/surface");
        var agentContent = await agentResponse.Content.ReadAsStringAsync();

        agentResponse.EnsureSuccessStatusCode();
        Assert.Equal("application/xhtml+xml", agentResponse.Content.Headers.ContentType?.MediaType);
        Assert.Contains("<context-interface-application consumer=\"agent\">", agentContent);
        Assert.Contains("<workflow name=\"Proof of Concept Bootstrap\">", agentContent);
        Assert.Contains("<interface-section id=\"agent-only-illustration\"", agentContent);
        Assert.Contains("<interface-section id=\"shared-illustration\"", agentContent);
        Assert.Contains("<available-actions>", agentContent);
        Assert.Contains("action-id=\"advance-workflow\"", agentContent);
        Assert.Contains("source-component=\"FoundationalDemoAction\"", agentContent);
        Assert.Contains("step-id=\"intent-anchoring\"", agentContent);
        Assert.Contains("<tool id=\"advance-workflow\" scope=\"application-surface\" source-component=\"FoundationalDemoAction\">", agentContent);

        var replayResponse = await client.GetAsync("/api/replay/latest");
        var replayContent = await replayResponse.Content.ReadAsStringAsync();

        replayResponse.EnsureSuccessStatusCode();
        Assert.Contains("context-interface-application", replayContent);
        Assert.Contains("Microsoft Agent Framework", replayContent);
    }

    [Fact]
    public async Task AgentSnapshot_ReturnsStructuredCurrentSurface()
    {
        using var client = CreateClient();

        var snapshot = await client.GetFromJsonAsync<InterfaceSnapshot>("/api/agent/snapshot");

        Assert.NotNull(snapshot);
        Assert.Equal(ProjectionTarget.Agent, snapshot!.Target);
        Assert.Equal("Context Interface Applications", snapshot.ApplicationName);
        Assert.Equal("context-interface-application", snapshot.Root.NodeType);

        var workflowNode = Assert.Single(snapshot.Root.Children, child => child.NodeType == "workflow");
        var stepNode = Assert.Single(workflowNode.Children, child => child.NodeType == "step");
        var sectionsNode = Assert.Single(workflowNode.Children, child => child.NodeType == "step-sections");
        var toolsNode = Assert.Single(workflowNode.Children, child => child.NodeType == "visible-tools");
        var actionsNode = Assert.Single(workflowNode.Children, child => child.NodeType == "available-actions");
        var runtimeNode = Assert.Single(snapshot.Root.Children, child => child.NodeType == "runtime-substrate");

        Assert.Equal("intent-anchoring", stepNode.Id);
        Assert.Contains(sectionsNode.Children, child => child.Id == "agent-only-illustration");
        Assert.Contains(sectionsNode.Children, child => child.Id == "shared-illustration");
        Assert.DoesNotContain(sectionsNode.Children, child => child.Id == "human-only-illustration");
        Assert.Contains(toolsNode.Children, child => child.Id == "runtime-substrate");
        Assert.Contains(actionsNode.Children, child => child.Id == "advance-workflow");
        Assert.Contains(runtimeNode.Properties, property => property is { Name: "version", Value: not null });
    }

    [Fact]
    public async Task ProjectionEndpoints_ReturnCurrentVisibleProjectionForEachTarget()
    {
        using var client = CreateClient();

        var agentProjection = await client.GetFromJsonAsync<CurrentInterfaceProjection>("/api/projections/agent");
        var humanProjection = await client.GetFromJsonAsync<CurrentInterfaceProjection>("/api/projections/human");

        Assert.NotNull(agentProjection);
        Assert.NotNull(humanProjection);
        Assert.Equal(ProjectionTarget.Agent, agentProjection!.Target);
        Assert.Equal(ProjectionTarget.Human, humanProjection!.Target);
        Assert.Contains(agentProjection.Sections, section => section.Id == "agent-only-illustration");
        Assert.DoesNotContain(agentProjection.Sections, section => section.Id == "human-only-illustration");
        Assert.Contains(humanProjection.Sections, section => section.Id == "human-only-illustration");
        Assert.Contains(agentProjection.Tools, tool => tool.Id == "runtime-substrate");
        Assert.Contains(agentProjection.Actions, action => action.ActionId == "advance-workflow");
    }

    [Fact]
    public async Task AgentDebugSurface_RendersEscapedAgentMarkupInHtmlShell()
    {
        using var client = CreateClient();

        var response = await client.GetAsync("/debug/agent/surface");
        var content = await response.Content.ReadAsStringAsync();

        response.EnsureSuccessStatusCode();
        Assert.Equal("text/html", response.Content.Headers.ContentType?.MediaType);
        Assert.Contains("Agent Surface Debug", content);
        Assert.Contains("Current Projection", content);
        Assert.Contains("section:agent-only-illustration", content);
        Assert.Contains("tool:runtime-substrate", content);
        Assert.Contains("action:advance-workflow", content);
        Assert.Contains("&lt;context-interface-application consumer=&quot;agent&quot;&gt;", content);
        Assert.Contains("current agent-facing payload", content);
    }

    [Fact]
    public async Task AgentDebugSurface_AllowsRawMarkupMode()
    {
        using var client = CreateClient();

        var response = await client.GetAsync("/debug/agent/surface?escaped=false");
        var content = await response.Content.ReadAsStringAsync();

        response.EnsureSuccessStatusCode();
        Assert.Equal("text/html", response.Content.Headers.ContentType?.MediaType);
        Assert.Contains("raw markup mode", content);
        Assert.Contains("<context-interface-application consumer=\"agent\">", content);
        Assert.DoesNotContain("&lt;context-interface-application", content);
    }

    [Fact]
    public async Task AgentAction_AdvancesCanonicalStateAcrossBothProjections()
    {
        using var client = CreateClient();

        var initialState = await client.GetFromJsonAsync<ContextInterfaceState>("/api/state");
        Assert.NotNull(initialState);
        Assert.Equal("intent-anchoring", initialState!.CurrentStep.Id);

        var actionResponse = await client.PostAsJsonAsync(
            "/api/agent/actions",
            new AgentActionRequest("advance-workflow", initialState.CurrentStep.Id));

        actionResponse.EnsureSuccessStatusCode();

        var actionResult = await actionResponse.Content.ReadFromJsonAsync<AgentActionResult>();
        Assert.NotNull(actionResult);
        Assert.True(actionResult!.Accepted);
        Assert.Equal("shared-state-projection", actionResult.CurrentState.CurrentStep.Id);
        Assert.Contains("inspect-dual-projection", actionResult.CurrentState.CurrentVisibleToolIds);
        Assert.DoesNotContain("runtime-substrate", actionResult.CurrentState.CurrentVisibleToolIds);
        Assert.Contains("advance-workflow", actionResult.CurrentState.CurrentAvailableActionIds);

        var humanSurface = await client.GetStringAsync("/");
        var agentSurface = await client.GetStringAsync("/agent/surface");

        Assert.Contains("Define shared canonical state and visible dual surfaces.", humanSurface);
        Assert.Contains("<step id=\"shared-state-projection\">", agentSurface);
        Assert.Contains("step-id=\"shared-state-projection\"", agentSurface);
        Assert.Contains("Inspect Dual Projection", humanSurface);
        Assert.Contains("<label>Inspect Dual Projection</label>", agentSurface);
        Assert.Contains("source-component=\"FoundationalDemoAction\"", agentSurface);
    }

    [Fact]
    public async Task AgentAction_RejectsStaleStepId()
    {
        using var client = CreateClient();

        var response = await client.PostAsJsonAsync(
            "/api/agent/actions",
            new AgentActionRequest("advance-workflow", "stale-step"));

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<AgentActionResult>();
        Assert.NotNull(result);
        Assert.False(result!.Accepted);
        Assert.Contains("no longer current", result.Message);
        Assert.Equal("intent-anchoring", result.CurrentState.CurrentStep.Id);
    }

    [Fact]
    public async Task ToolSurface_IsStepScopedAcrossWorkflowTransitions()
    {
        using var client = CreateClient();

        var initialState = await client.GetFromJsonAsync<ContextInterfaceState>("/api/state");
        Assert.NotNull(initialState);
        Assert.Contains("runtime-substrate", initialState!.CurrentVisibleToolIds);
        Assert.DoesNotContain("inspect-dual-projection", initialState.CurrentVisibleToolIds);
        Assert.DoesNotContain("inspect-replay", initialState.CurrentVisibleToolIds);
        Assert.Contains("advance-workflow", initialState.CurrentAvailableActionIds);

        var firstAdvance = await client.PostAsJsonAsync(
            "/api/agent/actions",
            new AgentActionRequest("advance-workflow", initialState.CurrentStep.Id));
        firstAdvance.EnsureSuccessStatusCode();

        var sharedProjectionState = await firstAdvance.Content.ReadFromJsonAsync<AgentActionResult>();
        Assert.NotNull(sharedProjectionState);
        Assert.Contains("inspect-dual-projection", sharedProjectionState!.CurrentState.CurrentVisibleToolIds);
        Assert.DoesNotContain("runtime-substrate", sharedProjectionState.CurrentState.CurrentVisibleToolIds);
        Assert.Contains("advance-workflow", sharedProjectionState.CurrentState.CurrentAvailableActionIds);

        var secondAdvance = await client.PostAsJsonAsync(
            "/api/agent/actions",
            new AgentActionRequest("advance-workflow", sharedProjectionState.CurrentState.CurrentStep.Id));
        secondAdvance.EnsureSuccessStatusCode();

        var replayState = await secondAdvance.Content.ReadFromJsonAsync<AgentActionResult>();
        Assert.NotNull(replayState);
        Assert.Equal("replay-capture", replayState!.CurrentState.CurrentStep.Id);
        Assert.Contains("inspect-replay", replayState.CurrentState.CurrentVisibleToolIds);
        Assert.DoesNotContain("advance-workflow", replayState.CurrentState.CurrentVisibleToolIds);
        Assert.Contains("reset-workflow", replayState.CurrentState.CurrentAvailableActionIds);
    }

    [Fact]
    public async Task ReplayStep_UsesReplayCaptureComponentAndCanResetWorkflow()
    {
        using var client = CreateClient();

        var initialState = await client.GetFromJsonAsync<ContextInterfaceState>("/api/state");
        Assert.NotNull(initialState);

        var firstAdvance = await client.PostAsJsonAsync(
            "/api/agent/actions",
            new AgentActionRequest("advance-workflow", initialState!.CurrentStep.Id));
        firstAdvance.EnsureSuccessStatusCode();
        var sharedProjectionState = await firstAdvance.Content.ReadFromJsonAsync<AgentActionResult>();
        Assert.NotNull(sharedProjectionState);

        var secondAdvance = await client.PostAsJsonAsync(
            "/api/agent/actions",
            new AgentActionRequest("advance-workflow", sharedProjectionState!.CurrentState.CurrentStep.Id));
        secondAdvance.EnsureSuccessStatusCode();

        var replayHumanSurface = await client.GetStringAsync("/");
        var replayAgentSurface = await client.GetStringAsync("/agent/surface");

        Assert.Contains("Replay Capture Action", replayHumanSurface);
        Assert.Contains("action-id=\"reset-workflow\"", replayAgentSurface);
        Assert.Contains("source-component=\"ReplayCaptureAction\"", replayAgentSurface);

        var resetResponse = await client.PostAsJsonAsync(
            "/api/agent/actions",
            new AgentActionRequest("reset-workflow", "replay-capture"));
        resetResponse.EnsureSuccessStatusCode();

        var resetResult = await resetResponse.Content.ReadFromJsonAsync<AgentActionResult>();
        Assert.NotNull(resetResult);
        Assert.Equal("intent-anchoring", resetResult!.CurrentState.CurrentStep.Id);
    }

    [Fact]
    public async Task AcceptedAction_RecordsReplayableTransitionArtifact()
    {
        using var client = CreateClient();

        var initialState = await client.GetFromJsonAsync<ContextInterfaceState>("/api/state");
        Assert.NotNull(initialState);

        var actionResponse = await client.PostAsJsonAsync(
            "/api/agent/actions",
            new AgentActionRequest("advance-workflow", initialState!.CurrentStep.Id));
        actionResponse.EnsureSuccessStatusCode();

        var transitionResponse = await client.GetAsync("/api/replay/transitions/latest");
        transitionResponse.EnsureSuccessStatusCode();

        var transition = await transitionResponse.Content.ReadFromJsonAsync<TransitionArtifact>();
        Assert.NotNull(transition);
        Assert.Equal("advance-workflow", transition!.ActionId);
        Assert.Equal("intent-anchoring", transition.FromStepId);
        Assert.Equal("shared-state-projection", transition.ToStepId);
        Assert.Contains("step-id=\"intent-anchoring\"", transition.BeforeSurface.Content);
        Assert.Contains("step-id=\"shared-state-projection\"", transition.AfterSurface.Content);
        Assert.Equal("intent-anchoring", transition.BeforeSnapshot.Root.Children.Single(child => child.NodeType == "workflow").Children.Single(child => child.NodeType == "step").Id);
        Assert.Equal("shared-state-projection", transition.AfterSnapshot.Root.Children.Single(child => child.NodeType == "workflow").Children.Single(child => child.NodeType == "step").Id);
        Assert.Contains(
            transition.BeforeSnapshot.Root.Children.Single(child => child.NodeType == "workflow").Children.Single(child => child.NodeType == "available-actions").Children,
            child => child.Id == "advance-workflow");
    }

    [Fact]
    public async Task VisibleToolCall_ProjectsStructuredResultIntoState()
    {
        using var client = CreateClient();

        var toolResponse = await client.PostAsJsonAsync(
            "/api/tools/call",
            new ToolInvocationRequest("runtime-substrate", "intent-anchoring"));
        toolResponse.EnsureSuccessStatusCode();

        var toolResult = await toolResponse.Content.ReadFromJsonAsync<ToolInvocationResult>();
        Assert.NotNull(toolResult);
        Assert.True(toolResult!.Succeeded);
        Assert.Equal("runtime-substrate", toolResult.ToolId);
        Assert.NotNull(toolResult.ProjectedResult);
        Assert.Equal("Runtime substrate", toolResult.ProjectedResult!.Label);

        var updatedState = await client.GetFromJsonAsync<ContextInterfaceState>("/api/state");
        Assert.NotNull(updatedState);
        Assert.Equal("Runtime substrate", updatedState!.RecentResults[0].Label);

        var humanSurface = await client.GetStringAsync("/");
        Assert.Contains("Runtime substrate", humanSurface);
    }

    [Fact]
    public async Task HiddenToolCall_IsRejected()
    {
        using var client = CreateClient();

        var toolResponse = await client.PostAsJsonAsync(
            "/api/tools/call",
            new ToolInvocationRequest("inspect-replay", "intent-anchoring"));

        Assert.Equal(HttpStatusCode.Conflict, toolResponse.StatusCode);

        var toolResult = await toolResponse.Content.ReadFromJsonAsync<ToolInvocationResult>();
        Assert.NotNull(toolResult);
        Assert.False(toolResult!.Succeeded);
        Assert.Contains("not visible", toolResult.Message);
    }
}
