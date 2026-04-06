# Implementation Plan: Authored Affordance Contracts

**Branch**: `002-authored-affordance-contracts` | **Date**: 2026-04-06 | **Spec**: [/Users/garycapps/Documents/GitHub/ContextInterfaceApplications/specs/002-authored-affordance-contracts/spec.md](/Users/garycapps/Documents/GitHub/ContextInterfaceApplications/specs/002-authored-affordance-contracts/spec.md)
**Input**: Feature specification from `/specs/002-authored-affordance-contracts/spec.md`

## Summary

Move visible tool and action contract authorship closer to Razor/component definitions while preserving the current runtime and workflow execution seams. This continues the web-native direction established by authored step-section metadata and reduces the remaining gap between component-authored interface structure and the visible affordances projected to human UI, agent markup, and structured snapshots.

## Technical Context

**Language/Version**: C# / .NET 10  
**Primary Dependencies**: ASP.NET Core, Blazor components, xUnit, `WebApplicationFactory`  
**Storage**: In-memory canonical state, replay artifacts, and transition artifacts remain sufficient for this pass  
**Testing**: `dotnet test tests/ContextInterfaceApplications.Tests/ContextInterfaceApplications.Tests.csproj -m:1 -nr:false`  
**Target Platform**: ASP.NET Core / Kestrel starter template  
**Project Type**: Starter template with shared human and agent-facing render paths  
**Constraints**: Preserve authored interface structure as source of truth; keep execution behind the current DI/runtime and workflow seams; avoid introducing a bespoke authored context DSL; keep shared page composition generic across workflows  
**Scale/Scope**: Starter architecture evolution focused on authored affordance contracts

## Constitution Check

*GATE: Must pass before implementation starts. Re-check after design.*

- Does the change preserve authored interface structure as the source of truth?
  - It must move visible affordance ownership closer to Razor-authored metadata rather than deeper into state/store descriptors.
- Does it avoid introducing a hidden context object or incidental prompt builder?
  - It must. Affordance projection should still feed the rendered surfaces and snapshots directly.
- Does it keep runtime substrates behind narrow infrastructure seams?
  - Yes. Tool execution remains in `IContextToolInvoker` and `IContextToolHandler`; action application remains in the workflow/state seams.
- Does it preserve or improve replay/inspection for affected workflow behavior?
  - Yes. Snapshots and transition artifacts should continue to reflect visible affordances with clearer authored provenance.
- Does it add reusable starter seams instead of demo-specific branching?
  - Yes. The seam should be expressed as starter-owned affordance metadata and projection helpers that the demo workflow merely adopts.

## Design Notes

### Affected Layers

- **Canonical State**: `ContextInterfaceState` may temporarily coexist with authored affordance metadata, but should no longer be the only source of visible tool/action descriptors.
- **Authored Interface Layer**: Shared components or step surfaces should declare visible tool/action contract metadata in a Razor-friendly form, likely through component-owned definitions or wrapper components similar to `ProjectionSection`.
- **Projection Layer**: `VisibleToolsPanel`, `AgentActionContractPanel`, and related shared components should project authored affordance metadata into human and agent render targets without demo-specific branching in shared pages.
- **Replay/Inspection Layer**: `AgentInterfaceSnapshotBuilder` should consume authored affordance metadata so snapshots and transition artifacts reflect the same contract source.
- **Runtime/Infrastructure Layer**: `IContextToolInvoker`, `IContextToolHandler`, `IWorkflowDefinition`, and `ICanonicalStateStore` continue to own execution and validation semantics rather than affordance authorship.

### Invariants

- Tool execution stays in DI-backed handler seams.
- Action validation and workflow mutation stay in the workflow/state boundary.
- Shared pages remain generic and do not branch on demo affordance types.
- Authored affordance definitions stay web-familiar and do not become a separate developer-authored context language.

### Verification Strategy

- Add rendering or service tests showing authored affordance metadata projects into both human and agent outputs.
- Add snapshot tests proving the same authored affordance metadata appears in the structured snapshot.
- Add integration tests proving tool calls and action posts still flow through the existing runtime and workflow seams.
- Verify visible affordance provenance becomes clearer without changing runtime ownership.

## Project Structure

### Documentation (this feature)

```text
specs/002-authored-affordance-contracts/
├── plan.md
├── spec.md
└── tasks.md
```

### Source Code (repository root)

```text
src/ContextInterfaceApplications.Web/
├── Models/
├── Services/
├── Components/
└── Workflows/

tests/ContextInterfaceApplications.Tests/
├── ApplicationEndpointsTests.cs
├── RenderingTests.cs
└── TestServiceFactory.cs
```

**Structure Decision**: Introduce starter-owned authored affordance metadata and projection helpers in the web layer, adopt them in the demo workflow, and keep execution/validation in the current DI and workflow seams rather than collapsing those responsibilities into the component layer.

## Complexity Tracking

| Complexity | Why Needed | Simpler Alternative Rejected Because |
|------------|------------|-------------------------------------|
| Authored affordance metadata seam alongside existing descriptors | Keeps visible tools/actions closer to the component layer while preserving compatibility during transition | Replacing all descriptor/state paths immediately would create unnecessary churn and risk breaking existing starter seams |
| Shared projection helpers for authored affordances | Lets one authored contract feed human UI, agent markup, and snapshots consistently | Hand-building three separate projections per affordance would reintroduce duplicated interface logic |
