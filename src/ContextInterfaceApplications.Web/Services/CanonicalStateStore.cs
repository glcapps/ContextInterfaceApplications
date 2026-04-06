using ContextInterfaceApplications.Web.Models;

namespace ContextInterfaceApplications.Web.Services;

public interface ICanonicalStateStore
{
    ContextInterfaceState GetState();
    ContextInterfaceState AdvanceWorkflow();
    AgentActionResult ApplyAgentAction(AgentActionRequest request);
    ContextInterfaceState ApplyProjectedResult(ProjectedResult result);
}

public sealed class WorkflowStateStore : ICanonicalStateStore
{
    private readonly Lock _gate = new();
    private readonly IWorkflowDefinition _workflowDefinition;

    private ContextInterfaceState _state;

    public WorkflowStateStore(IWorkflowDefinition workflowDefinition)
    {
        _workflowDefinition = workflowDefinition;
        _state = workflowDefinition.CreateInitialState();
    }

    public ContextInterfaceState GetState()
    {
        lock (_gate)
        {
            return _state;
        }
    }

    public ContextInterfaceState AdvanceWorkflow()
    {
        lock (_gate)
        {
            _state = _workflowDefinition.GetNextState(_state.CurrentStep.Id);
            return _state;
        }
    }

    public AgentActionResult ApplyAgentAction(AgentActionRequest request)
    {
        lock (_gate)
        {
            var result = _workflowDefinition.ApplyAction(_state, request);
            if (result.Accepted)
            {
                _state = result.CurrentState;
            }

            return result;
        }
    }

    public ContextInterfaceState ApplyProjectedResult(ProjectedResult result)
    {
        lock (_gate)
        {
            var recentResults = new[] { result }
                .Concat(_state.RecentResults.Take(4))
                .ToArray();

            _state = _state with
            {
                RecentResults = recentResults,
                UpdatedAtUtc = DateTimeOffset.UtcNow
            };

            return _state;
        }
    }
}
