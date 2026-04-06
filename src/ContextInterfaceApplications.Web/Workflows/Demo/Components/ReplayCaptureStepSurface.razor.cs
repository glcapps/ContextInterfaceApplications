using ContextInterfaceApplications.Web.Models;

namespace ContextInterfaceApplications.Web.Workflows.Demo.Components;

public partial class ReplayCaptureStepSurface
{
    internal static IReadOnlyList<SurfaceSectionDefinition> GetSections() =>
    [
        new SurfaceSectionDefinition(
            "replay-inspection",
            "Replay Inspection",
            "This step is where transition artifacts become the main debugging surface.",
            SurfaceSectionVisibility.Shared,
            "volatile",
            nameof(ReplayCaptureStepSurface))
    ];
}
