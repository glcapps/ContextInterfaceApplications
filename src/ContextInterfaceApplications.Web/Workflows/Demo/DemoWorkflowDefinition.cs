using ContextInterfaceApplications.Web.Components.Shared;
using ContextInterfaceApplications.Web.Models;
using ContextInterfaceApplications.Web.Services;

namespace ContextInterfaceApplications.Web.Workflows.Demo;

public sealed class DemoWorkflowDefinition : IWorkflowDefinition
{
    private readonly IAuthoredAffordanceResolver _affordanceResolver;
    private const string ReviewWorkspaceName = "Item Review Workspace";
    private const string TriageWorkspaceName = "Inbox Triage Workspace";

    public DemoWorkflowDefinition()
        : this(new DemoAuthoredAffordanceResolver())
    {
    }

    public DemoWorkflowDefinition(IAuthoredAffordanceResolver affordanceResolver)
    {
        _affordanceResolver = affordanceResolver;
    }

    public string WorkflowId => "workspace-switching-demo";

    public ContextInterfaceState CreateInitialState() => CreateReviewWorkspaceInitialState();

    public ContextInterfaceState GetNextState(string currentStepId) =>
        currentStepId switch
        {
            "new-item" => CreateReviewInProgressState(),
            "needs-followup" => CreateReviewInProgressState("Resume active review after follow-up.", "Use the additional context to approve the item or request more follow-up."),
            "approved" => CreateReviewInProgressState("Reopened review.", "The item is back in review after being reopened."),
            "queued-item" => CreateTriageRoutedState(),
            "deferred-item" => CreateTriageQueuedState("Resume triage for the deferred item.", "Decide whether to route this item onward or defer it again."),
            "routed-item" => CreateTriageQueuedState("Queue restored.", "The item is back in the triage queue for another routing pass."),
            _ => CreateInitialState()
        };

    public AgentActionResult ApplyAction(ContextInterfaceState currentState, AgentActionRequest request)
    {
        if (!string.Equals(request.StepId, currentState.CurrentStep.Id, StringComparison.Ordinal))
        {
            return new AgentActionResult(
                false,
                $"Rejected action because step '{request.StepId}' is no longer current.",
                currentState);
        }

        if (!currentState.CurrentAvailableActionIds.Contains(request.ActionId, StringComparer.Ordinal))
        {
            return new AgentActionResult(
                false,
                $"Rejected action '{request.ActionId}'. It is not available on the current agent-facing surface.",
                currentState);
        }

        var nextState = request.ActionId switch
        {
            "start-review" => GetNextState(currentState.CurrentStep.Id),
            "approve-item" => BuildState(
                ReviewWorkspaceName,
                currentState.CurrentItem with { Status = "approved" },
                "approved",
                "Item approved.",
                "The item is approved and can be reopened if another pass is needed.",
                "Reopen review."),
            "request-followup" => BuildState(
                ReviewWorkspaceName,
                currentState.CurrentItem with { Status = "needs_followup" },
                "needs-followup",
                "Follow-up requested.",
                "The item needs additional information before approval.",
                "Resume review."),
            "resume-review" => GetNextState(currentState.CurrentStep.Id),
            "reopen-review" => GetNextState(currentState.CurrentStep.Id),
            "assign-review" => CreateTriageRoutedState(),
            "defer-item" => CreateTriageDeferredState(),
            "resume-triage" => CreateTriageQueuedState("Resume triage for the deferred item.", "Decide whether to route this item onward or defer it again."),
            "return-to-queue" => CreateTriageQueuedState("Queue restored.", "The item is back in the triage queue for another routing pass."),
            "switch-to-triage-workspace" => CreateTriageWorkspaceInitialState(),
            "switch-to-review-workspace" => CreateReviewWorkspaceInitialState(),
            _ => currentState
        };

        return new AgentActionResult(
            true,
            $"Accepted action '{request.ActionId}'.",
            nextState);
    }

    private ContextInterfaceState BuildState(
        string workflowName,
        ReviewItem item,
        string stepId,
        string title,
        string decision,
        string nextValidAction)
    {
        return new ContextInterfaceState(
            "Context Interface Applications",
            workflowName,
            item,
            new WorkflowStep(stepId, title, decision, nextValidAction),
            _affordanceResolver.GetTools(stepId).Select(tool => tool.Id).ToArray(),
            _affordanceResolver.GetActions(stepId).Select(action => action.ActionId).ToArray(),
            new[]
            {
                new ProjectedResult(
                    "Item summary",
                    $"{item.Title} is currently marked as {item.Status} in {workflowName}.",
                    DateTimeOffset.UtcNow.AddMinutes(-3)),
                new ProjectedResult(
                    "Workspace guidance",
                    $"The current step '{stepId}' exposes only the actions appropriate for the active workspace.",
                    DateTimeOffset.UtcNow.AddMinutes(-1))
            },
            DateTimeOffset.UtcNow);
    }

    private ContextInterfaceState CreateReviewWorkspaceInitialState() => BuildState(
        ReviewWorkspaceName,
        BuildItem(
            "item-4821",
            "Review Submitted Item",
            "new",
            "A generic submitted item that needs review before it can be approved.",
            "Use the current surface to inspect the item, review prior notes, and choose the next valid review action."),
        "new-item",
        "Review a newly submitted item.",
        "Assess whether the item is ready to enter active review.",
        "Start review.");

    private ContextInterfaceState CreateReviewInProgressState(
        string title = "Evaluate the item in active review.",
        string decision = "Choose whether to approve the item or request follow-up.") => BuildState(
        ReviewWorkspaceName,
        BuildItem(
            "item-4821",
            "Review Submitted Item",
            "in_review",
            "A generic submitted item that is currently under active review.",
            "Use the visible review surface to approve the item, request follow-up, or switch to another workspace."),
        "in-review",
        title,
        decision,
        "Approve item or request follow-up.");

    private ContextInterfaceState CreateTriageWorkspaceInitialState() => CreateTriageQueuedState(
        "Assess a queued item in the inbox.",
        "Choose whether to route the item onward or defer it for later triage.");

    private ContextInterfaceState CreateTriageQueuedState(string title, string decision) => BuildState(
        TriageWorkspaceName,
        BuildItem(
            "item-7713",
            "Triage Incoming Item",
            "queued",
            "A generic queued item waiting for routing from an inbox-style workspace.",
            "Use the current surface to inspect queue context, review routing policy, and choose the next triage action."),
        "queued-item",
        title,
        decision,
        "Assign review or defer item.");

    private ContextInterfaceState CreateTriageDeferredState() => BuildState(
        TriageWorkspaceName,
        BuildItem(
            "item-7713",
            "Triage Incoming Item",
            "deferred",
            "A generic queued item that has been deferred until another triage pass.",
            "Use the current surface to decide when triage should resume or switch back to another workspace."),
        "deferred-item",
        "Deferred item awaiting another triage pass.",
        "Keep the item visible while deferring immediate routing.",
        "Resume triage.");

    private ContextInterfaceState CreateTriageRoutedState() => BuildState(
        TriageWorkspaceName,
        BuildItem(
            "item-7713",
            "Triage Incoming Item",
            "routed",
            "A generic queued item that has been routed onward from triage.",
            "Use the current surface to inspect the routing outcome or return the item to the queue."),
        "routed-item",
        "Item routed onward from triage.",
        "Review the routing outcome and decide whether the item should return to the queue.",
        "Return item to queue.");

    private static ReviewItem BuildItem(string id, string title, string status, string summary, string detail) =>
        new(
            id,
            title,
            status,
            summary,
            detail);
}
