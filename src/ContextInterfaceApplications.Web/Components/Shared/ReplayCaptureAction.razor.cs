using ContextInterfaceApplications.Web.Models;
using Microsoft.AspNetCore.Components;

namespace ContextInterfaceApplications.Web.Components.Shared;

public partial class ReplayCaptureAction
{
    public static IReadOnlyList<VisibleTool> GetToolsForStep(string stepId) =>
        stepId switch
        {
            "replay-capture" =>
            [
                new VisibleTool(
                    "inspect-replay",
                    "Inspect Replay Artifact",
                    "application-surface",
                    "Retrieve the latest agent-visible interface snapshot.",
                    "ReplayCaptureAction"),
                new VisibleTool(
                    "reset-workflow",
                    "Reset Workflow",
                    "application-surface",
                    "Return to the first step after validating replay capture.",
                    "ReplayCaptureAction")
            ],
            _ => []
        };

    public static IReadOnlyList<AgentActionDescriptor> GetContractsForStep(string stepId) =>
        stepId switch
        {
            "replay-capture" =>
            [
                new AgentActionDescriptor(
                    "reset-workflow",
                    "replay-capture",
                    "Reset the workflow after replay inspection back to the first step.",
                    "The next rendered surfaces return to intent anchoring.",
                    "ReplayCaptureAction")
            ],
            _ => []
        };

    [Parameter, EditorRequired]
    public required AgentActionDescriptor Action { get; set; }

    [Parameter]
    public ProjectionTarget Target { get; set; }
}
