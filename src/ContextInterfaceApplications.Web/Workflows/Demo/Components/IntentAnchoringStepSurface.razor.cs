using ContextInterfaceApplications.Web.Models;

namespace ContextInterfaceApplications.Web.Workflows.Demo.Components;

public partial class IntentAnchoringStepSurface
{
    internal static IReadOnlyList<SurfaceSectionDefinition> GetSections() =>
    [
        new SurfaceSectionDefinition(
            "human-only-illustration",
            "Human-Only Illustration",
            "This panel exists only in the human-facing surface and demonstrates consumer-specific UI.",
            SurfaceSectionVisibility.HumanOnly,
            "stable",
            nameof(IntentAnchoringStepSurface)),
        new SurfaceSectionDefinition(
            "agent-only-illustration",
            "Agent-Only Illustration",
            "This section exists only in the agent-facing surface and demonstrates consumer-specific interface projection.",
            SurfaceSectionVisibility.AgentOnly,
            "stable",
            nameof(IntentAnchoringStepSurface)),
        new SurfaceSectionDefinition(
            "shared-illustration",
            "Shared Illustration",
            "This authored section is shared by both projections and rendered through the same component metadata.",
            SurfaceSectionVisibility.Shared,
            "stable",
            nameof(IntentAnchoringStepSurface))
    ];
}
