using ContextInterfaceApplications.Runtime;
using ContextInterfaceApplications.Web.Models;

namespace ContextInterfaceApplications.Web.Services;

public interface IAgentInterfaceSnapshotBuilder
{
    InterfaceSnapshot Build(ContextInterfaceState state, RuntimeSubstrateDescriptor substrate);
}

public sealed class AgentInterfaceSnapshotBuilder : IAgentInterfaceSnapshotBuilder
{
    private readonly ICurrentInterfaceProjectionResolver _projectionResolver;

    public AgentInterfaceSnapshotBuilder(
        ICurrentInterfaceProjectionResolver projectionResolver)
    {
        _projectionResolver = projectionResolver;
    }

    public InterfaceSnapshot Build(ContextInterfaceState state, RuntimeSubstrateDescriptor substrate)
    {
        var projection = _projectionResolver.Resolve(state, ProjectionTarget.Agent);
        var sectionNodes = projection.Sections
            .Select(BuildSectionNode)
            .ToArray();
        var toolNodes = projection.Tools
            .Select(BuildToolNode)
            .ToArray();
        var actionNodes = projection.Actions
            .Select(BuildActionNode)
            .ToArray();

        var workflowNode = new InterfaceNode(
            "workflow",
            state.CurrentStep.Id,
            state.WorkflowName,
            null,
            [],
            [
                new InterfaceNode(
                    "step",
                    state.CurrentStep.Id,
                    state.CurrentStep.Title,
                    state.CurrentStep.Decision,
                    [
                        new InterfaceNodeProperty("next-valid-action", state.CurrentStep.NextValidAction)
                    ],
                    []),
                new InterfaceNode(
                    "current-item",
                    state.CurrentItem.Id,
                    state.CurrentItem.Title,
                    state.CurrentItem.Summary,
                    [
                        new InterfaceNodeProperty("status", state.CurrentItem.Status),
                        new InterfaceNodeProperty("detail", state.CurrentItem.Detail)
                    ],
                    []),
                new InterfaceNode(
                    "step-sections",
                    "step-sections",
                    "Step Sections",
                    null,
                    [],
                    sectionNodes),
                new InterfaceNode(
                    "visible-tools",
                    "visible-tools",
                    "Visible Tools",
                    null,
                    [],
                    toolNodes),
                new InterfaceNode(
                    "available-actions",
                    "available-actions",
                    "Available Actions",
                    null,
                    [],
                    actionNodes),
                new InterfaceNode(
                    "projected-results",
                    "projected-results",
                    "Projected Results",
                    null,
                    [],
                    state.RecentResults.Select(BuildResultNode).ToArray())
            ]);

        var runtimeNode = new InterfaceNode(
            "runtime-substrate",
            "runtime-substrate",
            substrate.Name,
            substrate.Role,
            [
                new InterfaceNodeProperty("package-id", substrate.PackageId),
                new InterfaceNodeProperty("version", substrate.Version),
                new InterfaceNodeProperty("constraint", substrate.Constraint)
            ],
            []);

        var root = new InterfaceNode(
            "context-interface-application",
            state.ApplicationName,
            state.ApplicationName,
            null,
            [
                new InterfaceNodeProperty("consumer", ProjectionTarget.Agent.ToString().ToLowerInvariant()),
                new InterfaceNodeProperty("updated-at-utc", state.UpdatedAtUtc.ToString("O"))
            ],
            [workflowNode, runtimeNode]);

        return new InterfaceSnapshot(
            Guid.NewGuid().ToString("n"),
            ProjectionTarget.Agent,
            state.ApplicationName,
            state.WorkflowName,
            root,
            DateTimeOffset.UtcNow);
    }

    private static InterfaceNode BuildToolNode(AuthoredToolContract tool) =>
        new(
            "tool",
            tool.Id,
            tool.Label,
            tool.Description,
            [
                new InterfaceNodeProperty("scope", tool.Scope),
                new InterfaceNodeProperty("source-component", tool.SourceComponent)
            ],
            []);

    private static InterfaceNode BuildActionNode(AuthoredActionContract action) =>
        new(
            "action",
            action.ActionId,
            action.ActionId,
            action.Description,
            [
                new InterfaceNodeProperty("step-id", action.StepId),
                new InterfaceNodeProperty("result", action.Result),
                new InterfaceNodeProperty("source-component", action.SourceComponent)
            ],
            []);

    private static InterfaceNode BuildResultNode(ProjectedResult result) =>
        new(
            "projected-result",
            result.Label,
            result.Label,
            result.Summary,
            [
                new InterfaceNodeProperty("timestamp-utc", result.TimestampUtc.ToString("O"))
            ],
            []);

    private static InterfaceNode BuildSectionNode(SurfaceSectionDefinition section) =>
        new(
            "interface-section",
            section.Id,
            section.Title,
            section.Summary,
            [
                new InterfaceNodeProperty("visibility", section.Visibility.ToString().ToLowerInvariant()),
                new InterfaceNodeProperty("volatility", section.Volatility),
                new InterfaceNodeProperty("source-component", section.SourceComponent)
            ],
            []);
}
