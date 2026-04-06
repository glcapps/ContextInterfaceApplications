using ContextInterfaceApplications.Web.Models;

namespace ContextInterfaceApplications.Web.Workflows.Demo.Components;

public partial class QueuedTriageStepSurface
{
    internal static IReadOnlyList<SurfaceSectionDefinition> GetSections() =>
    [
        new SurfaceSectionDefinition(
            "triage-queue-context",
            "Triage Queue Context",
            "This workspace presents a queued item and the next routing choices available from triage.",
            SurfaceSectionVisibility.Shared,
            "stable",
            nameof(QueuedTriageStepSurface))
    ];
}
