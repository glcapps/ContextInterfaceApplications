namespace ContextInterfaceApplications.Web.Models;

public sealed record ReplayArtifact(
    string Id,
    string Content,
    DateTimeOffset CreatedAtUtc);
