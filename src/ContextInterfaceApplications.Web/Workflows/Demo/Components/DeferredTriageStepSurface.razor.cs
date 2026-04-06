using ContextInterfaceApplications.Web.Models;

namespace ContextInterfaceApplications.Web.Workflows.Demo.Components;

public partial class DeferredTriageStepSurface
{
    internal static IReadOnlyList<SurfaceSectionDefinition> GetSections() =>
    [
        new SurfaceSectionDefinition(
            "deferred-triage-guidance",
            "Deferred Triage Guidance",
            "This workspace keeps the queued item visible while triage is deferred to a later pass.",
            SurfaceSectionVisibility.Shared,
            "volatile",
            nameof(DeferredTriageStepSurface))
    ];
}
