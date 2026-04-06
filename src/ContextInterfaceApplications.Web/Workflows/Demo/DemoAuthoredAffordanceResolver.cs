using ContextInterfaceApplications.Web.Components.Shared;
using ContextInterfaceApplications.Web.Models;
using ContextInterfaceApplications.Web.Services;

namespace ContextInterfaceApplications.Web.Workflows.Demo;

public sealed class DemoAuthoredAffordanceResolver : IAuthoredAffordanceResolver
{
    public IReadOnlyList<AuthoredToolContract> GetTools(string stepId) =>
        stepId switch
        {
            "new-item" or "in-review" => FoundationalDemoAction.GetAuthoredToolsForStep(stepId),
            "needs-followup" or "approved" => ReplayCaptureAction.GetAuthoredToolsForStep(stepId),
            "queued-item" or "deferred-item" or "routed-item" => TriageWorkspaceAction.GetAuthoredToolsForStep(stepId),
            _ => []
        };

    public IReadOnlyList<AuthoredActionContract> GetActions(string stepId)
    {
        var stepActions = stepId switch
        {
            "new-item" or "in-review" => FoundationalDemoAction.GetAuthoredActionsForStep(stepId),
            "needs-followup" or "approved" => ReplayCaptureAction.GetAuthoredActionsForStep(stepId),
            "queued-item" or "deferred-item" or "routed-item" => TriageWorkspaceAction.GetAuthoredActionsForStep(stepId),
            _ => []
        };

        return stepActions
            .Concat(WorkspaceSwitchAction.GetAuthoredActionsForStep(stepId))
            .ToArray();
    }
}
