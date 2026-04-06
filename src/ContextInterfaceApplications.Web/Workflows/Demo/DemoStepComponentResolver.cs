using ContextInterfaceApplications.Web.Models;
using ContextInterfaceApplications.Web.Services;
using ContextInterfaceApplications.Web.Workflows.Demo.Components;

namespace ContextInterfaceApplications.Web.Workflows.Demo;

public sealed class DemoStepComponentResolver : IStepComponentResolver
{
    public Type? Resolve(string stepId, ProjectionTarget target) =>
        stepId switch
        {
            "new-item" => typeof(IntentAnchoringStepSurface),
            "in-review" => typeof(SharedStateProjectionStepSurface),
            "needs-followup" => typeof(ReplayCaptureStepSurface),
            "approved" => typeof(ApprovedStepSurface),
            "queued-item" => typeof(QueuedTriageStepSurface),
            "deferred-item" => typeof(DeferredTriageStepSurface),
            "routed-item" => typeof(RoutedTriageStepSurface),
            _ => null
        };
}
