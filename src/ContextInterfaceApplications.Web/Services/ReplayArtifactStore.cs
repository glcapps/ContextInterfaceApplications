using ContextInterfaceApplications.Web.Models;

namespace ContextInterfaceApplications.Web.Services;

public interface IReplayArtifactStore
{
    void Record(ReplayArtifact artifact);
    void RecordTransition(TransitionArtifact artifact);
    ReplayArtifact? GetLatest();
    TransitionArtifact? GetLatestTransition();
}

public sealed class InMemoryReplayArtifactStore : IReplayArtifactStore
{
    private readonly Lock _gate = new();
    private ReplayArtifact? _latest;
    private TransitionArtifact? _latestTransition;

    public void Record(ReplayArtifact artifact)
    {
        lock (_gate)
        {
            _latest = artifact;
        }
    }

    public ReplayArtifact? GetLatest()
    {
        lock (_gate)
        {
            return _latest;
        }
    }

    public void RecordTransition(TransitionArtifact artifact)
    {
        lock (_gate)
        {
            _latestTransition = artifact;
        }
    }

    public TransitionArtifact? GetLatestTransition()
    {
        lock (_gate)
        {
            return _latestTransition;
        }
    }
}
