# Implementation Plan: Retrospective Foundation Baseline

**Branch**: `000-retrospective-foundation` | **Date**: 2026-04-06 | **Spec**: [/Users/garycapps/Documents/GitHub/ContextInterfaceApplications/specs/000-retrospective-foundation/spec.md](/Users/garycapps/Documents/GitHub/ContextInterfaceApplications/specs/000-retrospective-foundation/spec.md)
**Input**: Feature specification from `/specs/000-retrospective-foundation/spec.md`

## Summary

Document the starter architecture that already exists in the repository so future specs have an explicit baseline. This reinforces the README and constitution principles around authored interface ownership, visible interface as payload, runtime isolation, replayability, and starter seams.

## Technical Context

**Language/Version**: C# / .NET 10, Markdown  
**Primary Dependencies**: ASP.NET Core, Blazor components, xUnit, starter-specific runtime adapters  
**Storage**: In-memory starter artifacts for now; no new storage changes in this retrospective pass  
**Testing**: Documentation review against current code; no new executable tests required  
**Target Platform**: ASP.NET Core / Kestrel starter template  
**Project Type**: Starter template with shared human and agent-facing render paths  
**Constraints**: Preserve authored interface structure as source of truth; keep runtime substrates infrastructural; maintain replay/inspection fidelity; avoid rewriting history as if the repo were greenfield  
**Scale/Scope**: Documentation-only baseline capture for already-landed starter architecture

## Constitution Check

*GATE: Must pass before implementation starts. Re-check after design.*

- Does the change preserve authored interface structure as the source of truth?
  - Yes. It documents existing ownership rather than relocating it.
- Does it avoid introducing a hidden context object or incidental prompt builder?
  - Yes. This is documentation-only.
- Does it keep runtime substrates behind narrow infrastructure seams?
  - Yes. The plan explicitly describes `ContextInterfaceApplications.Runtime.Abstractions` and `ContextInterfaceApplications.Runtime.Maf` as existing boundaries.
- Does it preserve or improve replay/inspection for affected workflow behavior?
  - Yes. It records replay/transition artifacts as part of the current baseline.
- Does it add reusable starter seams instead of demo-specific branching?
  - Yes. The baseline explicitly distinguishes demo implementation from starter architecture.

## Design Notes

### Affected Layers

- **Canonical State**: Document the current workflow-backed state store and demo workflow plug-in seam.
- **Authored Interface Layer**: Document the existing step and action component resolver seams and shared Blazor authorship model.
- **Projection Layer**: Document the current human and agent surface render paths as shared authored structure with consumer-specific projection.
- **Replay/Inspection Layer**: Document replay artifact storage, transition artifacts, and the existing debug/replay endpoints.
- **Runtime/Infrastructure Layer**: Document the runtime abstractions project and the isolated MAF adapter project.

### Invariants

- The baseline describes only major seams already in the repository.
- Demo workflow code remains example implementation, not the architectural source of truth.
- The spec should be reusable as a reference point for future feature deltas.

### Verification Strategy

- Review the spec against the current code layout in `src/` and `tests/`.
- Confirm the documented seams match the current DI registrations in [Program.cs](/Users/garycapps/Documents/GitHub/ContextInterfaceApplications/src/ContextInterfaceApplications.Web/Program.cs).
- Confirm the spec references the current replay, runtime, tool, workflow, and projection seams without contradicting the constitution.

## Project Structure

### Documentation (this feature)

```text
specs/000-retrospective-foundation/
├── plan.md
├── spec.md
└── tasks.md
```

### Source Code (repository root)

```text
src/
├── ContextInterfaceApplications.Runtime.Abstractions/
├── ContextInterfaceApplications.Runtime.Maf/
└── ContextInterfaceApplications.Web/
    ├── Components/
    ├── Models/
    ├── Services/
    └── Workflows/

tests/
└── ContextInterfaceApplications.Tests/

.specify/
docs/
specs/
```

**Structure Decision**: Keep this feature documentation-only. It should describe existing starter seams, not refactor code under the cover of retrospective documentation.

## Complexity Tracking

| Complexity | Why Needed | Simpler Alternative Rejected Because |
|------------|------------|-------------------------------------|
| Retrospective baseline spec | Spec Kit needs a real architectural baseline before future features can describe deltas | Starting with only forward-looking specs would make the current codebase look undocumented and force future specs to restate baseline seams repeatedly |
