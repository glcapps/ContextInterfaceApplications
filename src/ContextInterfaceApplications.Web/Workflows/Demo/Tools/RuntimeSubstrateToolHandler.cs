using ContextInterfaceApplications.Runtime;
using ContextInterfaceApplications.Web.Models;
using ContextInterfaceApplications.Web.Services;

namespace ContextInterfaceApplications.Web.Workflows.Demo.Tools;

public sealed class RuntimeSubstrateToolHandler : IContextToolHandler
{
    private readonly IAgentRuntimeSubstrate _runtimeSubstrate;

    public RuntimeSubstrateToolHandler(IAgentRuntimeSubstrate runtimeSubstrate)
    {
        _runtimeSubstrate = runtimeSubstrate;
    }

    public string ToolId => "runtime-substrate";

    public Task<ToolInvocationResult> InvokeAsync(ContextInterfaceState state, ToolInvocationRequest request, CancellationToken cancellationToken = default)
    {
        var runtime = _runtimeSubstrate.Describe();
        return Task.FromResult(new ToolInvocationResult(
            true,
            ToolId,
            "Runtime substrate description projected into interface state.",
            new ProjectedResult(
                "Runtime substrate",
                $"{runtime.Name} via {runtime.PackageId} acts only as substrate: {runtime.Constraint}",
                DateTimeOffset.UtcNow)));
    }
}
