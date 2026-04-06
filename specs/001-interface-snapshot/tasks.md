---

description: "Task list for implementing structured interface snapshots"
---

# Tasks: Interface Snapshot Scaffold

**Input**: Design documents from `/specs/001-interface-snapshot/`
**Prerequisites**: plan.md, spec.md

**Tests**: Starter changes in this feature require integration and rendering/unit tests because the work adds a new reusable seam and changes replay/inspection outputs.

**Organization**: Tasks are grouped by architectural slice so the snapshot seam can be implemented and verified independently of the current demo workflow.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (e.g., US1, US2, US3)
- Include exact file paths in descriptions

## Phase 1: Seam Setup

- [ ] T001 [US1] Add snapshot models in `src/ContextInterfaceApplications.Web/Models/` for `InterfaceSnapshot`, `InterfaceNode`, and any node-type or projection metadata needed
- [ ] T002 [US1] Add a starter seam in `src/ContextInterfaceApplications.Web/Services/` for building the current agent-facing snapshot
- [ ] T003 [P] [US1] Register the snapshot service in `src/ContextInterfaceApplications.Web/Program.cs` and mirror the registration in `tests/ContextInterfaceApplications.Tests/TestServiceFactory.cs`
- [x] T003a [US1] Add authored step-section metadata models and a resolver seam in `src/ContextInterfaceApplications.Web/Models/` and `src/ContextInterfaceApplications.Web/Services/`

## Phase 2: Authored Interface Alignment

- [ ] T004 [US1] Implement snapshot construction from existing workflow state, visible tools, available actions, projected results, and runtime metadata without adding demo-specific branching to shared pages or services
- [ ] T005 [P] [US3] Ensure the snapshot seam uses starter abstractions such as `IWorkflowDefinition`, `IStepComponentResolver`, `IActionComponentResolver`, and `IAgentRuntimeSubstrate` rather than direct demo-only assumptions
- [x] T005a [US1] Add a shared Razor wrapper component in `src/ContextInterfaceApplications.Web/Components/Shared/ProjectionSection.razor` so step surfaces can author section metadata through normal component composition
- [x] T005b [US3] Add `src/ContextInterfaceApplications.Web/Workflows/Demo/DemoStepSurfaceMetadataResolver.cs` and step component-backed section definitions so snapshots consume authored Razor metadata for the current demo workflow

## Phase 3: Projection And Replay

- [ ] T006 [US1] Expose the current agent-facing snapshot through an inspection-friendly endpoint in `src/ContextInterfaceApplications.Web/Program.cs`
- [ ] T007 [US2] Extend `src/ContextInterfaceApplications.Web/Models/TransitionArtifact.cs` and `src/ContextInterfaceApplications.Web/Services/ReplayArtifactStore.cs` to store structured before/after snapshots alongside rendered payloads
- [ ] T008 [P] [US2] Update replay/debug endpoint payloads in `src/ContextInterfaceApplications.Web/Program.cs` so transition inspection returns structured snapshots as well as rendered artifacts

## Phase 4: Runtime And Composition

- [ ] T009 [US1] Keep runtime substrate description nodes clearly separated from step-local workflow, tool, action, and result nodes
- [ ] T010 [US3] Validate that the snapshot seam remains starter-owned and does not duplicate the agent render path into a hidden prompt-building architecture

## Phase 5: Verification

- [ ] T011 [US1] Add or update rendering/service tests in `tests/ContextInterfaceApplications.Tests/RenderingTests.cs` for current snapshot construction
- [ ] T012 [US1] Add or update endpoint coverage in `tests/ContextInterfaceApplications.Tests/ApplicationEndpointsTests.cs` for retrieving the current agent-facing snapshot
- [ ] T013 [US2] Add or update endpoint coverage in `tests/ContextInterfaceApplications.Tests/ApplicationEndpointsTests.cs` verifying accepted actions record both rendered and structured before/after transition artifacts
- [ ] T014 [US3] Verify tests exercise the seam through starter abstractions rather than demo-only code paths
- [x] T014a [US1] Verify the snapshot includes authored step-section nodes sourced from Razor-owned metadata rather than only from flat state

## Dependency Rule

- Snapshot models and service seam come before replay integration
- Replay integration comes before endpoint and test verification
- Verification must prove the snapshot is a structural sibling of the rendered payload, not a separate hidden architecture

## Notes

- Keep the first pass focused on agent-facing snapshots; human-facing comparative snapshots can remain a later feature
- Prefer adding typed node structures over embedding arbitrary dictionaries into replay artifacts
- If a snapshot representation cannot be related back to the rendered surface, it is too detached from the README intent
- Keep authored metadata additions web-familiar. Wrapper components and component-owned definitions are acceptable; a bespoke developer-authored context DSL is not
