using ContextInterfaceApplications.Web.Models;
using ContextInterfaceApplications.Web.Services;
using ContextInterfaceApplications.Web.Workflows.Demo.Components;

namespace ContextInterfaceApplications.Web.Workflows.Demo;

public sealed class DemoStepSurfaceMetadataResolver : IStepSurfaceMetadataResolver
{
    public IReadOnlyList<SurfaceSectionDefinition> GetSections(string stepId, ProjectionTarget target)
    {
        var sections = stepId switch
        {
            "new-item" => IntentAnchoringStepSurface.GetSections(),
            "in-review" => SharedStateProjectionStepSurface.GetSections(),
            "needs-followup" => ReplayCaptureStepSurface.GetSections(),
            "approved" => ApprovedStepSurface.GetSections(),
            _ => []
        };

        return sections.Where(section => section.Visibility switch
            {
                SurfaceSectionVisibility.Shared => true,
                SurfaceSectionVisibility.HumanOnly => target == ProjectionTarget.Human,
                SurfaceSectionVisibility.AgentOnly => target == ProjectionTarget.Agent,
                _ => false
            })
            .ToArray();
    }
}
