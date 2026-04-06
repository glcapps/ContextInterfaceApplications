# Implementation Plan: Context Switching Between Workspaces

## Goal

Add a second generic workspace and make workspace switching a first-class interface transition.

## Scope

- Keep the existing item review workspace.
- Add a second generic triage workspace.
- Add switch actions that replace the active workspace state.
- Update projections, rendered surfaces, and replay tests to prove replacement rather than accumulation.

## Design

### Runtime State

- Continue to keep one active current item and one active workflow name in canonical state.
- Use the current state as the sole source of the active workspace.
- On a workspace switch, replace the item, step, visible ids, and projected results with the destination workspace state.

### Authored UI

- Keep Razor components as the authored interface layer.
- Add triage-specific step surfaces and action components.
- Add a shared workspace-switch action component so switching is visible as part of the authored surface.

### Tools And Actions

- Keep review-local tools and actions scoped to review steps.
- Add triage-local tools and actions scoped to triage steps.
- Add workspace switch actions across both workspaces.

### Verification

- Add tests for:
  - switching from review to triage
  - switching back from triage to review
  - inactive workspace affordances disappearing
  - replay artifacts showing distinct before/after workspace names
