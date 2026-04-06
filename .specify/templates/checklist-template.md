# [CHECKLIST TYPE] Checklist: [FEATURE NAME]

**Purpose**: Verify that a starter change remains aligned with the Context Interface Applications constitution  
**Created**: [DATE]  
**Feature**: [Link to spec.md or relevant documentation]

## Interface Alignment

- [ ] Visible interface changes are explicitly described
- [ ] Shared authored structure remains the source of truth
- [ ] Human and agent projections do not diverge without a documented reason

## Runtime Boundaries

- [ ] Runtime substrates do not gain workflow or interface ownership
- [ ] Tool execution remains behind DI/runtime seams
- [ ] No hidden context-building layer is introduced

## Replay And Inspection

- [ ] Replay artifacts are updated if workflow behavior changes
- [ ] Inspection/debug surfaces remain able to show what the agent saw
- [ ] New starter seams are testable through integration or rendering tests

## Starter Hygiene

- [ ] Demo behavior is pushed behind starter seams rather than into reusable layers
- [ ] New abstractions are generic enough for reuse beyond the demo workflow
- [ ] File/project ownership remains clear
