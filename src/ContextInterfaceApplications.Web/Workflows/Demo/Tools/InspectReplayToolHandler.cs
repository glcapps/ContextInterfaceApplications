using ContextInterfaceApplications.Web.Models;
using ContextInterfaceApplications.Web.Services;

namespace ContextInterfaceApplications.Web.Workflows.Demo.Tools;

public sealed class InspectReplayToolHandler : IContextToolHandler
{
    private readonly IReplayArtifactStore _replayArtifactStore;

    public InspectReplayToolHandler(IReplayArtifactStore replayArtifactStore)
    {
        _replayArtifactStore = replayArtifactStore;
    }

    public string ToolId => "inspect-replay";

    public Task<ToolInvocationResult> InvokeAsync(ContextInterfaceState state, ToolInvocationRequest request, CancellationToken cancellationToken = default)
    {
        var latestTransition = _replayArtifactStore.GetLatestTransition();
        var summary = latestTransition is null
            ? "No transition artifact is recorded yet."
            : $"Latest transition: {latestTransition.FromStepId} -> {latestTransition.ToStepId} via {latestTransition.ActionId}.";

        return Task.FromResult(new ToolInvocationResult(
            true,
            ToolId,
            "Replay inspection projected into interface state.",
            new ProjectedResult(
                "Replay inspection",
                summary,
                DateTimeOffset.UtcNow)));
    }
}
