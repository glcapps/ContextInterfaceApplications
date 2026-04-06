namespace ContextInterfaceApplications.Web.Models;

public sealed record TransitionArtifact(
    string Id,
    string ActionId,
    string FromStepId,
    string ToStepId,
    ReplayArtifact BeforeSurface,
    ReplayArtifact AfterSurface,
    InterfaceSnapshot BeforeSnapshot,
    InterfaceSnapshot AfterSnapshot,
    DateTimeOffset CreatedAtUtc);
