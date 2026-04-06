---

description: "Task list template for feature implementation"
---

# Tasks: [FEATURE NAME]

**Input**: Design documents from `/specs/[###-feature-name]/`
**Prerequisites**: plan.md (required), spec.md (required for user stories), research.md, data-model.md, contracts/

**Tests**: Starter changes should usually include rendering or integration tests whenever a seam, projection, or replay behavior changes.

**Organization**: Tasks should be grouped by user story or architectural slice so reusable seams can be implemented and verified independently.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (e.g., US1, US2, US3)
- Include exact file paths in descriptions

## Path Conventions

- Runtime abstractions: `src/ContextInterfaceApplications.Runtime.Abstractions/`
- Runtime adapters: `src/ContextInterfaceApplications.Runtime.*/`
- Starter web/template code: `src/ContextInterfaceApplications.Web/`
- Tests: `tests/ContextInterfaceApplications.Tests/`
- Spec process artifacts: `.specify/`, `specs/`, `docs/`

## Phase 1: Seam Setup

- [ ] T001 Add or refine the starter seam described in the spec
- [ ] T002 Register the seam in the composition root if needed
- [ ] T003 [P] Add or update starter models/contracts for the seam

## Phase 2: Authored Interface Alignment

- [ ] T004 Update authored workflow definitions, Blazor components, or projection contracts to use the seam
- [ ] T005 [P] Move any remaining demo behavior behind the seam instead of hardcoding it in reusable layers

## Phase 3: Projection And Replay

- [ ] T006 Update human and/or agent projection paths affected by the seam
- [ ] T007 Update replay/inspection artifacts if visible interface behavior changes
- [ ] T008 [P] Update debug or inspection endpoints if needed

## Phase 4: Runtime And Composition

- [ ] T009 Update DI registration, runtime adapters, or tool/action invocation seams if needed
- [ ] T010 Validate that runtime ownership still stops at infrastructure boundaries

## Phase 5: Verification

- [ ] T011 Add or update integration tests in `tests/ContextInterfaceApplications.Tests/`
- [ ] T012 Add or update rendering/unit tests where useful
- [ ] T013 Validate that no new hidden architecture bypasses authored, rendered, or replayable surfaces

## Dependency Rule

- Starter seam work comes before demo-specific updates
- Demo updates come before replay/debug integration
- Tests close the loop on the seam, not only on the demo path

### Incremental Delivery

1. Complete Seam Setup → Starter boundary ready
2. Add User Story 1 → Test independently → Deploy/Demo (MVP!)
3. Add User Story 2 → Test independently → Deploy/Demo
4. Add User Story 3 → Test independently → Deploy/Demo
5. Each story adds value without breaking previous stories

### Parallel Team Strategy

With multiple developers:

1. Team completes Seam Setup together
2. Once Seam Setup is done:
   - Developer A: User Story 1
   - Developer B: User Story 2
   - Developer C: User Story 3
3. Stories complete and integrate independently

---

## Notes

- [P] tasks = different files, no dependencies
- [Story] label maps task to specific user story for traceability
- Each user story should be independently completable and testable
- Prefer task descriptions that name the exact seam, component, and test files involved
- Verify tests fail before implementing
- Commit after each task or logical group
- Stop at any checkpoint to validate story independently
- Avoid: vague tasks, same file conflicts, cross-story dependencies that break independence
