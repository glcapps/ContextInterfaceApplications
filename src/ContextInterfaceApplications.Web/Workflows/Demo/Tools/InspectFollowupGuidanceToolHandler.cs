using ContextInterfaceApplications.Web.Models;
using ContextInterfaceApplications.Web.Services;

namespace ContextInterfaceApplications.Web.Workflows.Demo.Tools;

public sealed class InspectFollowupGuidanceToolHandler : IContextToolHandler
{
    public string ToolId => "inspect-followup-guidance";

    public Task<ToolInvocationResult> InvokeAsync(ContextInterfaceState state, ToolInvocationRequest request, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new ToolInvocationResult(
            true,
            ToolId,
            "Follow-up guidance projected into interface state.",
            new ProjectedResult(
                "Follow-up guidance",
                "Resume review once the item has enough additional context to support an approval decision.",
                DateTimeOffset.UtcNow)));
    }
}
