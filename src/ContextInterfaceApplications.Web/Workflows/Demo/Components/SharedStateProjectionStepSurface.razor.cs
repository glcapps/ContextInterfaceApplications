using ContextInterfaceApplications.Web.Models;

namespace ContextInterfaceApplications.Web.Workflows.Demo.Components;

public partial class SharedStateProjectionStepSurface
{
    internal static IReadOnlyList<SurfaceSectionDefinition> GetSections() =>
    [
        new SurfaceSectionDefinition(
            "projection-inspection",
            "Projection Inspection",
            "This step emphasizes that the human and agent surfaces derive from the same canonical state.",
            SurfaceSectionVisibility.Shared,
            "volatile",
            nameof(SharedStateProjectionStepSurface))
    ];
}
