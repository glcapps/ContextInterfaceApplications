# Feature Specification: Authored Affordance Contracts

**Feature Branch**: `002-authored-affordance-contracts`  
**Created**: 2026-04-06  
**Status**: Draft  
**Input**: User description: "Make visible tools and actions feel more directly authored from the component layer while keeping execution and validation in the existing DI/runtime seams."

## Intent

Add a starter-grade seam for authored affordance contracts so visible tools and actions are declared closer to the Razor/component layer, then projected consistently into human-facing affordances, agent-facing markup, and structured snapshots. This reinforces the README principles that the visible interface is the payload, authored interface structure is the source of truth, and the project should remain legible to ordinary web developers rather than drifting into a bespoke agent-only authoring model.

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Author Visible Affordances From Razor-Friendly Metadata (Priority: P1)

As a starter maintainer, I need visible tools and actions to be declared from Razor-friendly authored metadata so interface affordances feel like part of normal component authoring instead of separate descriptors attached later in the state layer.

**Why this priority**: This is the next meaningful step after authored step sections. It moves the remaining visible affordances closer to the component layer while preserving the existing runtime and workflow seams.

**Independent Test**: A current step can expose its visible tool and action contracts through starter abstractions backed by component-owned metadata, and those contracts appear consistently in the human surface, the agent surface, and the agent snapshot.

**Acceptance Scenarios**:

1. **Given** a workflow step authored through Razor-friendly affordance metadata, **When** the human and agent surfaces are rendered, **Then** the same authored tool and action contracts appear in both projections with consumer-appropriate output.
2. **Given** the current agent-facing snapshot is requested, **When** authored affordance metadata is present, **Then** the snapshot includes affordance nodes derived from that authored metadata rather than only flat state descriptors.

---

### User Story 2 - Keep Runtime Execution And Validation Behind Existing Seams (Priority: P2)

As a starter maintainer, I need authored affordance contracts to remain separate from execution and workflow mutation so the project does not collapse UI authorship into runtime ownership.

**Why this priority**: The README and PoC planning both insist that runtime frameworks and execution layers must not own interface semantics. Moving affordance declarations closer to Razor should not weaken that boundary.

**Independent Test**: Tool invocations still execute through `IContextToolInvoker` and `IContextToolHandler`, and action application still validates through the workflow/state seams even after authored affordance metadata becomes the visible contract source.

**Acceptance Scenarios**:

1. **Given** an authored tool contract is visible, **When** it is invoked, **Then** execution still flows through the DI-backed tool invoker and handlers.
2. **Given** an authored action contract is visible, **When** it is posted back, **Then** workflow validation still rejects stale or invalid actions through the current action/state boundary.

---

### User Story 3 - Keep The Starter Web-Native And Reusable (Priority: P3)

As a starter maintainer, I need the authored affordance seam to stay generic across workflows and feel familiar to web developers so the starter remains reusable and does not drift into a custom context DSL.

**Why this priority**: The project is explicitly trying to stay close to frontend development mental models. Affordance authorship should look like normal component metadata and wrapper composition, not a parallel interface language.

**Independent Test**: The seam is implemented through starter abstractions and shared components or metadata models that other workflows could adopt without depending on the current demo workflow classes.

**Acceptance Scenarios**:

1. **Given** a different workflow definition is registered, **When** it adopts the authored affordance seam, **Then** the starter can project those authored contracts without demo-specific branching in shared pages.

### Architecture Impact

- **Visible Interface Changes**: Visible tools and actions become more directly attributable to Razor-authored affordance metadata in both render targets and snapshots.
- **Shared Authorship Changes**: Tool and action contract authorship moves closer to shared components or step surfaces instead of remaining primarily store-derived descriptors.
- **Runtime Boundary Changes**: Runtime execution, handler dispatch, and workflow validation remain in the current DI and workflow seams and do not move into the authored component layer.
- **Replay/Inspection Changes**: Snapshots and replay artifacts should show authored affordance provenance more clearly as those contracts move closer to component authorship.
- **Stable vs Volatile Regions**: Affordance contracts remain step-scoped volatile interface regions, while the starter abstractions that project them should remain stable.

### Edge Cases

- How should a shared authored affordance appear differently across human and agent projections without duplicating its contract definition?
- How should hidden or invalid affordances be represented when a workflow step changes and previously visible authored contracts are no longer valid?
- How can affordance metadata stay web-familiar without turning into a bespoke authored context schema?

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: The starter MUST define a reusable seam for authored tool and action contracts that is closer to the Razor/component layer than the current store-derived descriptors.
- **FR-002**: The same authored affordance metadata MUST be projectable into human-facing affordances, agent-facing markup, and structured agent snapshots.
- **FR-003**: Tool execution MUST remain behind `IContextToolInvoker` and `IContextToolHandler` or an equivalent starter-owned runtime seam.
- **FR-004**: Action validation and state mutation MUST remain behind the workflow and canonical state seams rather than moving into component authorship.
- **FR-005**: The seam MUST remain reusable across workflow definitions and MUST NOT require demo-specific branching in shared page composition.
- **FR-006**: The authored affordance model MUST remain legible to ordinary web developers and MUST NOT require developers to author a second bespoke context DSL.

### Key Entities *(include if feature involves data)*

- **Authored Affordance Contract**: A Razor-friendly representation of a visible tool or action contract that can be projected to multiple consumers.
- **Projection Affordance Node**: The rendered or snapshot-visible representation of an authored tool or action for a specific consumer.
- **Execution Boundary**: The existing DI-backed or workflow-backed runtime path that executes a tool call or validates and applies an action.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: At least one layer of visible tools and actions can be authored through Razor-friendly metadata and projected consistently into both render targets and snapshots.
- **SC-002**: Tool execution and action validation continue to flow through the existing runtime and workflow seams.
- **SC-003**: Tests prove the affordance seam works through starter abstractions without introducing a new bespoke authored interface language.

## Assumptions

- The first pass may apply the authored affordance seam to the current demo workflow before generalizing it further.
- Existing `VisibleTool` and `AgentActionDescriptor` models may coexist temporarily while authored affordance metadata becomes more authoritative.
- Step-section metadata introduced in `001-interface-snapshot` is the immediate precedent for keeping authored interface structure close to Razor.
