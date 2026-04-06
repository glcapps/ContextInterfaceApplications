# Feature Specification: Derived Runtime State

**Feature Branch**: `003-derived-runtime-state`  
**Created**: 2026-04-06  
**Status**: Draft  
**Input**: User description: "Make canonical runtime state stop acting like a second authored interface definition layer by carrying stable affordance ids while visible tool and action contracts remain derived from authored seams."

## Intent

Refine canonical runtime state so it carries workflow facts, recent projected results, and stable visible affordance ids for validation, while authored interface contracts remain owned by the Razor-friendly affordance and section seams. This reinforces the README principles that authored interface structure is the source of truth, the visible interface is the payload, and the architecture should still feel like ordinary web development rather than a duplicated hidden interface model.

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Keep Runtime State Focused On Runtime Facts (Priority: P1)

As a starter maintainer, I need canonical state to represent workflow/runtime facts rather than duplicate full visible tool and action descriptors so the state layer is easier to reason about and no longer acts like a second interface-definition source.

**Why this priority**: After authored step sections and authored affordance contracts, the remaining architectural ambiguity is that canonical state still carries full visible affordance descriptors. Tightening that split makes the authored UI layer more clearly primary.

**Independent Test**: The runtime state endpoint returns the current step, recent projected results, and stable visible affordance ids, while the human surface, agent surface, and agent snapshot still render full affordance contracts through the authored resolver seams.

**Acceptance Scenarios**:

1. **Given** the workflow is on a step, **When** `/api/state` is requested, **Then** the response exposes current visible tool ids and action ids rather than full authored descriptor payloads.
2. **Given** the human or agent surface is rendered, **When** authored affordance contracts are needed, **Then** they are still projected correctly through the authored resolver seam rather than reconstructed from runtime state descriptors.

---

### User Story 2 - Preserve Validation And Execution Boundaries (Priority: P2)

As a starter maintainer, I need action validation and tool visibility checks to continue working after the runtime state is reduced to ids so the architecture remains correct without reintroducing descriptor duplication.

**Why this priority**: Runtime state still needs enough information to reject invalid or stale calls. The cleanup is only useful if the validation boundary remains practical.

**Independent Test**: Action posts and tool calls still succeed or fail exactly as before, using current step id plus current visible affordance ids as the validation basis.

**Acceptance Scenarios**:

1. **Given** a visible tool id is present in runtime state, **When** the corresponding tool is invoked, **Then** the current tool visibility check still allows the call.
2. **Given** an action id is absent from runtime state for the current step, **When** that action is posted, **Then** the workflow still rejects it.

---

### User Story 3 - Keep The Project Web-Native And Coherent (Priority: P3)

As a starter maintainer, I need the split between authored interface contracts and runtime state to remain understandable to web developers so the project reads like a normal application with derived projections rather than a custom context system.

**Why this priority**: The project is explicitly trying to stay close to familiar frontend/server architecture. Runtime state should look like normal app state; authored contracts should look like normal component-owned metadata.

**Independent Test**: The codebase shows a clear distinction between authored projection seams and runtime state/validation seams without introducing a new bespoke authored context representation.

**Acceptance Scenarios**:

1. **Given** a maintainer reads the state model and the authored affordance resolver, **When** they compare responsibilities, **Then** the authored layer clearly owns full visible contracts while runtime state clearly owns only ids and workflow facts.

### Architecture Impact

- **Visible Interface Changes**: None in end-user output; the visible surfaces should remain the same.
- **Shared Authorship Changes**: Clarifies that authored sections and authored affordance contracts own full interface metadata.
- **Runtime Boundary Changes**: Canonical state becomes a cleaner runtime/validation handoff by carrying ids instead of full visible descriptor objects.
- **Replay/Inspection Changes**: Snapshots remain full-fidelity because they already derive visible contracts from authored seams instead of runtime-state descriptors.
- **Stable vs Volatile Regions**: Visible affordance ids remain step-scoped volatile runtime facts; authored metadata remains in the more stable component/resolver layer.

### Edge Cases

- How should runtime state represent a step with no visible tools or no available actions?
- How should tool/action validation behave if authored resolver output and runtime ids drift out of sync?
- Which remaining runtime-state fields are truly canonical facts versus still-derived convenience data?

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: Canonical runtime state MUST carry stable visible tool ids and action ids instead of full visible tool/action descriptor payloads.
- **FR-002**: Human-facing rendering, agent-facing rendering, and agent snapshots MUST continue to derive full visible affordance contracts from authored resolver seams rather than runtime state descriptors.
- **FR-003**: Tool visibility checks MUST continue to validate against the current runtime state.
- **FR-004**: Action validation and workflow mutation MUST continue to validate against the current runtime state and workflow seams.
- **FR-005**: The resulting split between runtime state and authored interface contracts MUST remain legible to ordinary web developers.

### Key Entities *(include if feature involves data)*

- **Runtime State**: The canonical workflow/runtime facts exposed by `ContextInterfaceState`.
- **Visible Affordance Id**: A stable tool id or action id that remains in runtime state for validation.
- **Authored Contract Source**: The authored affordance or section seam that owns full visible metadata for rendering and snapshots.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: `ContextInterfaceState` no longer acts like a second full source of visible affordance descriptor metadata.
- **SC-002**: Existing action and tool validation behavior still passes integration tests.
- **SC-003**: Visible surfaces and snapshots remain unchanged in behavior while their data sourcing becomes more coherent.

## Assumptions

- The current demo workflow remains the immediate implementation target.
- Existing authored affordance resolvers remain the full metadata source for visible tool/action projection.
- This pass may leave other runtime convenience fields in place if they still represent legitimate canonical facts.
