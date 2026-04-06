using ContextInterfaceApplications.Web.Models;
using ContextInterfaceApplications.Web.Services;

namespace ContextInterfaceApplications.Web.Workflows.Demo.Tools;

public sealed class InspectRoutingPolicyToolHandler : IContextToolHandler
{
    public string ToolId => "inspect-routing-policy";

    public Task<ToolInvocationResult> InvokeAsync(ContextInterfaceState state, ToolInvocationRequest request, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new ToolInvocationResult(
            true,
            ToolId,
            "Routing policy projected into interface state.",
            new ProjectedResult(
                "Routing policy",
                "Route the item onward when queue context is sufficient; defer it when the next destination is still ambiguous.",
                DateTimeOffset.UtcNow)));
    }
}
