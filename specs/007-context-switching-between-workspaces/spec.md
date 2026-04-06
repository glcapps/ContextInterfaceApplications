# Feature Specification: Context Switching Between Workspaces

**Feature Branch**: `007-context-switching-between-workspaces`  
**Created**: 2026-04-06  
**Status**: Draft  
**Input**: User description: "Add a second generic workspace and prove that switching replaces the active agent-facing surface instead of accumulating prior state, tools, and actions."

## Intent

Prove that the starter can switch the model between distinct application work surfaces, not just move through steps inside one workspace. The active workspace should determine the visible item, visible tools, available actions, and recent projected results. Switching workspaces must replace the current agent-facing payload so the model sees one oriented surface at a time.

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Switch Between Distinct Work Surfaces (Priority: P1)

As a starter maintainer, I need the demo to expose at least two distinct workspaces so the project demonstrates clean context switching rather than only intra-workflow transitions.

**Why this priority**: The README and planning docs emphasize replacing one oriented work surface with another instead of blending tasks into one ambiguous context window.

**Independent Test**: Starting from the item review workspace, switching to the second workspace replaces the item, workflow name, tools, and actions exposed by the human and agent surfaces.

**Acceptance Scenarios**:

1. **Given** the agent is on the item review workspace, **When** `switch-to-triage-workspace` is chosen, **Then** the next rendered surface shows the triage workspace rather than the prior review surface.
2. **Given** the agent is on the triage workspace, **When** `switch-to-review-workspace` is chosen, **Then** the next rendered surface shows the review workspace rather than the prior triage surface.

---

### User Story 2 - Keep Actions And Tools Scoped To The Active Workspace (Priority: P2)

As a starter maintainer, I need tools and actions from the inactive workspace to disappear after a switch so the context window remains a single, step-scoped decision surface.

**Why this priority**: Context switching only helps if prior workspace affordances are removed from the visible surface instead of silently lingering.

**Independent Test**: After a workspace switch, the inactive workspace's tool ids and action ids no longer appear in the current projection or rendered agent surface.

**Acceptance Scenarios**:

1. **Given** the review workspace exposes review actions and tools, **When** the triage workspace becomes active, **Then** those review affordances no longer appear in the rendered surface.
2. **Given** the triage workspace exposes triage actions and tools, **When** the review workspace becomes active, **Then** those triage affordances no longer appear in the rendered surface.

---

### User Story 3 - Preserve Replay And Inspection Through Workspace Switches (Priority: P3)

As a starter maintainer, I need replay and inspection artifacts to capture workspace changes so the harness can show what surface the agent actually saw before and after a switch.

**Why this priority**: Clean switching is part of the interface claim only if the harness can replay the switch as an interface event.

**Independent Test**: The latest transition artifact records a before surface and after surface with different workspace names when a workspace switch action succeeds.

**Acceptance Scenarios**:

1. **Given** a workspace switch action succeeds, **When** the latest transition artifact is fetched, **Then** it records distinct before and after workspace names.

## Architecture Impact

- **Visible Interface Changes**: The demo now exposes two distinct workspaces with explicit switch actions.
- **Shared Authorship Changes**: Both workspaces remain authored through the same Razor component and authored affordance seams.
- **Runtime Boundary Changes**: Canonical state remains the source of the active workspace, item, step, visible tool ids, available action ids, and projected results.
- **Replay/Inspection Changes**: Replay artifacts now prove work-surface replacement across workspace switches.
- **Stable vs Volatile Regions**: The active workspace is stable for a given step, while visible actions, tools, and results remain volatile with each switch or step transition.

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: The demo workflow MUST expose at least two distinct generic workspaces.
- **FR-002**: Switching workspaces MUST replace the active visible item, tools, actions, and projected results rather than accumulating the prior workspace surface.
- **FR-003**: Human and agent surfaces MUST remain derived from shared canonical state and shared authored seams after a switch.
- **FR-004**: Replay artifacts MUST capture before and after surfaces for a successful workspace switch.
- **FR-005**: The example MUST remain generic and web-developer-legible rather than introducing a bespoke context-switching abstraction.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: The starter demonstrates two distinct generic work surfaces instead of one isolated demo screen.
- **SC-002**: Tests prove that switching workspaces removes inactive tools and actions from the current visible surface.
- **SC-003**: Replay artifacts prove the harness can inspect a workspace switch as an interface event.
