using ContextInterfaceApplications.Web.Components.Shared;

namespace ContextInterfaceApplications.Web.Services;

public sealed class ActionComponentResolver : IActionComponentResolver
{
    private static readonly IReadOnlyDictionary<string, Type> ComponentMap = new Dictionary<string, Type>(StringComparer.Ordinal)
    {
        [nameof(FoundationalDemoAction)] = typeof(FoundationalDemoAction),
        [nameof(ReplayCaptureAction)] = typeof(ReplayCaptureAction)
    };

    public Type Resolve(string sourceComponent) =>
        ComponentMap.TryGetValue(sourceComponent, out var componentType)
            ? componentType
            : typeof(FoundationalDemoAction);
}
