namespace ContextInterfaceApplications.Web.Models;

public sealed record AgentActionResult(
    bool Accepted,
    string Message,
    ContextInterfaceState CurrentState);
