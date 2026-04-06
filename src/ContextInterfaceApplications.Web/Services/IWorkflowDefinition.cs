using ContextInterfaceApplications.Web.Models;

namespace ContextInterfaceApplications.Web.Services;

public interface IWorkflowDefinition
{
    string WorkflowId { get; }
    ContextInterfaceState CreateInitialState();
    ContextInterfaceState GetNextState(string currentStepId);
    AgentActionResult ApplyAction(ContextInterfaceState currentState, AgentActionRequest request);
}
