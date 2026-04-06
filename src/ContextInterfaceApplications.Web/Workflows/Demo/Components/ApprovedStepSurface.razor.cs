using ContextInterfaceApplications.Web.Models;

namespace ContextInterfaceApplications.Web.Workflows.Demo.Components;

public partial class ApprovedStepSurface
{
    internal static IReadOnlyList<SurfaceSectionDefinition> GetSections() =>
    [
        new SurfaceSectionDefinition(
            "approval-summary",
            "Approval Summary",
            "This step represents a completed review outcome while keeping the item reopenable for another pass.",
            SurfaceSectionVisibility.Shared,
            "volatile",
            nameof(ApprovedStepSurface))
    ];
}
