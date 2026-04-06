# Spec Kit Mapping For This Repository

This repository uses Spec Kit only as an internal development process for evolving the starter.

It does not imply that applications built with this starter should treat markdown specs as the source of truth. The source of truth for Context Interface Applications remains the authored interface structure and the visible rendered surfaces derived from it.

## What Spec Kit Governs Here

Spec Kit artifacts in this repository exist to help with:

- documenting architectural intent before implementation
- planning reusable seams and starter boundaries
- tracking replay and inspection impacts of changes
- preventing demo-specific shortcuts from becoming architecture

## What Spec Kit Does Not Govern

Spec Kit files do not replace:

- canonical application state models
- authored Blazor component structure
- interface snapshots or replay artifacts
- runtime adapter boundaries

Those remain code and runtime artifacts inside the starter.

## Artifact Mapping

### `spec.md`

Use `spec.md` to describe a starter capability or architectural evolution in terms of:

- interface structures affected
- human and agent projection implications
- replay or inspection changes
- starter seams introduced or protected

### `plan.md`

Use `plan.md` to document:

- which reusable boundaries are added or refined
- which projects or folders should own the change
- how runtime and authored-interface concerns remain separate

### `tasks.md`

Use `tasks.md` to break the work into implementation steps that keep:

- demo logic out of reusable layers
- replay and inspection artifacts updated
- tests aligned with starter extension points

## Definition Of Good Use In This Repo

Spec-driven work is considered aligned only if it reinforces the constitution and the README principles, especially:

- the visible interface is the payload
- authored interface structure is the source of truth
- runtime substrates remain infrastructure
- replay and inspection are first-class outputs
- shared authored structure drives both human and agent-facing projections
