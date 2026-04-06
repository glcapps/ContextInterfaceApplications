using ContextInterfaceApplications.Web.Models;

namespace ContextInterfaceApplications.Web.Workflows.Demo.Components;

public partial class ReplayCaptureStepSurface
{
    internal static IReadOnlyList<SurfaceSectionDefinition> GetSections() =>
    [
        new SurfaceSectionDefinition(
            "followup-review-guidance",
            "Follow-Up Review Guidance",
            "This step keeps the item visible while waiting for follow-up before review resumes.",
            SurfaceSectionVisibility.Shared,
            "volatile",
            nameof(ReplayCaptureStepSurface))
    ];
}
