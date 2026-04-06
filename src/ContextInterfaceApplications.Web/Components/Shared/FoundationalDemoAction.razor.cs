using ContextInterfaceApplications.Web.Models;
using Microsoft.AspNetCore.Components;

namespace ContextInterfaceApplications.Web.Components.Shared;

public partial class FoundationalDemoAction
{
    public static IReadOnlyList<AuthoredToolContract> GetAuthoredToolsForStep(string stepId) =>
        stepId switch
        {
            "new-item" =>
            [
                new AuthoredToolContract(
                    "inspect-item-context",
                    "Inspect Item Context",
                    "application-surface",
                    "Review the summary and detail currently visible for this item.",
                    "FoundationalDemoAction"),
                new AuthoredToolContract(
                    "inspect-review-policy",
                    "Inspect Review Policy",
                    "application-surface",
                    "Review the generic criteria used to start item review.",
                    "FoundationalDemoAction")
            ],
            "in-review" =>
            [
                new AuthoredToolContract(
                    "inspect-review-history",
                    "Inspect Review History",
                    "application-surface",
                    "Review the most recent projected notes and prior review context.",
                    "FoundationalDemoAction"),
                new AuthoredToolContract(
                    "inspect-review-policy",
                    "Inspect Review Policy",
                    "application-surface",
                    "Review the generic criteria used to approve or request follow-up.",
                    "FoundationalDemoAction")
            ],
            _ => []
        };

    public static IReadOnlyList<AuthoredActionContract> GetAuthoredActionsForStep(string stepId) =>
        stepId switch
        {
            "new-item" =>
            [
                new AuthoredActionContract(
                    "start-review",
                    "new-item",
                    "Start reviewing this item from its initial new state.",
                    "The item moves into active review with approval and follow-up decisions available.",
                    "FoundationalDemoAction")
            ],
            "in-review" =>
            [
                new AuthoredActionContract(
                    "approve-item",
                    "in-review",
                    "Approve the item and complete the current review pass.",
                    "The item moves into the approved state.",
                    "FoundationalDemoAction"),
                new AuthoredActionContract(
                    "request-followup",
                    "in-review",
                    "Request more information or changes before final approval.",
                    "The item moves into a needs-follow-up state.",
                    "FoundationalDemoAction")
            ],
            _ => []
        };

    [Parameter, EditorRequired]
    public required AuthoredActionContract Action { get; set; }

    [Parameter]
    public ProjectionTarget Target { get; set; }
}
