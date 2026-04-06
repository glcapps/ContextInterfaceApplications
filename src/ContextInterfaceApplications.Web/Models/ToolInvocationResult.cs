namespace ContextInterfaceApplications.Web.Models;

public sealed record ToolInvocationResult(
    bool Succeeded,
    string ToolId,
    string Message,
    ProjectedResult? ProjectedResult);
