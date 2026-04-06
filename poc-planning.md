

# POC Planning

## Purpose

This file exists to keep planning for the proof of concept anchored to the actual goal.

The goal is **not** to bolt a custom XML/XHTML emitter onto an agent framework.
The goal is **not** to assemble prompts with nicer structure.
The goal is **not** to treat the agent-facing surface as incidental serialization.

The goal is to prove that a serious application can have:

- a human-facing frontend
- an agent-facing frontend
- shared underlying application state and workflow
- distinct projections for each consumer
- a context window used as the delivery target for the currently visible agent-facing surface

A Context Interface Application is therefore closer to **frontend application development for agents** than to prompt engineering.

---

## Core Claim To Prove

A mature existing UI can gain an agent-facing application surface derived from the same state and workflow, with the context window serving as the delivery target for that surface.

This means the POC should demonstrate:

- shared canonical state
- shared workflow position
- step-scoped actions
- relevant result projection
- replayable agent-visible interface states
- clean context switching between work surfaces

---

## Non-Goals

The POC should not drift into these:

- chatbot shell as the main product surface
- giant prompt assembly with nicer formatting
- XML as incidental output only
- globally exposed tools
- autonomous open-ended agent wandering
- toy workflow unrelated to a real UI
- agent memory accumulation as a substitute for interface design
- treating the agent-facing side as hidden glue code

---

## The Important Correction

The agent-facing XHTML is **not** merely a custom renderer output.
It is a **first-class application surface**.

That means it deserves the same class of discipline expected in frontend work:

- structure
- reusable sections or components
- validation
- transforms
- inspection
- replay
- previewability
- deterministic rendering
- step-local updates
- stable versus volatile regions

The context window is the delivery target of the visible surface, not the whole development model.

---

## Rendering Is Not Special

The POC must not drift into treating agent interaction as requiring a special rendering system.

Rendering for the agent is fundamentally the same as rendering for any other consumer:

- the application renders a surface
- that surface is delivered to a consumer
- the consumer acts
- the application updates state
- the next surface is rendered

For the human, the consumer is the browser.
For the agent, the consumer is the model.

This means:

- there is no separate “prompt construction layer”
- there is no hidden “context builder”
- there is no secondary representation of state for the agent

The rendered agent-facing surface **is the context**.

If a piece of state is not rendered into the agent-facing surface, it does not exist for the model.
If a tool does not appear in the rendered surface, it is not available.

This mirrors standard web development patterns:

- conditional rendering already controls visibility
- server-side rendering already controls delivery
- components already encapsulate structure and behavior

The only extension is that the same UI system must support a second render target.

The constraint:

Rendering must remain **mundane and unsurprising**.
The complexity belongs in interface design, not in inventing new rendering pipelines.

---

## Why XHTML Matters

XHTML/XML is not valuable here merely because it is structured.
It is valuable because it has unusual strength on both sides of the problem.

### For the developer

- web structure is familiar
- validation is mature
- transforms are mature
- authoring tools are mature
- inspection habits are mature
- state-to-render thinking is already established

### For the model

- web and document structure are deeply represented in training
- element boundaries are salient
- containment is natural
- closing tags and outdenting help re-establish scope
- structured sections are easier to keep local than prose blobs

### For the architecture

- a web application can already project state into a human-facing surface
- that same state can also project into an agent-facing surface
- shared state plus dual render targets is an architectural advantage, not a hack

The key point: **XHTML is not the whole idea, but it is a particularly strong medium for expressing the agent-facing application surface.**

---

## Frontend Mental Model

The correct mental model is not prompt templating.
The correct mental model is closer to frontend development.

Useful framing:

- the human UI is one frontend
- the agent UI is another frontend
- both derive from the same underlying state
- each gets a different projection
- each has different affordances
- each must remain coherent with the same workflow and domain truth

If React-like frontend thinking feels like the right analogy, that is a sign that the idea is being understood correctly.

This does **not** mean React must be the immediate implementation choice.
It means the development discipline should resemble frontend engineering more than prompt authoring.

---

## What The POC Should Immediately Demonstrate

The first meaningful step after hello world is **not** rendering a trivial "hello world" surface in XHTML or wiring a basic agent loop.

A "hello world" in this domain is not about outputting text at all. It is about establishing the **development workflow** for a Context Interface Application.

This means:

- the ability to stand up a real application surface (not a toy)
- the ability to derive both human-facing and agent-facing views from shared state
- the ability to render a step-scoped, decision-oriented agent interface
- the ability to project that interface into the context window
- the ability to inspect and replay what the agent actually saw

In other words, the first success condition is not "the agent said hello".
The first success condition is "we can develop and evolve an agent-facing frontend with the same discipline as a real UI".

Only after that foundation exists does any trivial demonstration have meaning.

---

## Architectural Shape

The architecture should be discussed in these layers.

### 1. Canonical Application State

This is the source of truth for:

- domain entities
- workflow position
- permissions
- constraints
- recent relevant results
- tool availability rules

This is not the context window.

### 2. Human-Facing Frontend

This may be Blazor now and may eventually resemble React-style workflows more strongly.
Its job is to render a human-oriented interface from canonical state.

### 3. Agent-Facing Frontend

This is the Context Interface Application surface.
It is not a separate system; it is a second render target of the same UI.

This must **not** be implemented as a bespoke parallel UI system.
It must **not** be a second frontend codebase with duplicated logic.
It must **not** be a post-hoc serialization layer.

Instead:

- the agent-facing surface is derived from the same UI system used for the human frontend
- existing UI tooling (e.g., Blazor component model) is extended to support agent-facing projection
- components, layout decisions, and workflow structure originate in the shared UI layer
- agent-specific concerns (visibility, step scope, tool affordances, emphasis) are expressed within that same system

The agent-facing interface is therefore:

- a **second render target** of the same application
- not a separate application
- not a string emitter
- not a simplified mirror of state

XHTML/ForageMap remains a strong structural medium for projection,
but it is the **output of an extended UI system**, not the source of truth.

The core principle:

The system has **one interface definition layer** that can project to multiple consumers (human and agent), rather than separate interface implementations.

### 4. Context Window Projection

The currently visible portion of the agent-facing frontend is projected into the context window.

This projection should be treated as equivalent to delivering a rendered page to a browser:

- it is the exact surface the agent sees
- it is not a transformed or summarized representation
- it is not a secondary data structure

The DOM (or DOM-equivalent structure) of the agent-facing surface is serialized and sent as the inference payload.

Key rule:

**The visible interface is the payload.**
Projection here means selection and delivery, not transformation.

There must be no alternate “context object” that diverges from what is rendered.

If the system cannot show exactly what was sent, the design is incorrect.

### 5. Runtime / Agent Substrate

A framework like Microsoft Agent Framework may execute model calls, tool calls, sessions, and broader runtime behavior.
It must not define the application semantics.

### 6. Replay / Inspection Layer

The system should be able to reconstruct:

- visible agent-facing surface
- visible tools
- visible results
- workflow position
- transition taken

If this cannot be replayed, the harness is incomplete.

---

## Tool Surface Distinction

The POC must preserve the distinction among three tool categories.

### Tools inside the Context Interface Application

These belong to the currently visible application surface.
They are part of the current step and may project results directly into the visible agent-facing interface.

### Tools in the broader runtime

These support orchestration, storage, logging, caching, synchronization, or preparation.
They may affect the system without becoming part of the visible interface.

### Tools for user-side or incidental actions

These relate to human interaction or peripheral system behavior and should not automatically appear in the agent-facing surface.

Rule:

Only tools and results that belong to the current application-facing decision surface should be projected directly into the context window.

All tool effects that matter to the current decision must be expressed through changes to the rendered interface.
Tool execution is therefore equivalent to mutating UI state, not populating hidden data structures.

---

## Context Discipline

The context window is a scarce resource in more than one sense.

- raw token capacity is not equal to usable reasoning capacity
- only a fraction of the visible window is likely to be meaningfully reasoned over
- stable regions matter for cache efficiency
- volatile regions should be localized
- projection should be step-specific, not accumulated blindly

The design question is not “what can fit?”
The design question is “what must be visible for the next valid action?”

---

## Context Switching

A useful part of the idea is that strong interface boundaries allow clean switching between application surfaces or tasks.

Just as switching visible applications helps humans leave one chore behind and enter another,
context switching should replace one oriented work surface with another instead of blending goals, stale state, and irrelevant tools into one ambiguous window.

This matters for:

- multi-task workflows
- multi-application agents
- hybrid human/agent systems
- reduced cross-task hallucination
- reduced chore confusion

---

## POC Stack Discussion Constraints

Future conversation about stack choices should stay anchored to these constraints:

- do not collapse the agent-facing surface into incidental XML output
- do not let a framework own the application semantics
- do not confuse runtime tooling with the visible interface
- do not replace interface design with memory accumulation
- do not treat the context window as the authored model
- do not assume the agent-facing side is simpler than the human-facing side
- do allow frontend-style discipline and tooling to be part of the value proposition

---

## Likely Initial Stack Direction

This is not final, but it captures current likely direction.

- ASP.NET Core / Kestrel as host
- a real human-facing frontend, likely Blazor for the first pass
- an agent-facing projection derived from the same UI component system (e.g., Blazor), rendered into XHTML/ForageMap-like structure
- shared canonical state beneath both
- Microsoft Agent Framework or equivalent used as a substrate, not as the application model
- replay/inspection artifacts as first-class outputs

The key is that the stack should demonstrate **dual frontend projection from shared application state**.

### Important Constraint on Stack Use

UI frameworks such as Blazor are not used only for the human interface.
They are the **authoring environment for both human and agent-facing surfaces**.

Agent frameworks such as Microsoft Agent Framework are not used to define workflows or interfaces.
They are used strictly as a **runtime substrate** for model execution and tool invocation.

The stack must reinforce:

- a single source of truth for interface structure
- shared component logic across both render targets
- projection rather than duplication
- interface discipline over orchestration convenience

---

## Immediate Next Planning Questions

1. What existing advanced UI or workflow should be chosen as the first integration target?
2. What is the minimum canonical state model needed to drive both human and agent surfaces?
3. What part of the agent-facing surface is authored structure versus runtime-projected content?
4. How are step-scoped tools declared and filtered?
5. Which tool results become visible interface state and which remain runtime-internal?
6. What parts of the context projection should be stable versus volatile?
7. What replay artifact should be captured at each step?
8. What degree of frontend-style tooling is needed in the first pass versus later passes?

---

## One-Sentence Summary

The POC should prove that an advanced application can project the same underlying state into both a human-facing frontend and an agent-facing frontend, with the context window serving as the delivery target for the currently visible agent-facing work surface.