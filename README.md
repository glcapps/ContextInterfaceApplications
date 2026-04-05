# Context Interface Applications
## The Interface Moves Inside the Model

⸻

SECTION I — Framing the Shift
	1.	From UI to Context: Where the Interface Went
	2.	The Context Window as a First-Class Boundary
	3.	You Already Know This: Translating Web Dev Mental Models
	4.	Why Current LLM Systems Feel Architecturally Wrong

⸻

SECTION II — The Hidden Structure of Today’s Systems
	5.	Implicit Constraints: Where They Actually Live Today
	6.	Prompt Strings as Pre-Framework Chaos
	7.	Orchestration Layers vs Application Architecture
	8.	Debugging Without an Interface

⸻

SECTION III — Model vs Harness: The Emerging Split
	9.	Model Development vs Harness Development
	10.	Determinism in the Model vs Determinism in the Interface
	11.	Why Better Models Don’t Fix Structural Problems
	12.	The Rise of the Harness as a First-Class Layer

⸻

SECTION IV — Context Interface Applications
	13.	What Is a Context Interface Application?
	14.	The Interface as Context, Not UI or API
	15.	Designing for Agent Perception Instead of Human Perception
	16.	Constraints as the Primary Design Tool

⸻

SECTION V — DOM Thinking for Agents (ForageMap Perspective)
	17.	Dual DOM Architecture: Human vs Machine Interfaces
	18.	The ForageMap DOM: Structure, State, and Workflow
	19.	Workflow as Interface: Navigating a Map Instead of a UI
	20.	Tools as Components: Clickable Actions for Agents

⸻

SECTION VI — Core Design Questions
	21.	Where Does State Live?
	22.	What Belongs in Context vs Code?
	23.	How Much Context Is Enough?
	24.	Who Owns the Interface?

⸻

SECTION VII — Failure Modes and Pressure Points
	25.	Context Bloat vs Context Loss
	26.	Structure Collapse During Serialization
	27.	State Drift Across Layers
	28.	Misinterpretation as an Interface Bug

⸻

SECTION VIII — Counterintuitive Realities (Working Notes)
	29.	More Context Can Reduce Accuracy

	•	Adding tokens can dilute signal and shift attention away from the task-critical nodes
	•	Larger windows increase misordering and interference; retrieval must be selective, not exhaustive

	30.	Less Freedom Can Improve Outcomes

	•	Constraining available actions (tools/steps) reduces error surface
	•	Workflow maps outperform open-ended reasoning loops for repeatable tasks

	31.	Structure Does Not Guarantee Understanding

	•	XML/DOM shape helps, but serialization to linear tokens can collapse hierarchy
	•	The model may ignore or misweight nodes; placement and repetition still matter

	32.	Determinism Emerges from Constraints, Not Control

	•	JSON schemas, retries, and guardrails attempt control at the model layer
	•	Predictability improves more from constrained interfaces (steps, scoped tools) than from tighter prompts

	33.	Constraints Already Exist—They’re Just Hidden

	•	Today’s systems embed rules across prompts, code, and validators
	•	Elevating them into the context interface makes them inspectable and testable

	34.	The Interface Is Invisible but Primary

	•	Developers are still designing an interface—they just can’t see it
	•	Context composition (what is present, ordered, omitted) is the real UX for the agent

	35.	Maps vs Open Terrain

	•	Guided traversal (maps) increases reliability; open exploration increases flexibility
	•	Most production systems implicitly choose maps but don’t declare them

	36.	The Wrong Layer for Determinism

	•	Industry trend: force determinism out of probabilistic models
	•	Alternative: place determinism in the interface (workflow, state, allowed actions)

⸻

SECTION IX — Toward a New Discipline
	33.	Context Interface Design as a Field
	34.	From Prompt Engineering to Interface Engineering
	35.	What a Context Layout System Might Look Like
	36.	The Role of Application Developers in the Next Wave

⸻

SECTION X — Technology Stack and Runtime Surfaces
	41.	What Runs the Human UI?
	42.	What Runs the Server Harness?
	43.	Browser, Server, CLI, and Hybrid Runtime Choices
	44.	Rendering the Human DOM and the Context Interface Together
	45.	Filesystems, Databases, and Context State Stores
	46.	Tool Execution Surfaces: APIs, SQL, Scripts, and Components

⸻

SECTION XI — Forward Outlook
	47.	Standardizing Context Interfaces
	48.	Multi-Agent Context Negotiation
	49.	The Future of CLI, SPA, and Agent Systems
	50.	The Post-UI Application Stack
