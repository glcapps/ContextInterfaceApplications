---

description: "Task list for documenting the retrospective starter foundation baseline"
---

# Tasks: Retrospective Foundation Baseline

**Input**: Design documents from `/specs/000-retrospective-foundation/`
**Prerequisites**: plan.md, spec.md

**Tests**: No executable tests required. Validation is documentation-to-code alignment.

**Organization**: Tasks are grouped around documenting the current baseline clearly and making it usable for future specs.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (e.g., US1, US2, US3)
- Include exact file paths in descriptions

## Phase 1: Baseline Capture

- [ ] T001 [US1] Review current starter seams in `src/ContextInterfaceApplications.Web/Services/`, `src/ContextInterfaceApplications.Runtime.Abstractions/`, `src/ContextInterfaceApplications.Runtime.Maf/`, and `src/ContextInterfaceApplications.Web/Program.cs`
- [ ] T002 [US1] Confirm the retrospective baseline in `specs/000-retrospective-foundation/spec.md` matches the current code-owned seams and constitution language

## Phase 2: Demo Separation

- [ ] T003 [US2] Verify the baseline spec distinguishes demo implementations in `src/ContextInterfaceApplications.Web/Workflows/Demo/` from reusable starter seams
- [ ] T004 [P] [US2] Verify the documented projection and resolver seams match `src/ContextInterfaceApplications.Web/Components/` and `src/ContextInterfaceApplications.Web/Services/`

## Phase 3: Future Spec Readiness

- [ ] T005 [US3] Ensure the retrospective spec can be used as the reference baseline for `specs/001-interface-snapshot/spec.md`
- [ ] T006 [P] [US3] Keep `.specify/templates/` and `docs/spec-kit-mapping.md` aligned with the existence of a retrospective baseline

## Phase 4: Verification

- [ ] T007 [US1] Compare the baseline spec against `tests/ContextInterfaceApplications.Tests/ApplicationEndpointsTests.cs` and `tests/ContextInterfaceApplications.Tests/RenderingTests.cs` to ensure replay/runtime/tool seams are reflected accurately
- [ ] T008 [US1] Confirm no code changes are introduced under this retrospective spec

## Dependency Rule

- Baseline capture comes before demo-separation validation
- Demo-separation validation comes before future-spec readiness checks
- Verification closes the loop that this is documentation-only and aligned to current code

## Notes

- This retrospective spec should document only major starter seams, not every historical edit
- If the documented baseline and the code disagree, fix the documentation first unless the user explicitly asks for a code correction
