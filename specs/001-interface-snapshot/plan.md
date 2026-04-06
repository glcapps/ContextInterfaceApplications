# Implementation Plan: Interface Snapshot Scaffold

**Branch**: `001-interface-snapshot` | **Date**: 2026-04-06 | **Spec**: [/Users/garycapps/Documents/GitHub/ContextInterfaceApplications/specs/001-interface-snapshot/spec.md](/Users/garycapps/Documents/GitHub/ContextInterfaceApplications/specs/001-interface-snapshot/spec.md)
**Input**: Feature specification from `/specs/001-interface-snapshot/spec.md`

## Summary

Add a starter-grade interface snapshot seam that produces a structured representation of the visible agent-facing surface, and carry that structured representation through replay and inspection. This reinforces the README principles that context should be inspectable like a DOM, that authored interface structure remains the source of truth, that replay/debugging belong in the harness, and that the authoring experience must remain legible to ordinary web developers.

## Technical Context

**Language/Version**: C# / .NET 10  
**Primary Dependencies**: ASP.NET Core, Blazor components, xUnit, `WebApplicationFactory`  
**Storage**: In-memory replay and transition artifacts extended to include structured snapshots  
**Testing**: `dotnet test tests/ContextInterfaceApplications.Tests/ContextInterfaceApplications.Tests.csproj -m:1 -nr:false`  
**Target Platform**: ASP.NET Core / Kestrel starter template  
**Project Type**: Starter template with shared human and agent-facing render paths  
**Constraints**: Preserve authored interface structure as source of truth; do not introduce a hidden context builder; keep runtime substrate data clearly separated from step-local visible nodes; keep snapshot seams reusable across workflows; keep snapshot-visible authored structure downstream of Razor component authoring rather than a bespoke context DSL  
**Scale/Scope**: Starter architecture evolution centered on agent-facing interface inspection

## Constitution Check

*GATE: Must pass before implementation starts. Re-check after design.*

- Does the change preserve authored interface structure as the source of truth?
  - It must derive snapshots from the same state, contracts, and projection seams that already drive rendering, with at least one layer coming directly from Razor-authored metadata.
- Does it avoid introducing a hidden context object or incidental prompt builder?
  - It must. The snapshot is an inspectable structural representation of the visible surface, not a parallel prompt-only model.
- Does it keep runtime substrates behind narrow infrastructure seams?
  - Yes. Runtime metadata may appear in snapshots only as clearly typed nodes sourced from the existing abstraction.
- Does it preserve or improve replay/inspection for affected workflow behavior?
  - Yes. Replay artifacts must gain structured before/after snapshots.
- Does it add reusable starter seams instead of demo-specific branching?
  - Yes. Snapshot building must work through workflow and projection seams, not demo-only hardcoding.

## Design Notes

### Affected Layers

- **Canonical State**: Existing `ContextInterfaceState` remains the state source; snapshot building reads from it rather than replacing it.
- **Authored Interface Layer**: Snapshot construction must reflect authored actions, visible tools, step surfaces, and projected results already exposed by the starter, and now also read authored step-section metadata from Razor-owned definitions.
- **Projection Layer**: The agent-facing render path gains a sibling structured projection path; human-facing snapshot support can remain out of scope for the first pass.
- **Replay/Inspection Layer**: `TransitionArtifact` and replay endpoints expand to include structured before/after interface snapshots.
- **Runtime/Infrastructure Layer**: `IAgentRuntimeSubstrate` metadata may be projected into snapshot nodes without expanding the runtime abstraction.

### Invariants

- The agent-facing rendered payload remains authoritative and still exists alongside snapshots.
- Snapshot seams are reusable across workflow definitions.
- Shared pages and renderers do not gain new demo-specific branching.
- Snapshot-visible authored structure should be introduced through familiar component metadata and wrapper components, not a separate developer-authored tree DSL.
- Replay/debug output stays able to show what the agent saw.

### Verification Strategy

- Add service-level tests for snapshot construction against current workflow state and authored step-section metadata.
- Add integration tests for a new snapshot endpoint and replay transition payloads.
- Verify accepted actions update both rendered surfaces and structured snapshots.
- Verify runtime metadata, authored step sections, visible tools, actions, and projected results are represented distinctly.

## Project Structure

### Documentation (this feature)

```text
specs/001-interface-snapshot/
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
└── Program.cs

tests/ContextInterfaceApplications.Tests/
├── ApplicationEndpointsTests.cs
├── RenderingTests.cs
└── TestServiceFactory.cs
```

**Structure Decision**: Add a reusable snapshot model and service in the web starter layer, extend replay artifacts to carry snapshots, expose the current snapshot through inspection-friendly endpoints, and keep authored structure closest to Razor by introducing step-section metadata through normal component patterns rather than a second hidden authoring architecture.

## Complexity Tracking

| Complexity | Why Needed | Simpler Alternative Rejected Because |
|------------|------------|-------------------------------------|
| Structured snapshot model alongside rendered XHTML | The README requires inspectable interface structure, not only string artifacts | Parsing rendered XHTML would make inspection incidental and brittle |
| Replay artifacts carrying both rendered and structured output | Replay must support structural inspection as well as payload review | Recording only strings loses the node-level structure the feature is meant to add |
| Razor-authored section metadata plus resolver seam | Keeps the feature closer to normal web development while giving snapshots a direct authored input | Building snapshots only from flat state would drift away from the README goal that authored UI structure remains primary |
