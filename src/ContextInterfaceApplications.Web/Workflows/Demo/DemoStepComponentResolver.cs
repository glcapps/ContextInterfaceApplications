using ContextInterfaceApplications.Web.Models;
using ContextInterfaceApplications.Web.Services;
using ContextInterfaceApplications.Web.Workflows.Demo.Components;

namespace ContextInterfaceApplications.Web.Workflows.Demo;

public sealed class DemoStepComponentResolver : IStepComponentResolver
{
    public Type? Resolve(string stepId, ProjectionTarget target) =>
        stepId switch
        {
            "intent-anchoring" => typeof(IntentAnchoringStepSurface),
            "shared-state-projection" => typeof(SharedStateProjectionStepSurface),
            "replay-capture" => typeof(ReplayCaptureStepSurface),
            _ => null
        };
}
