using ContextInterfaceApplications.Web.Services;
using System.Net;
using ContextInterfaceApplications.Web.Models;
using ContextInterfaceApplications.Web.Workflows.Demo;
using ContextInterfaceApplications.Web.Workflows.Demo.Tools;
using ContextInterfaceApplications.Runtime;
using ContextInterfaceApplications.Runtime.Maf;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents();
builder.Services.AddSingleton<IWorkflowDefinition, DemoWorkflowDefinition>();
builder.Services.AddSingleton<IAuthoredAffordanceResolver, DemoAuthoredAffordanceResolver>();
builder.Services.AddSingleton<IStepComponentResolver, DemoStepComponentResolver>();
builder.Services.AddSingleton<IStepSurfaceMetadataResolver, DemoStepSurfaceMetadataResolver>();
builder.Services.AddSingleton<ICanonicalStateStore, WorkflowStateStore>();
builder.Services.AddSingleton<IActionComponentResolver, ActionComponentResolver>();
builder.Services.AddSingleton<IContextToolInvoker, ContextToolInvoker>();
builder.Services.AddSingleton<IContextToolHandler, RuntimeSubstrateToolHandler>();
builder.Services.AddSingleton<IContextToolHandler, InspectDualProjectionToolHandler>();
builder.Services.AddSingleton<IContextToolHandler, InspectReplayToolHandler>();
builder.Services.AddSingleton<IBlazorComponentRenderer, BlazorComponentRenderer>();
builder.Services.AddSingleton<IHumanSurfaceRenderer, HumanSurfaceRenderer>();
builder.Services.AddSingleton<IAgentSurfaceRenderer, AgentSurfaceRenderer>();
builder.Services.AddSingleton<IAgentInterfaceSnapshotBuilder, AgentInterfaceSnapshotBuilder>();
builder.Services.AddSingleton<IReplayArtifactStore, InMemoryReplayArtifactStore>();
builder.Services.AddMafRuntimeSubstrate();

var app = builder.Build();

app.MapGet("/", async (
    ICanonicalStateStore stateStore,
    IHumanSurfaceRenderer renderer) =>
{
    var state = stateStore.GetState();
    var html = await renderer.RenderAsync(state);

    return Results.Content(html, "text/html; charset=utf-8");
});

app.MapGet("/agent/surface", async (
    ICanonicalStateStore stateStore,
    IAgentSurfaceRenderer renderer,
    IReplayArtifactStore replayStore,
    IAgentRuntimeSubstrate runtimeSubstrate) =>
{
    var state = stateStore.GetState();
    var surface = await renderer.RenderAsync(state, runtimeSubstrate.Describe());
    replayStore.Record(surface);

    return Results.Content(surface.Content, "application/xhtml+xml; charset=utf-8");
});

app.MapGet("/debug/agent/surface", async (
    ICanonicalStateStore stateStore,
    IAgentSurfaceRenderer renderer,
    IAgentRuntimeSubstrate runtimeSubstrate,
    bool escaped = true) =>
{
    var state = stateStore.GetState();
    var surface = await renderer.RenderAsync(state, runtimeSubstrate.Describe());
    var debugContent = escaped
        ? $"<pre>{WebUtility.HtmlEncode(surface.Content)}</pre>"
        : surface.Content;
    var modeLabel = escaped ? "escaped source" : "raw markup";

    var html = $$"""
    <!DOCTYPE html>
    <html lang="en">
    <head>
      <meta charset="utf-8" />
      <meta name="viewport" content="width=device-width, initial-scale=1" />
      <title>Agent Surface Debug</title>
      <style>
        :root { color-scheme: light; --bg: #f5f1e8; --panel: #fffdf8; --line: #d8d1c7; --ink: #1d1c19; --muted: #6b665f; }
        body { margin: 0; background: var(--bg); color: var(--ink); font-family: Georgia, 'Iowan Old Style', serif; }
        main { max-width: 1100px; margin: 0 auto; padding: 32px 20px 48px; }
        section { background: var(--panel); border: 1px solid var(--line); border-radius: 16px; padding: 18px; box-shadow: 0 10px 30px rgba(29, 28, 25, 0.05); }
        h1 { margin-top: 0; }
        p { color: var(--muted); }
        pre { margin: 0; overflow-x: auto; white-space: pre-wrap; word-break: break-word; font: 0.95rem/1.5 ui-monospace, SFMono-Regular, monospace; }
      </style>
    </head>
    <body>
      <main>
        <h1>Agent Surface Debug</h1>
        <p>This view shows the current agent-facing payload in {{modeLabel}} mode.</p>
        <section>
          {{debugContent}}
        </section>
      </main>
    </body>
    </html>
    """;

    return Results.Content(html, "text/html; charset=utf-8");
});

app.MapGet("/api/state", (ICanonicalStateStore stateStore) => Results.Ok(stateStore.GetState()));

app.MapGet("/api/runtime", (IAgentRuntimeSubstrate runtimeSubstrate) => Results.Ok(runtimeSubstrate.Describe()));

app.MapGet("/api/agent/snapshot", (
    ICanonicalStateStore stateStore,
    IAgentRuntimeSubstrate runtimeSubstrate,
    IAgentInterfaceSnapshotBuilder snapshotBuilder) =>
{
    var state = stateStore.GetState();
    var snapshot = snapshotBuilder.Build(state, runtimeSubstrate.Describe());
    return Results.Ok(snapshot);
});

app.MapGet("/api/replay/latest", (IReplayArtifactStore replayStore) =>
{
    var artifact = replayStore.GetLatest();
    return artifact is null ? Results.NotFound() : Results.Ok(artifact);
});

app.MapGet("/api/replay/transitions/latest", (IReplayArtifactStore replayStore) =>
{
    var artifact = replayStore.GetLatestTransition();
    return artifact is null ? Results.NotFound() : Results.Ok(artifact);
});

app.MapPost("/api/workflow/advance", (ICanonicalStateStore stateStore) =>
{
    var updated = stateStore.AdvanceWorkflow();
    return Results.Ok(updated);
});

app.MapPost("/api/agent/actions", async (
    AgentActionRequest request,
    ICanonicalStateStore stateStore,
    IAgentSurfaceRenderer renderer,
    IAgentRuntimeSubstrate runtimeSubstrate,
    IAgentInterfaceSnapshotBuilder snapshotBuilder,
    IReplayArtifactStore replayStore) =>
{
    var substrate = runtimeSubstrate.Describe();
    var priorState = stateStore.GetState();
    var beforeSurface = await renderer.RenderAsync(priorState, substrate);
    var beforeSnapshot = snapshotBuilder.Build(priorState, substrate);
    var result = stateStore.ApplyAgentAction(request);

    if (!result.Accepted)
    {
        return Results.Conflict(result);
    }

    var afterSurface = await renderer.RenderAsync(result.CurrentState, substrate);
    var afterSnapshot = snapshotBuilder.Build(result.CurrentState, substrate);
    replayStore.Record(afterSurface);
    replayStore.RecordTransition(new TransitionArtifact(
        Guid.NewGuid().ToString("n"),
        request.ActionId,
        priorState.CurrentStep.Id,
        result.CurrentState.CurrentStep.Id,
        beforeSurface,
        afterSurface,
        beforeSnapshot,
        afterSnapshot,
        DateTimeOffset.UtcNow));

    return Results.Ok(result);
});

app.MapPost("/api/tools/call", async (
    ToolInvocationRequest request,
    ICanonicalStateStore stateStore,
    IContextToolInvoker toolInvoker) =>
{
    var currentState = stateStore.GetState();

    if (!string.Equals(request.StepId, currentState.CurrentStep.Id, StringComparison.Ordinal))
    {
        return Results.Conflict(new ToolInvocationResult(
            false,
            request.ToolId,
            $"Rejected tool call because step '{request.StepId}' is no longer current.",
            null));
    }

    if (!currentState.VisibleTools.Any(tool => string.Equals(tool.Id, request.ToolId, StringComparison.Ordinal)))
    {
        return Results.Conflict(new ToolInvocationResult(
            false,
            request.ToolId,
            $"Rejected tool call '{request.ToolId}'. It is not visible on the current surface.",
            null));
    }

    var invocation = await toolInvoker.InvokeAsync(currentState, request);
    if (!invocation.Succeeded || invocation.ProjectedResult is null)
    {
        return Results.Conflict(invocation);
    }

    stateStore.ApplyProjectedResult(invocation.ProjectedResult);
    return Results.Ok(invocation);
});

app.Run();

namespace ContextInterfaceApplications.Web
{
    public partial class Program
    {
    }
}
