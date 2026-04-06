using ContextInterfaceApplications.Web.Models;

namespace ContextInterfaceApplications.Web.Services;

public interface IStepSurfaceMetadataResolver
{
    IReadOnlyList<SurfaceSectionDefinition> GetSections(string stepId, ProjectionTarget target);
}
