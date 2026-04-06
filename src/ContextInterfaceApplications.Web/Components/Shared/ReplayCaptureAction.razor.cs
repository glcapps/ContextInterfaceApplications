using ContextInterfaceApplications.Web.Models;
using Microsoft.AspNetCore.Components;

namespace ContextInterfaceApplications.Web.Components.Shared;

public partial class ReplayCaptureAction
{
    public static IReadOnlyList<AuthoredToolContract> GetAuthoredToolsForStep(string stepId) =>
        stepId switch
        {
            "needs-followup" =>
            [
                new AuthoredToolContract(
                    "inspect-followup-guidance",
                    "Inspect Follow-Up Guidance",
                    "application-surface",
                    "Review the current follow-up expectation before resuming review.",
                    "ReplayCaptureAction"),
                new AuthoredToolContract(
                    "inspect-review-history",
                    "Inspect Review History",
                    "application-surface",
                    "Review the recent projected review notes while follow-up is pending.",
                    "ReplayCaptureAction")
            ],
            "approved" =>
            [
                new AuthoredToolContract(
                    "inspect-review-history",
                    "Inspect Review History",
                    "application-surface",
                    "Review the projected results that led to approval.",
                    "ReplayCaptureAction"),
                new AuthoredToolContract(
                    "inspect-item-context",
                    "Inspect Item Context",
                    "application-surface",
                    "Re-read the approved item summary and detail.",
                    "ReplayCaptureAction")
            ],
            _ => []
        };

    public static IReadOnlyList<AuthoredActionContract> GetAuthoredActionsForStep(string stepId) =>
        stepId switch
        {
            "needs-followup" =>
            [
                new AuthoredActionContract(
                    "resume-review",
                    "needs-followup",
                    "Resume review after follow-up information is considered ready.",
                    "The item returns to active review.",
                    "ReplayCaptureAction")
            ],
            "approved" =>
            [
                new AuthoredActionContract(
                    "reopen-review",
                    "approved",
                    "Reopen the item for another review pass.",
                    "The item returns to active review.",
                    "ReplayCaptureAction")
            ],
            _ => []
        };

    [Parameter, EditorRequired]
    public required AuthoredActionContract Action { get; set; }

    [Parameter]
    public ProjectionTarget Target { get; set; }
}
