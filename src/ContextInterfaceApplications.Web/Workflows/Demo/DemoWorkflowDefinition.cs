using ContextInterfaceApplications.Web.Components.Shared;
using ContextInterfaceApplications.Web.Models;
using ContextInterfaceApplications.Web.Services;

namespace ContextInterfaceApplications.Web.Workflows.Demo;

public sealed class DemoWorkflowDefinition : IWorkflowDefinition
{
    public string WorkflowId => "demo-bootstrap";

    public ContextInterfaceState CreateInitialState() => BuildState(
        "intent-anchoring",
        "Anchor the proof of concept to the workflow, not the framework.",
        "Keep MAF in the runtime layer and keep interface semantics in application state.",
        "Advance to the shared projection step.");

    public ContextInterfaceState GetNextState(string currentStepId) =>
        currentStepId switch
        {
            "intent-anchoring" => BuildState(
                "shared-state-projection",
                "Define shared canonical state and visible dual surfaces.",
                "Render a human view and an agent view from the same state object.",
                "Promote the projection pipeline to replay capture."),
            "shared-state-projection" => BuildState(
                "replay-capture",
                "Capture what the agent actually saw.",
                "Persist rendered agent-facing artifacts as replay material.",
                "Inspect the latest replay artifact."),
            _ => CreateInitialState()
        };

    public AgentActionResult ApplyAction(ContextInterfaceState currentState, AgentActionRequest request)
    {
        var matchingAction = currentState.AvailableAgentActions
            .SingleOrDefault(action => string.Equals(action.ActionId, request.ActionId, StringComparison.Ordinal));

        if (!string.Equals(request.StepId, currentState.CurrentStep.Id, StringComparison.Ordinal))
        {
            return new AgentActionResult(
                false,
                $"Rejected action because step '{request.StepId}' is no longer current.",
                currentState);
        }

        if (matchingAction is null)
        {
            return new AgentActionResult(
                false,
                $"Rejected action '{request.ActionId}'. It is not available on the current agent-facing surface.",
                currentState);
        }

        var nextState = request.ActionId switch
        {
            "advance-workflow" => GetNextState(currentState.CurrentStep.Id),
            "reset-workflow" => CreateInitialState(),
            _ => currentState
        };

        return new AgentActionResult(
            true,
            $"Accepted action '{request.ActionId}'.",
            nextState);
    }

    private static ContextInterfaceState BuildState(
        string stepId,
        string title,
        string decision,
        string nextValidAction)
    {
        return new ContextInterfaceState(
            "Context Interface Applications",
            "Proof of Concept Bootstrap",
            new WorkflowStep(stepId, title, decision, nextValidAction),
            GetVisibleTools(stepId),
            GetAvailableAgentActions(stepId),
            new[]
            {
                new ProjectedResult(
                    "Canonical state",
                    "Shared application state is the single source for both human and agent projections.",
                    DateTimeOffset.UtcNow.AddMinutes(-3)),
                new ProjectedResult(
                    "Current constraint",
                    "The visible interface is the payload. No alternate hidden context object exists.",
                    DateTimeOffset.UtcNow.AddMinutes(-1))
            },
            DateTimeOffset.UtcNow);
    }

    private static IReadOnlyList<VisibleTool> GetVisibleTools(string stepId) =>
        stepId switch
        {
            "intent-anchoring" or "shared-state-projection" => FoundationalDemoAction.GetToolsForStep(stepId),
            "replay-capture" => ReplayCaptureAction.GetToolsForStep(stepId),
            _ =>
            [
                new VisibleTool(
                    "advance-workflow",
                    "Advance Workflow",
                    "application-surface",
                    "Move to the next explicit step in the proof-of-concept flow.",
                    nameof(DemoWorkflowDefinition))
            ]
        };

    private static IReadOnlyList<AgentActionDescriptor> GetAvailableAgentActions(string stepId) =>
        stepId switch
        {
            "intent-anchoring" or "shared-state-projection" => FoundationalDemoAction.GetContractsForStep(stepId),
            "replay-capture" => ReplayCaptureAction.GetContractsForStep(stepId),
            _ => []
        };
}
