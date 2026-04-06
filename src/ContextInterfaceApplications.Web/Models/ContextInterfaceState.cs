namespace ContextInterfaceApplications.Web.Models;

public sealed record ContextInterfaceState(
    string ApplicationName,
    string WorkflowName,
    WorkflowStep CurrentStep,
    IReadOnlyList<VisibleTool> VisibleTools,
    IReadOnlyList<AgentActionDescriptor> AvailableAgentActions,
    IReadOnlyList<ProjectedResult> RecentResults,
    DateTimeOffset UpdatedAtUtc);

public sealed record WorkflowStep(
    string Id,
    string Title,
    string Decision,
    string NextValidAction);

public sealed record VisibleTool(
    string Id,
    string Label,
    string Scope,
    string Description,
    string SourceComponent);

public sealed record AgentActionDescriptor(
    string ActionId,
    string StepId,
    string Description,
    string Result,
    string SourceComponent);

public sealed record ProjectedResult(
    string Label,
    string Summary,
    DateTimeOffset TimestampUtc);
