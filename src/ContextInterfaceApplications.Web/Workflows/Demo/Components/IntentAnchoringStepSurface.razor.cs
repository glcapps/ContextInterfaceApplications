using ContextInterfaceApplications.Web.Models;

namespace ContextInterfaceApplications.Web.Workflows.Demo.Components;

public partial class IntentAnchoringStepSurface
{
    internal static IReadOnlyList<SurfaceSectionDefinition> GetSections() =>
    [
        new SurfaceSectionDefinition(
            "reviewer-notes",
            "Reviewer Notes",
            "This panel gives the human reviewer a quick note about starting the review from a new item state.",
            SurfaceSectionVisibility.HumanOnly,
            "stable",
            nameof(IntentAnchoringStepSurface)),
        new SurfaceSectionDefinition(
            "agent-review-focus",
            "Agent Review Focus",
            "This section gives the agent a review-oriented summary of what matters at the start of item review.",
            SurfaceSectionVisibility.AgentOnly,
            "stable",
            nameof(IntentAnchoringStepSurface)),
        new SurfaceSectionDefinition(
            "shared-review-brief",
            "Shared Review Brief",
            "This shared section keeps both surfaces aligned on the current item and its initial review objective.",
            SurfaceSectionVisibility.Shared,
            "stable",
            nameof(IntentAnchoringStepSurface))
    ];
}
