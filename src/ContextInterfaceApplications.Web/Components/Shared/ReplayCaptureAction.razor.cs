using ContextInterfaceApplications.Web.Models;
using Microsoft.AspNetCore.Components;

namespace ContextInterfaceApplications.Web.Components.Shared;

public partial class ReplayCaptureAction
{
    public static IReadOnlyList<AuthoredToolContract> GetAuthoredToolsForStep(string stepId) =>
        stepId switch
        {
            "replay-capture" =>
            [
                new AuthoredToolContract(
                    "inspect-replay",
                    "Inspect Replay Artifact",
                    "application-surface",
                    "Retrieve the latest agent-visible interface snapshot.",
                    "ReplayCaptureAction"),
                new AuthoredToolContract(
                    "reset-workflow",
                    "Reset Workflow",
                    "application-surface",
                    "Return to the first step after validating replay capture.",
                    "ReplayCaptureAction")
            ],
            _ => []
        };

    public static IReadOnlyList<AuthoredActionContract> GetAuthoredActionsForStep(string stepId) =>
        stepId switch
        {
            "replay-capture" =>
            [
                new AuthoredActionContract(
                    "reset-workflow",
                    "replay-capture",
                    "Reset the workflow after replay inspection back to the first step.",
                    "The next rendered surfaces return to intent anchoring.",
                    "ReplayCaptureAction")
            ],
            _ => []
        };

    [Parameter, EditorRequired]
    public required AuthoredActionContract Action { get; set; }

    [Parameter]
    public ProjectionTarget Target { get; set; }
}
