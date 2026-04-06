namespace ContextInterfaceApplications.Web.Models;

public sealed record AuthoredToolContract(
    string Id,
    string Label,
    string Scope,
    string Description,
    string SourceComponent)
{
    public VisibleTool ToVisibleTool() => new(Id, Label, Scope, Description, SourceComponent);
}

public sealed record AuthoredActionContract(
    string ActionId,
    string StepId,
    string Description,
    string Result,
    string SourceComponent)
{
    public AgentActionDescriptor ToAgentActionDescriptor() => new(ActionId, StepId, Description, Result, SourceComponent);
}
