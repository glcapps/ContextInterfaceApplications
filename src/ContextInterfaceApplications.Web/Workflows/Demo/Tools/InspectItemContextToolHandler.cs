using ContextInterfaceApplications.Web.Models;
using ContextInterfaceApplications.Web.Services;

namespace ContextInterfaceApplications.Web.Workflows.Demo.Tools;

public sealed class InspectItemContextToolHandler : IContextToolHandler
{
    public string ToolId => "inspect-item-context";

    public Task<ToolInvocationResult> InvokeAsync(ContextInterfaceState state, ToolInvocationRequest request, CancellationToken cancellationToken = default)
    {
        var item = state.CurrentItem;
        return Task.FromResult(new ToolInvocationResult(
            true,
            ToolId,
            "Item context projected into interface state.",
            new ProjectedResult(
                "Item context",
                $"{item.Title} ({item.Id}) is in status {item.Status}: {item.Summary}",
                DateTimeOffset.UtcNow)));
    }
}
