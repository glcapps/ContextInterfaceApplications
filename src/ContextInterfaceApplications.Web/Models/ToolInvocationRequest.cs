namespace ContextInterfaceApplications.Web.Models;

public sealed record ToolInvocationRequest(
    string ToolId,
    string StepId);
