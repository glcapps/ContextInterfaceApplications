---

description: "Task list for implementing authored affordance contracts"
---

# Tasks: Authored Affordance Contracts

**Input**: Design documents from `/specs/002-authored-affordance-contracts/`
**Prerequisites**: plan.md, spec.md

**Tests**: Starter changes in this feature require integration and rendering/unit tests because the work changes visible affordance ownership and must preserve existing execution boundaries.

**Organization**: Tasks are grouped by architectural slice so authored affordance metadata can be introduced without collapsing runtime and workflow responsibilities into the component layer.

## Format: `[ID] [P?] [Story] Description`

- **[P]**: Can run in parallel (different files, no dependencies)
- **[Story]**: Which user story this task belongs to (e.g., US1, US2, US3)
- Include exact file paths in descriptions

## Phase 1: Seam Setup

- [x] T001 [US1] Add authored affordance metadata models in `src/ContextInterfaceApplications.Web/Models/` for tool/action contracts and any visibility or projection metadata needed
- [x] T002 [US1] Add a starter seam in `src/ContextInterfaceApplications.Web/Services/` for resolving authored affordance contracts by step and projection target
- [x] T003 [P] [US1] Register the authored affordance seam in `src/ContextInterfaceApplications.Web/Program.cs` and mirror it in `tests/ContextInterfaceApplications.Tests/TestServiceFactory.cs`

## Phase 2: Authored Interface Alignment

- [x] T004 [US1] Add shared Razor projection helpers in `src/ContextInterfaceApplications.Web/Components/Shared/` so authored affordance contracts can render into both human and agent surfaces
- [x] T005 [US1] Update the demo workflow or shared affordance components in `src/ContextInterfaceApplications.Web/Components/Shared/` and `src/ContextInterfaceApplications.Web/Workflows/Demo/` to expose authored tool/action metadata through the new seam
- [x] T006 [P] [US3] Ensure shared pages such as `src/ContextInterfaceApplications.Web/Components/HumanSurfacePage.razor` and `src/ContextInterfaceApplications.Web/Components/AgentSurfacePage.razor` remain generic and do not gain demo-specific affordance branching

## Phase 3: Projection And Replay

- [x] T007 [US1] Update `src/ContextInterfaceApplications.Web/Components/Shared/VisibleToolsPanel.razor` and `src/ContextInterfaceApplications.Web/Components/Shared/AgentActionContractPanel.razor` to consume authored affordance metadata where appropriate
- [x] T008 [US1] Update `src/ContextInterfaceApplications.Web/Services/AgentInterfaceSnapshotBuilder.cs` so snapshots include affordance nodes derived from authored metadata
- [x] T009 [P] [US2] Ensure replay and transition artifacts continue to expose the visible affordance contracts after the authored seam is introduced

## Phase 4: Runtime And Validation Boundaries

- [x] T010 [US2] Confirm tool invocations still execute through `src/ContextInterfaceApplications.Web/Services/IContextToolInvoker.cs`, `src/ContextInterfaceApplications.Web/Services/ContextToolInvoker.cs`, and `src/ContextInterfaceApplications.Web/Services/IContextToolHandler.cs`
- [x] T011 [US2] Confirm action validation and state mutation remain in `src/ContextInterfaceApplications.Web/Services/IWorkflowDefinition.cs` and `src/ContextInterfaceApplications.Web/Services/CanonicalStateStore.cs`
- [x] T012 [US3] Validate that authored affordance metadata remains projection/input structure only and does not become a second bespoke authored context language

## Phase 5: Verification

- [x] T013 [US1] Add or update rendering/service tests in `tests/ContextInterfaceApplications.Tests/RenderingTests.cs` for authored affordance projection into human/agent outputs and snapshots
- [x] T014 [US1] Add or update endpoint coverage in `tests/ContextInterfaceApplications.Tests/ApplicationEndpointsTests.cs` proving visible tool/action contracts still render correctly after the authored seam is introduced
- [x] T015 [US2] Add or update endpoint coverage in `tests/ContextInterfaceApplications.Tests/ApplicationEndpointsTests.cs` proving tool calls and action posts still flow through the existing runtime/workflow boundaries
- [x] T016 [US3] Verify tests exercise the new affordance seam through starter abstractions rather than demo-only code paths

## Dependency Rule

- Affordance models and resolver seam come before shared projection helpers
- Shared projection helpers come before snapshot integration
- Runtime and validation checks must pass before final verification

## Notes

- Keep the first pass small: it is enough if one layer of visible tools/actions becomes clearly authored from Razor-friendly metadata
- Avoid moving handler logic, workflow transitions, or execution policy into components
- Favor wrapper components and component-owned definitions over inventing a separate authored affordance DSL
