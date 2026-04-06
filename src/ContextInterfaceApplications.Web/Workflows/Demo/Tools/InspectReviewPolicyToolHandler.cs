using ContextInterfaceApplications.Web.Models;
using ContextInterfaceApplications.Web.Services;

namespace ContextInterfaceApplications.Web.Workflows.Demo.Tools;

public sealed class InspectReviewPolicyToolHandler : IContextToolHandler
{
    public string ToolId => "inspect-review-policy";

    public Task<ToolInvocationResult> InvokeAsync(ContextInterfaceState state, ToolInvocationRequest request, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(new ToolInvocationResult(
            true,
            ToolId,
            "Review policy projected into interface state.",
            new ProjectedResult(
                "Review policy",
                "Approve when the current item is coherent and complete; request follow-up when material gaps remain.",
                DateTimeOffset.UtcNow)));
    }
}
