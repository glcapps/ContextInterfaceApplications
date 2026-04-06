# [PROJECT_NAME] Constitution

## Core Principles

### I. Authored Interface Structure Is The Source Of Truth
[Describe how visible interface behavior must originate from authored structure such as components, workflow definitions, and explicit projection contracts rather than prompt strings or hidden builders.]

### II. The Visible Interface Is The Payload
[Describe how the model only knows what is rendered into the agent-facing surface, and how hidden context objects or unrendered tool access are disallowed.]

### III. Shared Authorship, Distinct Projection
[Describe how human and agent-facing surfaces should derive from shared underlying authored structure while still allowing consumer-specific projection.]

### IV. Runtime Substrates Remain Infrastructure
[Describe how frameworks such as MAF, tool runtimes, and hosting layers stay behind narrow seams and must not own workflow semantics or interface design.]

### V. Replay And Inspection Are First-Class
[Describe how new workflow or interface capabilities must update replay, transition, and debug/inspection artifacts.]

## Additional Constraints

[List starter-specific constraints such as keeping demo logic behind reusable seams, preserving deterministic rendering, and maintaining clear project ownership.]

## Development Workflow

[Describe how specs, plans, tasks, tests, and implementation should stay aligned with the README principles and constitution checks.]

## Governance

[Describe how constitutional exceptions must be justified, documented, and reviewed before merging.]

**Version**: [CONSTITUTION_VERSION] | **Ratified**: [RATIFICATION_DATE] | **Last Amended**: [LAST_AMENDED_DATE]
