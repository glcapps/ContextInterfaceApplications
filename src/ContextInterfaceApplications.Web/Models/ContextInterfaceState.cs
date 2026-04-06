namespace ContextInterfaceApplications.Web.Models;

public sealed record ContextInterfaceState(
    string ApplicationName,
    string WorkflowName,
    WorkflowStep CurrentStep,
    IReadOnlyList<string> CurrentVisibleToolIds,
    IReadOnlyList<string> CurrentAvailableActionIds,
    IReadOnlyList<ProjectedResult> RecentResults,
    DateTimeOffset UpdatedAtUtc);

public sealed record WorkflowStep(
    string Id,
    string Title,
    string Decision,
    string NextValidAction);

public sealed record ProjectedResult(
    string Label,
    string Summary,
    DateTimeOffset TimestampUtc);
