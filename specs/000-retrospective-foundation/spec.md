# Feature Specification: Retrospective Foundation Baseline

**Feature Branch**: `000-retrospective-foundation`  
**Created**: 2026-04-06  
**Status**: Draft  
**Input**: User description: "Retroactively document the major starter architecture already implemented so Spec Kit reflects the current project baseline rather than only future work."

## Intent

Capture the current architectural baseline already present in the repository so Spec Kit starts from the real starter state. This reinforces the README and constitution principles around authored interface ownership, visible interface as payload, runtime boundaries, replay/inspection, and reusable starter seams.

## User Scenarios & Testing *(mandatory)*

For this repository, user stories should usually describe starter maintainers, harness authors, or future adopters extending the starter. Keep the focus on reusable seams, inspectable interface structure, projection, replay, and runtime boundaries unless the repository has explicitly chosen a concrete application integration target.

### User Story 1 - Understand The Existing Starter Boundaries (Priority: P1)

As a starter maintainer, I need the current architectural seams documented as a retrospective spec so future work starts from the boundaries that already exist instead of rediscovering them from code alone.

**Why this priority**: Spec Kit is not useful if it begins after the important starter seams are already in place. The current baseline must be explicit before new features are specified against it.

**Independent Test**: A maintainer can read this spec and identify the current reusable seams for workflow definition, authored step/action resolution, runtime substrate isolation, tool invocation, and replay/transition recording.

**Acceptance Scenarios**:

1. **Given** a maintainer is planning new starter work, **When** they review this spec, **Then** they can locate the existing seam ownership and extension points without treating demo code as architecture.
2. **Given** a maintainer is evaluating a proposed change, **When** they compare it against this spec, **Then** they can see whether it respects the current starter boundaries.

---

### User Story 2 - Distinguish Starter Architecture From Demo Implementation (Priority: P2)

As a starter maintainer, I need the already-landed demo workflow described as an example implementation behind reusable seams so future changes keep demo behavior out of the reusable shell.

**Why this priority**: The repository currently uses a demo workflow to exercise the starter. That is useful only if the separation between starter shell and demo implementation remains explicit.

**Independent Test**: The spec identifies the demo workflow and demo components as example implementations plugged into workflow, step-component, action-component, and tool seams.

**Acceptance Scenarios**:

1. **Given** the current codebase, **When** this spec is read, **Then** the demo workflow is described as a registered implementation rather than the architectural source of truth.

---

### User Story 3 - Establish The Spec Baseline For Future Feature Work (Priority: P3)

As a starter maintainer, I need a retrospective baseline spec so future specs such as interface snapshots can describe deltas from the current project state instead of restating the entire starter.

**Why this priority**: Once the baseline is recorded, future specs can stay focused on new seams and behavior changes rather than repeatedly recapturing existing architecture.

**Independent Test**: A future spec can reference this baseline as already-established starter behavior and only describe new architectural movement.

**Acceptance Scenarios**:

1. **Given** a future starter feature is being specified, **When** the author references the baseline, **Then** they can scope the new work as a change against existing starter seams rather than as a greenfield design.

---

### Architecture Impact

- **Visible Interface Changes**: None. This retrospective spec documents current architecture rather than introducing a new visible surface.
- **Shared Authorship Changes**: Documents that human and agent-facing surfaces are currently driven by shared authored structure plus resolver seams.
- **Runtime Boundary Changes**: Documents the existing runtime substrate abstraction and MAF adapter isolation.
- **Replay/Inspection Changes**: Documents the current replay artifact and transition artifact baseline, including debug/replay endpoints already present.
- **Stable vs Volatile Regions**: Establishes the current baseline that step-scoped state, tools, actions, and projected results are volatile interface regions, while runtime substrate description and reusable starter seams are more stable.

### Edge Cases

- How should future changes document architecture that spans both reusable seams and demo examples without letting the demo become the baseline?
- Which existing debug conveniences are architectural enough to merit future specs versus staying as implementation details?
- How should future specs distinguish retrospective documentation of landed seams from forward-looking implementation work?

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: The repository MUST have a retrospective baseline spec that documents the major starter seams already implemented.
- **FR-002**: The baseline spec MUST describe authored interface ownership, workflow definition seams, step/action resolver seams, tool invocation seams, runtime substrate boundaries, and replay/transition artifacts.
- **FR-003**: The baseline spec MUST distinguish reusable starter architecture from the demo workflow and demo-authored components.
- **FR-004**: The baseline spec MUST remain aligned with the README and constitution rather than redefining the architecture in conflicting terms.
- **FR-005**: The baseline spec MUST be usable as the reference point for future feature specs.

### Key Entities *(include if feature involves data)*

- **Workflow Definition Seam**: The replaceable boundary that supplies step state, actions, tools, and transitions.
- **Projection Resolver Seams**: The step and action component registries that connect authored components to human and agent-facing renders.
- **Runtime Substrate Boundary**: The abstraction that keeps MAF and similar frameworks infrastructural rather than architectural.
- **Tool Invocation Seam**: The DI-driven execution path that separates visible tool contracts from runtime execution.
- **Replay/Transition Artifact**: The stored representation of agent-visible surfaces and accepted transitions.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: A maintainer can use this spec to describe the current starter architecture without relying only on code archaeology.
- **SC-002**: Future specs can reference this baseline instead of restating already-landed seams.
- **SC-003**: The documented baseline clearly separates reusable starter structure from the current demo implementation.

## Assumptions

- The baseline should record only major architectural seams already present, not every historical edit.
- The current demo workflow remains the primary example used to exercise the starter seams until a more realistic workflow replaces it.
- Future specs can supersede parts of this baseline as the starter evolves, but should do so explicitly.
