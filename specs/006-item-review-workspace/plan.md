# Implementation Plan: Item Review Workspace

## Goal

Replace the synthetic bootstrap flow with a generic, application-shaped item review workspace while keeping the existing starter seams intact.

## Scope

- Introduce a generic current review item into canonical runtime state.
- Rework the demo workflow to use realistic review states and branching actions.
- Keep authored sections, authored affordances, projection, snapshot, replay, and DI tool execution on the existing seams.
- Update the demo surfaces and tests to reflect the new generic workspace.

## Design

### Runtime State

- Add a `ReviewItem` record to the shared runtime state.
- Keep runtime state responsible for workflow facts, current ids, projected results, and the current item.
- Do not move authored affordance ownership into runtime state.

### Workflow

- Rework `DemoWorkflowDefinition` into a generic item-review flow with these states:
  - `new-item`
  - `in-review`
  - `needs-followup`
  - `approved`
- Support these actions:
  - `start-review`
  - `approve-item`
  - `request-followup`
  - `resume-review`
  - `reopen-review`

### Authored UI

- Keep Razor components as the authored surface.
- Add a shared item-display component for both human and agent consumers.
- Update step-surface metadata to describe the new review-oriented sections.
- Keep visible tools and actions authored through the existing affordance seam.

### Tools

- Replace architecture-demo tool labels with generic review-oriented tools.
- Keep execution behind `IContextToolInvoker` and `IContextToolHandler`.

### Verification

- Update rendering and endpoint tests to assert:
  - the current review item is visible on both surfaces
  - actions branch correctly
  - tool visibility remains state-scoped
  - snapshots and replay still work on the new slice
