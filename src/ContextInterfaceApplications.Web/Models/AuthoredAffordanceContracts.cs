namespace ContextInterfaceApplications.Web.Models;

public sealed record AuthoredToolContract(
    string Id,
    string Label,
    string Scope,
    string Description,
    string SourceComponent);

public sealed record AuthoredActionContract(
    string ActionId,
    string StepId,
    string Description,
    string Result,
    string SourceComponent);
