using ContextInterfaceApplications.Web.Models;
using ContextInterfaceApplications.Web.Services;

namespace ContextInterfaceApplications.Web.Workflows.Demo.Tools;

public sealed class InspectDualProjectionToolHandler : IContextToolHandler
{
    public string ToolId => "inspect-dual-projection";

    public Task<ToolInvocationResult> InvokeAsync(ContextInterfaceState state, ToolInvocationRequest request, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new ToolInvocationResult(
            true,
            ToolId,
            "Dual projection inspection projected into interface state.",
            new ProjectedResult(
                "Dual projection inspection",
                $"Step '{state.CurrentStep.Id}' exposes {state.CurrentVisibleToolIds.Count} visible tools and {state.CurrentAvailableActionIds.Count} agent actions from shared authored structure.",
                DateTimeOffset.UtcNow)));
    }
}
