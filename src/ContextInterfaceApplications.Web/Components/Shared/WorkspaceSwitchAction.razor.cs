using ContextInterfaceApplications.Web.Models;
using Microsoft.AspNetCore.Components;

namespace ContextInterfaceApplications.Web.Components.Shared;

public partial class WorkspaceSwitchAction
{
    private static readonly string[] ReviewSteps = ["new-item", "in-review", "needs-followup", "approved"];
    private static readonly string[] TriageSteps = ["queued-item", "deferred-item", "routed-item"];

    public static IReadOnlyList<AuthoredActionContract> GetAuthoredActionsForStep(string stepId)
    {
        if (ReviewSteps.Contains(stepId, StringComparer.Ordinal))
        {
            return
            [
                new AuthoredActionContract(
                    "switch-to-triage-workspace",
                    stepId,
                    "Leave the review workspace and open the triage workspace instead.",
                    "The visible item, tools, actions, and results are replaced by the triage surface.",
                    nameof(WorkspaceSwitchAction))
            ];
        }

        if (TriageSteps.Contains(stepId, StringComparer.Ordinal))
        {
            return
            [
                new AuthoredActionContract(
                    "switch-to-review-workspace",
                    stepId,
                    "Leave the triage workspace and open the review workspace instead.",
                    "The visible item, tools, actions, and results are replaced by the review surface.",
                    nameof(WorkspaceSwitchAction))
            ];
        }

        return [];
    }

    [Parameter, EditorRequired]
    public required AuthoredActionContract Action { get; set; }

    [Parameter]
    public ProjectionTarget Target { get; set; }
}
