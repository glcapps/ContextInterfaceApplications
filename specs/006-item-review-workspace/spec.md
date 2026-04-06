# Feature Specification: Item Review Workspace

**Feature Branch**: `006-item-review-workspace`  
**Created**: 2026-04-06  
**Status**: Draft  
**Input**: User description: "Add a generic but application-shaped item review workspace that proves the starter can project a more realistic shared work surface to both human and agent consumers."

## Intent

Replace the synthetic proof-of-concept step flow with a small, generic `Item Review Workspace` that still fits many application domains. The goal is to prove the starter can project a realistic shared work surface, with one current item, visible status, step-local actions, and projected results, to both human and agent consumers without becoming domain-specific product code.

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Review A Generic Work Item From Shared State (Priority: P1)

As a starter maintainer, I need the demo workflow to look like a generic item-review screen rather than an architecture-only demo so the PoC better proves the README claim with a more believable application slice.

**Why this priority**: The current harness is strong enough internally. The next proof step is to show it on a more application-shaped surface instead of a synthetic bootstrap flow.

**Independent Test**: The human and agent surfaces both show the same current review item, status, step-local actions, and projected results from shared state.

**Acceptance Scenarios**:

1. **Given** the workflow is on the initial review step, **When** the human or agent surface is rendered, **Then** both surfaces expose the same current item and the visible actions appropriate for that review state.
2. **Given** the current item advances through review, follow-up, and approval states, **When** either surface is rendered again, **Then** the visible affordances change with the item status.

---

### User Story 2 - Support A Small But Realistic Review Flow (Priority: P2)

As a starter maintainer, I need the example workflow to support more than one valid review outcome so the demo demonstrates actual workflow branching rather than only linear progression.

**Why this priority**: A review workspace becomes believable when an item can be approved or sent for follow-up instead of only advanced linearly.

**Independent Test**: The workflow supports at least these paths:
- new item -> start review -> in review
- in review -> approve item -> approved
- in review -> request follow-up -> needs follow-up
- needs follow-up -> resume review -> in review

**Acceptance Scenarios**:

1. **Given** an item is in review, **When** `approve-item` is chosen, **Then** the workflow moves to an approved state.
2. **Given** an item is in review, **When** `request-followup` is chosen, **Then** the workflow moves to a follow-up state.

---

### User Story 3 - Keep The Example Generic And Reusable (Priority: P3)

As a starter maintainer, I need this review workspace to remain generic enough that it still reads as starter example code rather than product-specific domain code.

**Why this priority**: The project should prove the pattern with something more real than a toy workflow, without turning the starter into a product implementation.

**Independent Test**: The item model, actions, and tools remain generic, and the starter seams still look reusable by another workflow.

**Acceptance Scenarios**:

1. **Given** another maintainer reads the example, **When** they inspect the current item model and review actions, **Then** they can see a generic review slice rather than a product-specific domain.

### Architecture Impact

- **Visible Interface Changes**: The demo surfaces now show a current review item and status-driven review affordances.
- **Shared Authorship Changes**: Existing authored section and authored affordance seams are exercised on a more realistic workflow slice.
- **Runtime Boundary Changes**: Runtime state gains a current item record while keeping authored metadata ownership in the existing authored seams.
- **Replay/Inspection Changes**: Replay and projection inspection now operate on a more application-shaped visible surface.
- **Stable vs Volatile Regions**: The current item identity is stable within a review session, while status, visible actions, and visible tools remain step-scoped volatile regions.

### Edge Cases

- How should a follow-up state expose enough context without becoming domain-specific?
- Which item fields are generic enough to be useful without implying a product domain?
- How should approved-state actions behave so the example remains inspectable and replayable?

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: The example workflow MUST expose a current review item from shared canonical state.
- **FR-002**: The example workflow MUST support at least one branching decision beyond simple linear progression.
- **FR-003**: The human-facing and agent-facing surfaces MUST continue to derive from the same shared state and authored seams.
- **FR-004**: The example item model, actions, and tools MUST remain generic enough to read as a starter example rather than a product domain.
- **FR-005**: Existing projection, snapshot, and replay behavior MUST continue to function on the new workflow slice.

### Key Entities *(include if feature involves data)*

- **Review Item**: The current generic item being reviewed.
- **Review Status**: The current workflow state of that item, such as `new`, `in_review`, `needs_followup`, or `approved`.
- **Review Affordance**: A visible action or tool appropriate for the current review state.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: The starter demonstrates a believable generic review workspace instead of a synthetic architecture-only step flow.
- **SC-002**: The workflow includes at least one meaningful branch and still renders coherently for both human and agent consumers.
- **SC-003**: Tests prove the shared-state, projection, and replay seams continue to work on the new example.

## Assumptions

- The item review slice is an example workflow, not a new product direction for the repository.
- The first pass may still use a single in-memory example item rather than a list or full queue.
- Existing infrastructure seams such as runtime adapters, projection resolution, and replay storage remain in place.
