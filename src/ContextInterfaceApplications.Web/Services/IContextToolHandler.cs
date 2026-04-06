using ContextInterfaceApplications.Web.Models;

namespace ContextInterfaceApplications.Web.Services;

// Tool handlers execute DI-backed runtime behavior while returning results
// shaped for projection into the visible interface.
public interface IContextToolHandler
{
    string ToolId { get; }
    Task<ToolInvocationResult> InvokeAsync(ContextInterfaceState state, ToolInvocationRequest request, CancellationToken cancellationToken = default);
}
