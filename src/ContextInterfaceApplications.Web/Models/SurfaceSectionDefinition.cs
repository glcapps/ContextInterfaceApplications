namespace ContextInterfaceApplications.Web.Models;

public sealed record SurfaceSectionDefinition(
    string Id,
    string Title,
    string Summary,
    SurfaceSectionVisibility Visibility,
    string Volatility,
    string SourceComponent);

public enum SurfaceSectionVisibility
{
    Shared,
    HumanOnly,
    AgentOnly
}
