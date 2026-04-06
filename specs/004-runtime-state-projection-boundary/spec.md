# Feature Specification: Runtime State Projection Boundary

**Feature Branch**: `004-runtime-state-projection-boundary`  
**Created**: 2026-04-06  
**Status**: Draft  
**Input**: User description: "Introduce one starter-owned projection boundary that composes the current visible interface from runtime state and authored resolvers so projection logic stops being scattered across pages and snapshot building."

## Intent

Add a starter-owned projection seam that composes the current visible interface from canonical runtime state plus authored section and affordance resolvers. This keeps canonical state focused on runtime facts, keeps authored metadata in the Razor-friendly layer, and reduces duplication in the rendering and snapshot paths.

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Resolve A Coherent Current Visible Projection (Priority: P1)

As a starter maintainer, I need one projection seam that can resolve the current visible sections, tools, and actions for a given consumer from runtime state plus authored metadata so pages and snapshots do not each reassemble that view independently.

**Why this priority**: After `003-derived-runtime-state`, the remaining cleanup is projection scattering. A single seam makes the architecture easier to explain and extend.

**Independent Test**: A projection resolver can be called with current state and a target and returns the expected sections, tools, and actions for that target.

**Acceptance Scenarios**:

1. **Given** the workflow is on a step, **When** the projection seam resolves the current agent projection, **Then** it returns the step sections, tools, and actions visible for that step.
2. **Given** the workflow changes steps, **When** the projection seam resolves the projection again, **Then** it returns the updated visible sections, tools, and actions for the new step.

---

### User Story 2 - Use The Projection Seam Across Render And Snapshot Paths (Priority: P2)

As a starter maintainer, I need both page rendering and snapshot construction to use the same projection seam so the current visible interface is assembled consistently.

**Why this priority**: The starter already has dual rendering and snapshots. The new seam only matters if it becomes the shared assembly point for those outputs.

**Independent Test**: The human page, agent page, and agent snapshot builder all use the projection seam and continue to render the same visible output as before.

**Acceptance Scenarios**:

1. **Given** the same runtime state, **When** the human page, agent page, and snapshot builder resolve their visible projection, **Then** they all use the same starter-owned projection path rather than separate resolver calls scattered across components.

---

### User Story 3 - Preserve Clear Architectural Boundaries (Priority: P3)

As a starter maintainer, I need the new projection seam to compose existing authored and runtime seams rather than becoming a replacement architecture so the code stays legible to web developers.

**Why this priority**: The value of this feature is coherence, not introducing a new meta-layer. It should remain a mundane application service that composes existing sources.

**Independent Test**: The projection seam depends on canonical runtime state plus authored resolvers and does not take over execution, workflow mutation, or component authorship.

**Acceptance Scenarios**:

1. **Given** the projection seam exists, **When** the codebase is reviewed, **Then** runtime state still owns workflow facts, authored resolvers still own visible metadata, and the projection seam simply composes them for current output.

### Architecture Impact

- **Visible Interface Changes**: None in intended user-visible behavior.
- **Shared Authorship Changes**: No new authored metadata shapes are required; the feature composes the seams already added in earlier steps.
- **Runtime Boundary Changes**: Runtime state remains the validation/runtime handoff and is not expanded again.
- **Replay/Inspection Changes**: Snapshots become more obviously downstream of the same projection seam used by rendering.
- **Stable vs Volatile Regions**: Projection assembly becomes a stable starter boundary over volatile per-step visible content.

### Edge Cases

- How should the projection seam behave when runtime ids and authored resolver output drift apart?
- How should consumer-specific visibility be handled without branching back into shared pages?
- Which runtime facts belong in the projection result versus staying only in canonical state?

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: The starter MUST define a reusable projection seam that resolves the current visible interface from runtime state plus authored resolvers.
- **FR-002**: The human render path, agent render path, and snapshot path MUST be able to consume that projection seam.
- **FR-003**: The seam MUST compose existing authored section and authored affordance resolvers rather than redefining their ownership.
- **FR-004**: Canonical runtime state MUST remain focused on workflow/runtime facts and visible ids.
- **FR-005**: The resulting architecture MUST remain legible to ordinary web developers as a normal application service boundary rather than a bespoke interface language.

### Key Entities *(include if feature involves data)*

- **Current Interface Projection**: The starter-owned composition of sections, tools, and actions visible for a target at the current step.
- **Projection Resolver**: The service that composes runtime state with authored section and affordance resolvers.
- **Projection Consumer**: A render or inspection path that consumes the current interface projection.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: Pages and snapshots no longer each independently resolve authored sections/tools/actions directly.
- **SC-002**: The current visible projection can be tested directly as a service.
- **SC-003**: Visible behavior stays the same while the architecture becomes more coherent.

## Assumptions

- The feature can initially focus on current-step projection only.
- Existing authored section and affordance resolvers remain in place and are composed rather than replaced.
- Validation boundaries in workflow and tool execution remain unchanged.
