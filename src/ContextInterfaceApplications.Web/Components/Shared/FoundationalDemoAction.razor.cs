using ContextInterfaceApplications.Web.Models;
using Microsoft.AspNetCore.Components;

namespace ContextInterfaceApplications.Web.Components.Shared;

public partial class FoundationalDemoAction
{
    public static IReadOnlyList<AuthoredToolContract> GetAuthoredToolsForStep(string stepId) =>
        stepId switch
        {
            "intent-anchoring" =>
            [
                new AuthoredToolContract(
                    "advance-workflow",
                    "Advance Workflow",
                    "application-surface",
                    "Move to the next explicit step in the proof-of-concept flow.",
                    "FoundationalDemoAction"),
                new AuthoredToolContract(
                    "runtime-substrate",
                    "Inspect Runtime Substrate",
                    "runtime",
                    "Check the Microsoft Agent Framework registration without handing it workflow ownership.",
                    "FoundationalDemoAction")
            ],
            "shared-state-projection" =>
            [
                new AuthoredToolContract(
                    "advance-workflow",
                    "Advance Workflow",
                    "application-surface",
                    "Move from shared-state projection to replay capture.",
                    "FoundationalDemoAction"),
                new AuthoredToolContract(
                    "inspect-dual-projection",
                    "Inspect Dual Projection",
                    "application-surface",
                    "Compare the shared human and agent render targets for the current step.",
                    "FoundationalDemoAction")
            ],
            _ => []
        };

    public static IReadOnlyList<AuthoredActionContract> GetAuthoredActionsForStep(string stepId) =>
        stepId switch
        {
            "intent-anchoring" =>
            [
                new AuthoredActionContract(
                    "advance-workflow",
                    "intent-anchoring",
                    "Advance from intent anchoring into the shared-state projection step.",
                    "The next rendered surfaces expose dual-projection inspection.",
                    "FoundationalDemoAction")
            ],
            "shared-state-projection" =>
            [
                new AuthoredActionContract(
                    "advance-workflow",
                    "shared-state-projection",
                    "Advance from shared-state projection into replay capture.",
                    "The next rendered surfaces expose replay inspection instead of projection comparison.",
                    "FoundationalDemoAction")
            ],
            _ => []
        };

    [Parameter, EditorRequired]
    public required AuthoredActionContract Action { get; set; }

    [Parameter]
    public ProjectionTarget Target { get; set; }
}
