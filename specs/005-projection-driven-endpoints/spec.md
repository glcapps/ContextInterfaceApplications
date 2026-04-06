# Feature Specification: Projection-Driven Endpoints

**Feature Branch**: `005-projection-driven-endpoints`  
**Created**: 2026-04-06  
**Status**: Draft  
**Input**: User description: "Expose the current visible projection as a first-class endpoint surface and make debug/inspection consume that same projection boundary."

## Intent

Add projection-driven HTTP endpoints so the repository’s outward inspection surfaces align with the architecture already established in code: runtime state endpoints expose workflow facts, projection endpoints expose current visible interface composition, rendered endpoints expose consumer-specific output, and replay endpoints expose stored artifacts. This keeps the project closer to the README goal of inspectable interface architecture without drifting into bespoke tooling.

## User Scenarios & Testing *(mandatory)*

### User Story 1 - Inspect Current Visible Projection Directly (Priority: P1)

As a starter maintainer, I need an endpoint that returns the current visible projection for a specific consumer so I can inspect the assembled interface structure without parsing rendered markup or combining runtime state with authored metadata myself.

**Why this priority**: The projection boundary now exists in code. The next step is to expose it explicitly so the architecture is visible at the HTTP surface as well.

**Independent Test**: A request to the projection endpoint returns the current sections, tools, and actions for the requested target.

**Acceptance Scenarios**:

1. **Given** the workflow is on a step, **When** `/api/projections/agent` is requested, **Then** the response includes the current agent-facing sections, tools, and actions.
2. **Given** the workflow is on a step, **When** `/api/projections/human` is requested, **Then** the response includes the current human-facing sections, tools, and actions for that same runtime state.

---

### User Story 2 - Align Debug Inspection With The Projection Boundary (Priority: P2)

As a starter maintainer, I need the debug endpoint to consume the same projection seam used by pages and snapshots so the inspection story does not rely only on rendered payload assembly.

**Why this priority**: The debug route is an obvious inspection surface. If it bypasses the projection boundary, the architecture remains coherent only inside the code, not at the inspection layer.

**Independent Test**: The debug endpoint shows information derived from the current projection seam in addition to the rendered payload.

**Acceptance Scenarios**:

1. **Given** the debug endpoint is requested, **When** the current projection is available, **Then** the response includes projection-derived metadata such as current sections, tools, and actions alongside the rendered payload.

---

### User Story 3 - Preserve Clear Endpoint Roles (Priority: P3)

As a starter maintainer, I need runtime state, projection, rendered surface, and replay endpoints to have clearer distinct roles so the repository is easier to understand from outside the codebase.

**Why this priority**: The next readers of the project should not have to infer the layer map from service registrations alone. The endpoint surface should mirror the architecture.

**Independent Test**: The code clearly separates endpoints that return runtime state, current projection, rendered surface output, and replay artifacts.

**Acceptance Scenarios**:

1. **Given** a maintainer inspects `Program.cs`, **When** they review the endpoint map, **Then** projection endpoints are clearly distinct from runtime state and replay endpoints.

### Architecture Impact

- **Visible Interface Changes**: None to the end-user-facing rendered surfaces; this is an inspection/API layering change.
- **Shared Authorship Changes**: No new authored metadata is required.
- **Runtime Boundary Changes**: Runtime state remains separate from projection and render output.
- **Replay/Inspection Changes**: Projection becomes a first-class inspection surface rather than an internal-only service.
- **Stable vs Volatile Regions**: Projection endpoints expose volatile current visible interface content while keeping runtime-state endpoints narrower.

### Edge Cases

- How should projection endpoints behave for invalid targets?
- How much of the projection should the debug page show without becoming a second bespoke inspector UI?
- How should projection endpoints and replay endpoints differ when the current state has moved past the last stored replay artifact?

## Requirements *(mandatory)*

### Functional Requirements

- **FR-001**: The repository MUST expose a current projection endpoint for at least human and agent targets.
- **FR-002**: The projection endpoint MUST return the starter-owned current projection model rather than rendered markup.
- **FR-003**: The debug agent surface MUST consume the current projection seam in addition to the rendered payload.
- **FR-004**: Runtime state endpoints, projection endpoints, render endpoints, and replay endpoints MUST remain distinct in responsibility.

### Key Entities *(include if feature involves data)*

- **Projection Endpoint**: An HTTP endpoint that returns the current visible interface projection for a target.
- **Rendered Surface Endpoint**: An HTTP endpoint that returns consumer-specific rendered output.
- **Inspection Surface**: A debug-oriented output that helps a maintainer inspect the current visible interface.

## Success Criteria *(mandatory)*

### Measurable Outcomes

- **SC-001**: A maintainer can request the current visible projection directly over HTTP.
- **SC-002**: The debug route reflects the current projection boundary rather than only rendered markup.
- **SC-003**: The endpoint surface more clearly mirrors the repository’s runtime/projection/render/replay architecture.

## Assumptions

- The first pass can keep the projection endpoint simple and unversioned.
- The first pass can limit projection targets to `human` and `agent`.
- The debug route can show projection summaries rather than a full custom inspector UI.
