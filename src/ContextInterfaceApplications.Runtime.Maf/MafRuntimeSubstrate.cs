using ContextInterfaceApplications.Runtime;

namespace ContextInterfaceApplications.Runtime.Maf;

// This adapter intentionally exposes only a narrow substrate descriptor.
// MAF remains an infrastructure concern behind this boundary.
public sealed class MafRuntimeSubstrate : IAgentRuntimeSubstrate
{
    public RuntimeSubstrateDescriptor Describe() =>
        new(
            "Microsoft Agent Framework",
            "Microsoft.Agents.Hosting.AspNetCore",
            "1.3.176",
            "Runtime substrate for model execution and tool invocation.",
            "The framework is present as infrastructure only; application semantics remain in canonical state and authored surfaces.");
}
