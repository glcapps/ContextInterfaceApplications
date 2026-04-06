namespace ContextInterfaceApplications.Runtime;

// Runtime substrates are infrastructure only. They must not define workflow,
// interface structure, or application semantics.
public interface IAgentRuntimeSubstrate
{
    RuntimeSubstrateDescriptor Describe();
}
