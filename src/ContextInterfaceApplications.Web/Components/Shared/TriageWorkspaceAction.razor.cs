using ContextInterfaceApplications.Web.Models;
using Microsoft.AspNetCore.Components;

namespace ContextInterfaceApplications.Web.Components.Shared;

public partial class TriageWorkspaceAction
{
    public static IReadOnlyList<AuthoredToolContract> GetAuthoredToolsForStep(string stepId) =>
        stepId switch
        {
            "queued-item" =>
            [
                new AuthoredToolContract(
                    "inspect-inbox-context",
                    "Inspect Inbox Context",
                    "application-surface",
                    "Review the queue-specific summary and current intake detail for this item.",
                    nameof(TriageWorkspaceAction)),
                new AuthoredToolContract(
                    "inspect-routing-policy",
                    "Inspect Routing Policy",
                    "application-surface",
                    "Review the generic policy used to route or defer queued items.",
                    nameof(TriageWorkspaceAction))
            ],
            "deferred-item" =>
            [
                new AuthoredToolContract(
                    "inspect-inbox-context",
                    "Inspect Inbox Context",
                    "application-surface",
                    "Review the queued item again before resuming triage.",
                    nameof(TriageWorkspaceAction)),
                new AuthoredToolContract(
                    "inspect-routing-policy",
                    "Inspect Routing Policy",
                    "application-surface",
                    "Review when a deferred item should return to active triage.",
                    nameof(TriageWorkspaceAction))
            ],
            "routed-item" =>
            [
                new AuthoredToolContract(
                    "inspect-routing-policy",
                    "Inspect Routing Policy",
                    "application-surface",
                    "Review the routing decision that moved this item onward from triage.",
                    nameof(TriageWorkspaceAction))
            ],
            _ => []
        };

    public static IReadOnlyList<AuthoredActionContract> GetAuthoredActionsForStep(string stepId) =>
        stepId switch
        {
            "queued-item" =>
            [
                new AuthoredActionContract(
                    "assign-review",
                    "queued-item",
                    "Route this queued item into active review.",
                    "The triage workspace records the routing outcome for this item.",
                    nameof(TriageWorkspaceAction)),
                new AuthoredActionContract(
                    "defer-item",
                    "queued-item",
                    "Defer the item until a later triage pass.",
                    "The triage workspace moves the item into a deferred state.",
                    nameof(TriageWorkspaceAction))
            ],
            "deferred-item" =>
            [
                new AuthoredActionContract(
                    "resume-triage",
                    "deferred-item",
                    "Bring the deferred item back into active triage.",
                    "The triage workspace returns the item to the queued state.",
                    nameof(TriageWorkspaceAction))
            ],
            "routed-item" =>
            [
                new AuthoredActionContract(
                    "return-to-queue",
                    "routed-item",
                    "Return the item to the queue for another triage pass.",
                    "The triage workspace moves the item back into the queued state.",
                    nameof(TriageWorkspaceAction))
            ],
            _ => []
        };

    [Parameter, EditorRequired]
    public required AuthoredActionContract Action { get; set; }

    [Parameter]
    public ProjectionTarget Target { get; set; }
}
