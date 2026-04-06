# Context Interface Applications Constitution

## Core Principles

### I. Authored Interface Structure Is The Source Of Truth
The primary artifact in this repository is the authored interface structure that projects into human-facing and agent-facing surfaces. Specifications, prompts, and runtime adapters may constrain implementation work, but they do not replace the authored interface layer as the architectural source of truth.

### II. The Visible Interface Is The Payload
The agent-facing render must remain a direct projection of the visible authored surface. No hidden context object, alternate prompt builder, or secondary representation may diverge from what the agent-facing render shows. If the system cannot show exactly what the agent received, the design is incorrect.

### III. Shared Authorship, Distinct Projection
Human-facing and agent-facing surfaces must derive from shared authored structure whenever possible. Differences between consumers are expressed as projection rules, component behavior, and visibility logic, not as duplicated parallel frontends or string emitters.

### IV. Runtime Substrates Must Remain Infrastructure
Runtime frameworks such as Microsoft Agent Framework are permitted only behind narrow infrastructure seams. They may execute model calls, sessions, or tools, but they must not define workflow semantics, visible interface contracts, or application state shape.

### V. Replay And Inspection Are First-Class
New workflow capabilities must include replayable and inspectable artifacts sufficient to reconstruct visible surfaces, visible tools, available actions, workflow position, and transitions taken. Features that cannot be replayed and inspected are incomplete.

## Starter Constraints

This repository is a starter and reference harness, not a domain application. Contributions must prefer reusable seams over demo-specific branching.

Required starter-oriented boundaries include:

- workflow definitions should be replaceable plug-ins
- authored step surfaces should be registered, not hardcoded in root pages
- action and tool execution should flow through DI seams
- runtime adapters should live outside the web/template project where practical

Temporary demo implementations are acceptable only when they clarify extension points and are kept behind those seams.

## Development Workflow

Feature and refactor specs for this repository must explicitly address:

- which README/planning principle is being reinforced
- which architectural seam is being introduced, refined, or protected
- how the change affects visible interface structure
- whether replay/inspection artifacts need to change
- whether the change introduces or removes demo-specific coupling

Plans and tasks should treat this repository's internal development process as separate from the application architecture being advocated. Spec artifacts govern how this repo evolves; they do not become the source of truth for the applications this starter helps build.

## Governance

This constitution governs future starter evolution. Reviews should reject changes that:

- introduce hidden context-building layers that diverge from rendered output
- let runtime substrates own workflow or interface semantics
- duplicate human and agent surfaces without a clear projection reason
- add demo logic directly into reusable starter layers
- omit replay/inspection updates for new workflow capabilities

Amendments must update this file and explain the architectural reason for the change.

**Version**: 1.0.0 | **Ratified**: 2026-04-05 | **Last Amended**: 2026-04-05
