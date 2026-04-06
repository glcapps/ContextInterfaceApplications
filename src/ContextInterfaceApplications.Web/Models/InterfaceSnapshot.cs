namespace ContextInterfaceApplications.Web.Models;

public sealed record InterfaceSnapshot(
    string Id,
    ProjectionTarget Target,
    string ApplicationName,
    string WorkflowName,
    InterfaceNode Root,
    DateTimeOffset CreatedAtUtc);

public sealed record InterfaceNode(
    string NodeType,
    string? Id,
    string? Label,
    string? Value,
    IReadOnlyList<InterfaceNodeProperty> Properties,
    IReadOnlyList<InterfaceNode> Children);

public sealed record InterfaceNodeProperty(
    string Name,
    string Value);
