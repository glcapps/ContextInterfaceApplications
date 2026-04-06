using ContextInterfaceApplications.Web.Components.Shared;
using ContextInterfaceApplications.Web.Models;
using ContextInterfaceApplications.Web.Services;

namespace ContextInterfaceApplications.Web.Workflows.Demo;

public sealed class DemoWorkflowDefinition : IWorkflowDefinition
{
    private readonly IAuthoredAffordanceResolver _affordanceResolver;

    public DemoWorkflowDefinition()
        : this(new DemoAuthoredAffordanceResolver())
    {
    }

    public DemoWorkflowDefinition(IAuthoredAffordanceResolver affordanceResolver)
    {
        _affordanceResolver = affordanceResolver;
    }

    public string WorkflowId => "item-review-workspace";

    public ContextInterfaceState CreateInitialState() => BuildState(
        BuildItem("item-4821", "Review Submitted Item", "new"),
        "new-item",
        "Review a newly submitted item.",
        "Assess whether the item is ready to enter active review.",
        "Start review.");

    public ContextInterfaceState GetNextState(string currentStepId) =>
        currentStepId switch
        {
            "new-item" => BuildState(
                BuildItem("item-4821", "Review Submitted Item", "in_review"),
                "in-review",
                "Evaluate the item in active review.",
                "Choose whether to approve the item or request follow-up.",
                "Approve item or request follow-up."),
            "needs-followup" => BuildState(
                BuildItem("item-4821", "Review Submitted Item", "in_review"),
                "in-review",
                "Resume active review after follow-up.",
                "Use the additional context to approve the item or request more follow-up.",
                "Approve item or request follow-up."),
            "approved" => BuildState(
                BuildItem("item-4821", "Review Submitted Item", "in_review"),
                "in-review",
                "Reopened review.",
                "The item is back in review after being reopened.",
                "Approve item or request follow-up."),
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
                currentState.CurrentItem with { Status = "approved" },
                "approved",
                "Item approved.",
                "The item is approved and can be reopened if another pass is needed.",
                "Reopen review."),
            "request-followup" => BuildState(
                currentState.CurrentItem with { Status = "needs_followup" },
                "needs-followup",
                "Follow-up requested.",
                "The item needs additional information before approval.",
                "Resume review."),
            "resume-review" => GetNextState(currentState.CurrentStep.Id),
            "reopen-review" => GetNextState(currentState.CurrentStep.Id),
            _ => currentState
        };

        return new AgentActionResult(
            true,
            $"Accepted action '{request.ActionId}'.",
            nextState);
    }

    private ContextInterfaceState BuildState(
        ReviewItem item,
        string stepId,
        string title,
        string decision,
        string nextValidAction)
    {
        return new ContextInterfaceState(
            "Context Interface Applications",
            "Item Review Workspace",
            item,
            new WorkflowStep(stepId, title, decision, nextValidAction),
            _affordanceResolver.GetTools(stepId).Select(tool => tool.Id).ToArray(),
            _affordanceResolver.GetActions(stepId).Select(action => action.ActionId).ToArray(),
            new[]
            {
                new ProjectedResult(
                    "Item summary",
                    $"{item.Title} is currently marked as {item.Status}.",
                    DateTimeOffset.UtcNow.AddMinutes(-3)),
                new ProjectedResult(
                    "Review guidance",
                    $"The current step '{stepId}' exposes only the actions appropriate for this review state.",
                    DateTimeOffset.UtcNow.AddMinutes(-1))
            },
            DateTimeOffset.UtcNow);
    }

    private static ReviewItem BuildItem(string id, string title, string status) =>
        new(
            id,
            title,
            status,
            "A generic submitted item that needs review before it can be approved.",
            "Use the current surface to inspect the item, review prior notes, and choose the next valid review action.");
}
