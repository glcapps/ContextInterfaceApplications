namespace ContextInterfaceApplications.Web.Models;

public sealed record CurrentInterfaceProjection(
    ProjectionTarget Target,
    IReadOnlyList<SurfaceSectionDefinition> Sections,
    IReadOnlyList<AuthoredToolContract> Tools,
    IReadOnlyList<AuthoredActionContract> Actions);
