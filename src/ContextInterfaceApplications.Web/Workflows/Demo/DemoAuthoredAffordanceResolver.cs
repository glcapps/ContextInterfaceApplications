using ContextInterfaceApplications.Web.Components.Shared;
using ContextInterfaceApplications.Web.Models;
using ContextInterfaceApplications.Web.Services;

namespace ContextInterfaceApplications.Web.Workflows.Demo;

public sealed class DemoAuthoredAffordanceResolver : IAuthoredAffordanceResolver
{
    public IReadOnlyList<AuthoredToolContract> GetTools(string stepId) =>
        stepId switch
        {
            "intent-anchoring" or "shared-state-projection" => FoundationalDemoAction.GetAuthoredToolsForStep(stepId),
            "replay-capture" => ReplayCaptureAction.GetAuthoredToolsForStep(stepId),
            _ => []
        };

    public IReadOnlyList<AuthoredActionContract> GetActions(string stepId) =>
        stepId switch
        {
            "intent-anchoring" or "shared-state-projection" => FoundationalDemoAction.GetAuthoredActionsForStep(stepId),
            "replay-capture" => ReplayCaptureAction.GetAuthoredActionsForStep(stepId),
            _ => []
        };
}
