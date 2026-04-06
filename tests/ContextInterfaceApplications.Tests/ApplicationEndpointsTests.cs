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
        Assert.Contains("Current Item", content);
        Assert.Contains("Review Submitted Item", content);
        Assert.Contains("Current Decision", content);
        Assert.Contains("Visible Tools", content);
        Assert.Contains("Reviewer Notes", content);
        Assert.Contains("Shared Review Brief", content);
        Assert.Contains("Review Action", content);
        Assert.Contains("start-review", content);
        Assert.Contains("switch-to-triage-workspace", content);
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
        Assert.Contains("<workflow name=\"Item Review Workspace\">", agentContent);
        Assert.Contains("<review-item id=\"item-4821\" status=\"new\">", agentContent);
        Assert.Contains("<interface-section id=\"agent-review-focus\"", agentContent);
        Assert.Contains("<interface-section id=\"shared-review-brief\"", agentContent);
        Assert.Contains("<available-actions>", agentContent);
        Assert.Contains("action-id=\"start-review\"", agentContent);
        Assert.Contains("action-id=\"switch-to-triage-workspace\"", agentContent);
        Assert.Contains("source-component=\"FoundationalDemoAction\"", agentContent);
        Assert.Contains("step-id=\"new-item\"", agentContent);
        Assert.Contains("<tool id=\"inspect-item-context\" scope=\"application-surface\" source-component=\"FoundationalDemoAction\">", agentContent);

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
        var itemNode = Assert.Single(workflowNode.Children, child => child.NodeType == "current-item");
        var sectionsNode = Assert.Single(workflowNode.Children, child => child.NodeType == "step-sections");
        var toolsNode = Assert.Single(workflowNode.Children, child => child.NodeType == "visible-tools");
        var actionsNode = Assert.Single(workflowNode.Children, child => child.NodeType == "available-actions");
        var runtimeNode = Assert.Single(snapshot.Root.Children, child => child.NodeType == "runtime-substrate");

        Assert.Equal("new-item", stepNode.Id);
        Assert.Equal("item-4821", itemNode.Id);
        Assert.Contains(sectionsNode.Children, child => child.Id == "agent-review-focus");
        Assert.Contains(sectionsNode.Children, child => child.Id == "shared-review-brief");
        Assert.DoesNotContain(sectionsNode.Children, child => child.Id == "reviewer-notes");
        Assert.Contains(toolsNode.Children, child => child.Id == "inspect-item-context");
        Assert.Contains(actionsNode.Children, child => child.Id == "start-review");
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
        Assert.Contains(agentProjection.Sections, section => section.Id == "agent-review-focus");
        Assert.DoesNotContain(agentProjection.Sections, section => section.Id == "reviewer-notes");
        Assert.Contains(humanProjection.Sections, section => section.Id == "reviewer-notes");
        Assert.Contains(agentProjection.Tools, tool => tool.Id == "inspect-item-context");
        Assert.Contains(agentProjection.Actions, action => action.ActionId == "start-review");
        Assert.Contains(agentProjection.Actions, action => action.ActionId == "switch-to-triage-workspace");
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
        Assert.Contains("Review Submitted Item", content);
        Assert.Contains("section:agent-review-focus", content);
        Assert.Contains("tool:inspect-item-context", content);
        Assert.Contains("action:start-review", content);
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
        Assert.Equal("new-item", initialState!.CurrentStep.Id);
        Assert.Equal("item-4821", initialState.CurrentItem.Id);
        Assert.Equal("new", initialState.CurrentItem.Status);

        var actionResponse = await client.PostAsJsonAsync(
            "/api/agent/actions",
            new AgentActionRequest("start-review", initialState.CurrentStep.Id));

        actionResponse.EnsureSuccessStatusCode();

        var actionResult = await actionResponse.Content.ReadFromJsonAsync<AgentActionResult>();
        Assert.NotNull(actionResult);
        Assert.True(actionResult!.Accepted);
        Assert.Equal("in-review", actionResult.CurrentState.CurrentStep.Id);
        Assert.Equal("in_review", actionResult.CurrentState.CurrentItem.Status);
        Assert.Contains("inspect-review-history", actionResult.CurrentState.CurrentVisibleToolIds);
        Assert.DoesNotContain("inspect-item-context", actionResult.CurrentState.CurrentVisibleToolIds);
        Assert.Contains("approve-item", actionResult.CurrentState.CurrentAvailableActionIds);
        Assert.Contains("request-followup", actionResult.CurrentState.CurrentAvailableActionIds);

        var humanSurface = await client.GetStringAsync("/");
        var agentSurface = await client.GetStringAsync("/agent/surface");

        Assert.Contains("Evaluate the item in active review.", humanSurface);
        Assert.Contains("<step id=\"in-review\">", agentSurface);
        Assert.Contains("step-id=\"in-review\"", agentSurface);
        Assert.Contains("Inspect Review History", humanSurface);
        Assert.Contains("<label>Inspect Review History</label>", agentSurface);
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
        Assert.Equal("new-item", result.CurrentState.CurrentStep.Id);
    }

    [Fact]
    public async Task ToolSurface_IsStepScopedAcrossWorkflowTransitions()
    {
        using var client = CreateClient();

        var initialState = await client.GetFromJsonAsync<ContextInterfaceState>("/api/state");
        Assert.NotNull(initialState);
        Assert.Contains("inspect-item-context", initialState!.CurrentVisibleToolIds);
        Assert.DoesNotContain("inspect-review-history", initialState.CurrentVisibleToolIds);
        Assert.DoesNotContain("inspect-followup-guidance", initialState.CurrentVisibleToolIds);
        Assert.Contains("start-review", initialState.CurrentAvailableActionIds);

        var firstAdvance = await client.PostAsJsonAsync(
            "/api/agent/actions",
            new AgentActionRequest("start-review", initialState.CurrentStep.Id));
        firstAdvance.EnsureSuccessStatusCode();

        var sharedProjectionState = await firstAdvance.Content.ReadFromJsonAsync<AgentActionResult>();
        Assert.NotNull(sharedProjectionState);
        Assert.Contains("inspect-review-history", sharedProjectionState!.CurrentState.CurrentVisibleToolIds);
        Assert.DoesNotContain("inspect-item-context", sharedProjectionState.CurrentState.CurrentVisibleToolIds);
        Assert.Contains("approve-item", sharedProjectionState.CurrentState.CurrentAvailableActionIds);
        Assert.Contains("request-followup", sharedProjectionState.CurrentState.CurrentAvailableActionIds);

        var secondAdvance = await client.PostAsJsonAsync(
            "/api/agent/actions",
            new AgentActionRequest("request-followup", sharedProjectionState.CurrentState.CurrentStep.Id));
        secondAdvance.EnsureSuccessStatusCode();

        var replayState = await secondAdvance.Content.ReadFromJsonAsync<AgentActionResult>();
        Assert.NotNull(replayState);
        Assert.Equal("needs-followup", replayState!.CurrentState.CurrentStep.Id);
        Assert.Equal("needs_followup", replayState.CurrentState.CurrentItem.Status);
        Assert.Contains("inspect-followup-guidance", replayState.CurrentState.CurrentVisibleToolIds);
        Assert.DoesNotContain("approve-item", replayState.CurrentState.CurrentAvailableActionIds);
        Assert.Contains("resume-review", replayState.CurrentState.CurrentAvailableActionIds);
        Assert.Contains("switch-to-triage-workspace", replayState.CurrentState.CurrentAvailableActionIds);
    }

    [Fact]
    public async Task WorkspaceSwitch_ReplacesTheVisibleSurfaceInsteadOfAccumulatingIt()
    {
        using var client = CreateClient();

        var initialState = await client.GetFromJsonAsync<ContextInterfaceState>("/api/state");
        Assert.NotNull(initialState);
        Assert.Equal("Item Review Workspace", initialState!.WorkflowName);

        var switchResponse = await client.PostAsJsonAsync(
            "/api/agent/actions",
            new AgentActionRequest("switch-to-triage-workspace", initialState.CurrentStep.Id));
        switchResponse.EnsureSuccessStatusCode();

        var switchResult = await switchResponse.Content.ReadFromJsonAsync<AgentActionResult>();
        Assert.NotNull(switchResult);
        Assert.Equal("Inbox Triage Workspace", switchResult!.CurrentState.WorkflowName);
        Assert.Equal("queued-item", switchResult.CurrentState.CurrentStep.Id);
        Assert.Equal("item-7713", switchResult.CurrentState.CurrentItem.Id);
        Assert.Contains("inspect-inbox-context", switchResult.CurrentState.CurrentVisibleToolIds);
        Assert.DoesNotContain("inspect-item-context", switchResult.CurrentState.CurrentVisibleToolIds);
        Assert.Contains("assign-review", switchResult.CurrentState.CurrentAvailableActionIds);
        Assert.Contains("switch-to-review-workspace", switchResult.CurrentState.CurrentAvailableActionIds);
        Assert.DoesNotContain("start-review", switchResult.CurrentState.CurrentAvailableActionIds);

        var humanSurface = await client.GetStringAsync("/");
        var agentSurface = await client.GetStringAsync("/agent/surface");

        Assert.Contains("Inbox Triage Workspace", humanSurface);
        Assert.Contains("Triage Incoming Item", humanSurface);
        Assert.Contains("assign-review", humanSurface);
        Assert.Contains("switch-to-review-workspace", humanSurface);
        Assert.DoesNotContain("Review Submitted Item", humanSurface);
        Assert.DoesNotContain("start-review", humanSurface);
        Assert.Contains("<workflow name=\"Inbox Triage Workspace\">", agentSurface);
        Assert.Contains("<review-item id=\"item-7713\" status=\"queued\">", agentSurface);
        Assert.Contains("action-id=\"assign-review\"", agentSurface);
        Assert.Contains("action-id=\"switch-to-review-workspace\"", agentSurface);
        Assert.DoesNotContain("action-id=\"start-review\"", agentSurface);
        Assert.DoesNotContain("tool id=\"inspect-item-context\"", agentSurface);
    }

    [Fact]
    public async Task WorkspaceSwitch_CanReturnToTheReviewWorkspace()
    {
        using var client = CreateClient();

        var initialState = await client.GetFromJsonAsync<ContextInterfaceState>("/api/state");
        Assert.NotNull(initialState);

        var switchToTriage = await client.PostAsJsonAsync(
            "/api/agent/actions",
            new AgentActionRequest("switch-to-triage-workspace", initialState!.CurrentStep.Id));
        switchToTriage.EnsureSuccessStatusCode();

        var triageState = await switchToTriage.Content.ReadFromJsonAsync<AgentActionResult>();
        Assert.NotNull(triageState);

        var switchBack = await client.PostAsJsonAsync(
            "/api/agent/actions",
            new AgentActionRequest("switch-to-review-workspace", triageState!.CurrentState.CurrentStep.Id));
        switchBack.EnsureSuccessStatusCode();

        var reviewState = await switchBack.Content.ReadFromJsonAsync<AgentActionResult>();
        Assert.NotNull(reviewState);
        Assert.Equal("Item Review Workspace", reviewState!.CurrentState.WorkflowName);
        Assert.Equal("new-item", reviewState.CurrentState.CurrentStep.Id);
        Assert.Equal("item-4821", reviewState.CurrentState.CurrentItem.Id);
        Assert.Contains("start-review", reviewState.CurrentState.CurrentAvailableActionIds);
        Assert.DoesNotContain("assign-review", reviewState.CurrentState.CurrentAvailableActionIds);
    }

    [Fact]
    public async Task ApprovedStep_UsesApprovalSurfaceAndCanReopenReview()
    {
        using var client = CreateClient();

        var initialState = await client.GetFromJsonAsync<ContextInterfaceState>("/api/state");
        Assert.NotNull(initialState);

        var firstAdvance = await client.PostAsJsonAsync(
            "/api/agent/actions",
            new AgentActionRequest("start-review", initialState!.CurrentStep.Id));
        firstAdvance.EnsureSuccessStatusCode();
        var sharedProjectionState = await firstAdvance.Content.ReadFromJsonAsync<AgentActionResult>();
        Assert.NotNull(sharedProjectionState);

        var secondAdvance = await client.PostAsJsonAsync(
            "/api/agent/actions",
            new AgentActionRequest("approve-item", sharedProjectionState!.CurrentState.CurrentStep.Id));
        secondAdvance.EnsureSuccessStatusCode();

        var approvedHumanSurface = await client.GetStringAsync("/");
        var approvedAgentSurface = await client.GetStringAsync("/agent/surface");

        Assert.Contains("Approval Summary", approvedHumanSurface);
        Assert.Contains("action-id=\"reopen-review\"", approvedAgentSurface);
        Assert.Contains("source-component=\"ReplayCaptureAction\"", approvedAgentSurface);

        var resetResponse = await client.PostAsJsonAsync(
            "/api/agent/actions",
            new AgentActionRequest("reopen-review", "approved"));
        resetResponse.EnsureSuccessStatusCode();

        var resetResult = await resetResponse.Content.ReadFromJsonAsync<AgentActionResult>();
        Assert.NotNull(resetResult);
        Assert.Equal("in-review", resetResult!.CurrentState.CurrentStep.Id);
    }

    [Fact]
    public async Task AcceptedAction_RecordsReplayableTransitionArtifact()
    {
        using var client = CreateClient();

        var initialState = await client.GetFromJsonAsync<ContextInterfaceState>("/api/state");
        Assert.NotNull(initialState);

        var actionResponse = await client.PostAsJsonAsync(
            "/api/agent/actions",
            new AgentActionRequest("start-review", initialState!.CurrentStep.Id));
        actionResponse.EnsureSuccessStatusCode();

        var transitionResponse = await client.GetAsync("/api/replay/transitions/latest");
        transitionResponse.EnsureSuccessStatusCode();

        var transition = await transitionResponse.Content.ReadFromJsonAsync<TransitionArtifact>();
        Assert.NotNull(transition);
        Assert.Equal("start-review", transition!.ActionId);
        Assert.Equal("new-item", transition.FromStepId);
        Assert.Equal("in-review", transition.ToStepId);
        Assert.Contains("step-id=\"new-item\"", transition.BeforeSurface.Content);
        Assert.Contains("step-id=\"in-review\"", transition.AfterSurface.Content);
        Assert.Equal("new-item", transition.BeforeSnapshot.Root.Children.Single(child => child.NodeType == "workflow").Children.Single(child => child.NodeType == "step").Id);
        Assert.Equal("in-review", transition.AfterSnapshot.Root.Children.Single(child => child.NodeType == "workflow").Children.Single(child => child.NodeType == "step").Id);
        Assert.Contains(
            transition.BeforeSnapshot.Root.Children.Single(child => child.NodeType == "workflow").Children.Single(child => child.NodeType == "available-actions").Children,
            child => child.Id == "start-review");
    }

    [Fact]
    public async Task WorkspaceSwitch_RecordsDistinctBeforeAndAfterWorkspacesInReplay()
    {
        using var client = CreateClient();

        var initialState = await client.GetFromJsonAsync<ContextInterfaceState>("/api/state");
        Assert.NotNull(initialState);

        var actionResponse = await client.PostAsJsonAsync(
            "/api/agent/actions",
            new AgentActionRequest("switch-to-triage-workspace", initialState!.CurrentStep.Id));
        actionResponse.EnsureSuccessStatusCode();

        var transitionResponse = await client.GetAsync("/api/replay/transitions/latest");
        transitionResponse.EnsureSuccessStatusCode();

        var transition = await transitionResponse.Content.ReadFromJsonAsync<TransitionArtifact>();
        Assert.NotNull(transition);
        Assert.Equal("switch-to-triage-workspace", transition!.ActionId);
        Assert.Contains("<workflow name=\"Item Review Workspace\">", transition.BeforeSurface.Content);
        Assert.Contains("<workflow name=\"Inbox Triage Workspace\">", transition.AfterSurface.Content);
    }

    [Fact]
    public async Task VisibleToolCall_ProjectsStructuredResultIntoState()
    {
        using var client = CreateClient();

        var toolResponse = await client.PostAsJsonAsync(
            "/api/tools/call",
            new ToolInvocationRequest("inspect-item-context", "new-item"));
        toolResponse.EnsureSuccessStatusCode();

        var toolResult = await toolResponse.Content.ReadFromJsonAsync<ToolInvocationResult>();
        Assert.NotNull(toolResult);
        Assert.True(toolResult!.Succeeded);
        Assert.Equal("inspect-item-context", toolResult.ToolId);
        Assert.NotNull(toolResult.ProjectedResult);
        Assert.Equal("Item context", toolResult.ProjectedResult!.Label);

        var updatedState = await client.GetFromJsonAsync<ContextInterfaceState>("/api/state");
        Assert.NotNull(updatedState);
        Assert.Equal("Item context", updatedState!.RecentResults[0].Label);

        var humanSurface = await client.GetStringAsync("/");
        Assert.Contains("Item context", humanSurface);
    }

    [Fact]
    public async Task HiddenToolCall_IsRejected()
    {
        using var client = CreateClient();

        var toolResponse = await client.PostAsJsonAsync(
            "/api/tools/call",
            new ToolInvocationRequest("inspect-followup-guidance", "new-item"));

        Assert.Equal(HttpStatusCode.Conflict, toolResponse.StatusCode);

        var toolResult = await toolResponse.Content.ReadFromJsonAsync<ToolInvocationResult>();
        Assert.NotNull(toolResult);
        Assert.False(toolResult!.Succeeded);
        Assert.Contains("not visible", toolResult.Message);
    }
}
