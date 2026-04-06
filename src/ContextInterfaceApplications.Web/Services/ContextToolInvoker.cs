using ContextInterfaceApplications.Web.Models;

namespace ContextInterfaceApplications.Web.Services;

public sealed class ContextToolInvoker : IContextToolInvoker
{
    private readonly IReadOnlyDictionary<string, IContextToolHandler> _handlers;

    public ContextToolInvoker(IEnumerable<IContextToolHandler> handlers)
    {
        _handlers = handlers.ToDictionary(handler => handler.ToolId, StringComparer.Ordinal);
    }

    public Task<ToolInvocationResult> InvokeAsync(ContextInterfaceState state, ToolInvocationRequest request, CancellationToken cancellationToken = default)
    {
        if (!_handlers.TryGetValue(request.ToolId, out var handler))
        {
            return Task.FromResult(new ToolInvocationResult(
                false,
                request.ToolId,
                $"Tool '{request.ToolId}' is not registered in the runtime tool boundary.",
                null));
        }

        return handler.InvokeAsync(state, request, cancellationToken);
    }
}
