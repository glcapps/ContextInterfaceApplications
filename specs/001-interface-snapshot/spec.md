# Feature Specification: Interface Snapshot Scaffold

**Feature Branch**: `001-interface-snapshot`  
**Created**: 2026-04-05  
**Status**: Draft  
**Input**: User description: "Add starter-grade interface snapshot scaffolding so the repository can inspect authored visible structure directly, not only rendered strings and transition artifacts."

## Intent

Add a starter-grade interface snapshot seam that makes the currently visible agent-facing surface inspectable as structured data. This directly reinforces the README principles that the visible interface is the payload, authored interface structure is the source of truth, and replay/inspection belong in the harness rather than in prompt logs.

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Inspect Current Agent Surface As Structured Data (Priority: P1)

As a starter maintainer, I need a structured interface snapshot for the current visible agent-facing surface so I can inspect what the system believes is visible without relying only on serialized XHTML.

**Why this priority**: This is the smallest meaningful step toward the README goal of treating the context interface like an inspectable DOM/devtools surface while keeping the authored UI/component layer central.

**Independent Test**: A current step can be requested through an endpoint or service and returns a structured node tree that includes workflow position, authored step sections, visible tools, actions, and projected results.

**Acceptance Scenarios**:

1. **Given** the workflow is on a step, **When** an interface snapshot is requested, **Then** the response includes that step, its authored step sections, its visible tools, its visible actions, and its projected results as structured nodes.
2. **Given** the workflow changes steps, **When** a snapshot is requested again, **Then** the node tree reflects the new visible interface rather than stale state.

---

### User Story 2 - Relate Replay Artifacts To Structured Interface Nodes (Priority: P2)

As a starter maintainer, I need transition replay to reference structured interface snapshots so replay compares interface structure as well as rendered payloads.

**Why this priority**: The README emphasizes replay, diff, and state correlation. String artifacts alone do not fully express that direction.

**Independent Test**: A recorded transition can expose before/after snapshots or snapshot identifiers alongside the rendered before/after surfaces.

**Acceptance Scenarios**:

1. **Given** an accepted action records a transition, **When** the latest transition is inspected, **Then** the replay artifact includes structured before/after interface data in addition to rendered surfaces.

---

### User Story 3 - Keep Snapshot Scaffolding Generic For Starter Reuse (Priority: P3)

As a starter maintainer, I need the snapshot system to be reusable across workflows so it reinforces starter seams rather than hardcoding the demo workflow.

**Why this priority**: This repository is evolving toward a reusable starter. Snapshot scaffolding should be generic from the start.

**Independent Test**: Snapshot construction uses starter abstractions and authored section metadata resolvers rather than requiring direct knowledge of demo-specific step markup in shared pages.

**Acceptance Scenarios**:

1. **Given** a workflow definition is swapped, **When** the snapshot seam is used, **Then** it still operates through workflow/resolver abstractions rather than demo-only code paths.

### Architecture Impact

- **Visible Interface Changes**: The starter gains a structured representation of the visible agent-facing surface in addition to serialized XHTML.
- **Shared Authorship Changes**: Snapshot construction must derive from the same workflow state, authored contracts, and projection seams that already drive agent rendering, including authored step-section metadata owned by Razor components.
- **Runtime Boundary Changes**: Runtime substrates remain unchanged except where their metadata must be exposed as clearly separated interface nodes.
- **Replay/Inspection Changes**: Transition artifacts and debug inspection should be able to expose structured before/after snapshots alongside rendered payloads.
- **Stable vs Volatile Regions**: The snapshot model should make it possible to distinguish stable metadata from step-local volatile interface regions.

### Edge Cases

- What happens when a step has no available agent actions but still has visible tools?
- How should stable runtime metadata be represented so it does not blur with step-local visible interface nodes?
- How should human-only or agent-only authored additions appear in snapshots without implying both consumers see the same nodes?
- How can authored step-section metadata remain familiar to web developers instead of becoming a bespoke context DSL?

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: The starter MUST define a structured interface snapshot model for visible interface nodes.
- **FR-002**: The snapshot model MUST distinguish among workflow state, authored step sections, visible tools, visible actions, projected results, and runtime metadata.
- **FR-003**: The starter MUST be able to construct a snapshot for the current visible agent-facing surface without requiring callers to parse rendered XHTML.
- **FR-004**: Snapshot construction MUST remain aligned with starter seams and MUST NOT introduce a hidden context object that diverges from rendered output.
- **FR-005**: Replay transition artifacts MUST be able to reference or include structured before/after interface snapshots.
- **FR-006**: Snapshot scaffolding MUST remain reusable across workflow definitions rather than being bound to the current demo workflow.
- **FR-007**: At least one layer of snapshot-visible structure MUST be derived from Razor-authored component metadata so the snapshot seam remains downstream of familiar web authoring patterns.

### Key Entities *(include if feature involves data)*

- **Interface Snapshot**: A structured representation of the currently visible interface surface for one consumer at one point in time.
- **Interface Node**: A typed node within the snapshot representing a visible piece of authored or projected interface structure.
- **Projection Record**: Metadata describing which consumer the snapshot targets and how it relates to rendered output.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: A maintainer can retrieve a structured snapshot for the current agent-facing surface without parsing serialized markup.
- **SC-002**: Transition replay exposes both rendered payloads and structured before/after interface representations.
- **SC-003**: The snapshot seam can be exercised in tests through starter abstractions without introducing new demo-specific branches in shared page composition.
- **SC-004**: Authored step sections can be projected into both agent markup and structured snapshots from Razor-owned metadata rather than a separate bespoke authoring model.

## Assumptions

- The first snapshot pass may coexist with existing rendered payload capture rather than replace it immediately.
- The current demo workflow remains the example implementation used to validate starter seams.
- The first snapshot model can focus on the agent-facing surface before later expanding to dual human/agent snapshot comparison.
- The first pass can start with step-section metadata as the authored layer brought closest to Razor, leaving tools and actions to follow in later work.
