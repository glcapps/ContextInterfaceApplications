using ContextInterfaceApplications.Web.Components;
using ContextInterfaceApplications.Web.Models;
using ContextInterfaceApplications.Runtime;

namespace ContextInterfaceApplications.Web.Services;

public interface IHumanSurfaceRenderer
{
    Task<string> RenderAsync(ContextInterfaceState state);
}

public interface IAgentSurfaceRenderer
{
    Task<ReplayArtifact> RenderAsync(ContextInterfaceState state, RuntimeSubstrateDescriptor substrate);
}

public sealed class HumanSurfaceRenderer : IHumanSurfaceRenderer
{
    private readonly IBlazorComponentRenderer _renderer;

    public HumanSurfaceRenderer(IBlazorComponentRenderer renderer)
    {
        _renderer = renderer;
    }

    public Task<string> RenderAsync(ContextInterfaceState state) =>
        _renderer.RenderAsync<HumanSurfacePage>(new Dictionary<string, object?>
        {
            ["State"] = state
        });
}

public sealed class AgentSurfaceRenderer : IAgentSurfaceRenderer
{
    private readonly IBlazorComponentRenderer _renderer;

    public AgentSurfaceRenderer(IBlazorComponentRenderer renderer)
    {
        _renderer = renderer;
    }

    public async Task<ReplayArtifact> RenderAsync(ContextInterfaceState state, RuntimeSubstrateDescriptor substrate)
    {
        var content = await _renderer.RenderAsync<AgentSurfacePage>(new Dictionary<string, object?>
        {
            ["State"] = state,
            ["RuntimeSubstrate"] = substrate
        });

        return new ReplayArtifact(
            Guid.NewGuid().ToString("n"),
            content,
            DateTimeOffset.UtcNow);
    }
}
