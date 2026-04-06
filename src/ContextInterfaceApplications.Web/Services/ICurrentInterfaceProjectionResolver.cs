using ContextInterfaceApplications.Web.Models;

namespace ContextInterfaceApplications.Web.Services;

public interface ICurrentInterfaceProjectionResolver
{
    CurrentInterfaceProjection Resolve(ContextInterfaceState state, ProjectionTarget target);
}

public sealed class CurrentInterfaceProjectionResolver : ICurrentInterfaceProjectionResolver
{
    private readonly IStepSurfaceMetadataResolver _stepSurfaceMetadataResolver;
    private readonly IAuthoredAffordanceResolver _authoredAffordanceResolver;

    public CurrentInterfaceProjectionResolver(
        IStepSurfaceMetadataResolver stepSurfaceMetadataResolver,
        IAuthoredAffordanceResolver authoredAffordanceResolver)
    {
        _stepSurfaceMetadataResolver = stepSurfaceMetadataResolver;
        _authoredAffordanceResolver = authoredAffordanceResolver;
    }

    public CurrentInterfaceProjection Resolve(ContextInterfaceState state, ProjectionTarget target)
    {
        var sections = _stepSurfaceMetadataResolver.GetSections(state.CurrentStep.Id, target);
        var tools = _authoredAffordanceResolver
            .GetTools(state.CurrentStep.Id)
            .Where(tool => state.CurrentVisibleToolIds.Contains(tool.Id, StringComparer.Ordinal))
            .ToArray();
        var actions = _authoredAffordanceResolver
            .GetActions(state.CurrentStep.Id)
            .Where(action => state.CurrentAvailableActionIds.Contains(action.ActionId, StringComparer.Ordinal))
            .ToArray();

        return new CurrentInterfaceProjection(target, sections, tools, actions);
    }
}
