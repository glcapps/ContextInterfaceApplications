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
            _ => []
        };

    public IReadOnlyList<AuthoredActionContract> GetActions(string stepId) =>
        stepId switch
        {
            "new-item" or "in-review" => FoundationalDemoAction.GetAuthoredActionsForStep(stepId),
            "needs-followup" or "approved" => ReplayCaptureAction.GetAuthoredActionsForStep(stepId),
            _ => []
        };
}
