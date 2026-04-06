# Context Interface Applications Development Guidelines

Auto-generated from accepted feature plans. Last updated: [DATE]

## Active Technologies

[EXTRACTED FROM PLAN.MD FILES]

## Starter Structure

```text
[ACTUAL STRUCTURE FROM PLANS]
```

## Current Baseline

- .NET 10 ASP.NET Core starter with Blazor-authored human and agent-facing surfaces
- Runtime substrate abstractions in `src/ContextInterfaceApplications.Runtime.Abstractions/`
- MAF adapter isolated in `src/ContextInterfaceApplications.Runtime.Maf/`
- Reusable starter seams in `src/ContextInterfaceApplications.Web/Services/`
- Demo workflow implementations in `src/ContextInterfaceApplications.Web/Workflows/Demo/`
- Integration and rendering tests in `tests/ContextInterfaceApplications.Tests/`

## Architectural Reminders

- Authored interface structure remains the source of truth for visible surfaces.
- Human and agent-facing surfaces should derive from shared authored structure wherever possible.
- Runtime substrates stay behind infrastructure boundaries and do not own workflow semantics.
- Replay and inspection changes are required whenever workflow capabilities change.
- Prefer reusable starter seams over demo-specific branching.

## Commands

[ONLY COMMANDS FOR ACTIVE TECHNOLOGIES]

## Recent Starter Evolutions

[LAST 3 ACCEPTED FEATURES AND WHAT SEAMS THEY ADDED OR REFINED]
