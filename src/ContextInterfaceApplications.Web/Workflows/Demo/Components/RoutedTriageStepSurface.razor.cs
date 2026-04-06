using ContextInterfaceApplications.Web.Models;

namespace ContextInterfaceApplications.Web.Workflows.Demo.Components;

public partial class RoutedTriageStepSurface
{
    internal static IReadOnlyList<SurfaceSectionDefinition> GetSections() =>
    [
        new SurfaceSectionDefinition(
            "routing-outcome-summary",
            "Routing Outcome Summary",
            "This workspace shows a completed triage routing decision while leaving the item reopenable for queue review.",
            SurfaceSectionVisibility.Shared,
            "volatile",
            nameof(RoutedTriageStepSurface))
    ];
}
