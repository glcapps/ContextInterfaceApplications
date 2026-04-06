using ContextInterfaceApplications.Web.Models;
using ContextInterfaceApplications.Web.Services;

namespace ContextInterfaceApplications.Web.Workflows.Demo.Tools;

public sealed class InspectReviewHistoryToolHandler : IContextToolHandler
{
    public string ToolId => "inspect-review-history";

    public Task<ToolInvocationResult> InvokeAsync(ContextInterfaceState state, ToolInvocationRequest request, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new ToolInvocationResult(
            true,
            ToolId,
            "Review history projected into interface state.",
            new ProjectedResult(
                "Review history",
                $"Current step '{state.CurrentStep.Id}' has {state.RecentResults.Count} projected result entries available for review context.",
                DateTimeOffset.UtcNow)));
    }
}
