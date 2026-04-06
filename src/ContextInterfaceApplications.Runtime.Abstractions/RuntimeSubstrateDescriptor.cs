namespace ContextInterfaceApplications.Runtime;

public sealed record RuntimeSubstrateDescriptor(
    string Name,
    string PackageId,
    string Version,
    string Role,
    string Constraint);
