using ContextInterfaceApplications.Web.Models;
using ContextInterfaceApplications.Web.Services;

namespace ContextInterfaceApplications.Web.Workflows.Demo.Tools;

public sealed class InspectInboxContextToolHandler : IContextToolHandler
{
    public string ToolId => "inspect-inbox-context";

    public Task<ToolInvocationResult> InvokeAsync(ContextInterfaceState state, ToolInvocationRequest request, CancellationToken cancellationToken = default)
    {
        var item = state.CurrentItem;
        return Task.FromResult(new ToolInvocationResult(
            true,
            ToolId,
            "Inbox context projected into interface state.",
            new ProjectedResult(
                "Inbox context",
                $"{item.Title} ({item.Id}) is currently {item.Status} in {state.WorkflowName}.",
                DateTimeOffset.UtcNow)));
    }
}
