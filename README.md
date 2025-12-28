# ai-dodgeball-fsm
Unity AI Dodgeball project implementing a finite state machine and throw-decision logic for autonomous agents.

# AI Dodgeball — Finite State Machine Project

**Autonomous dodgeball agents built with finite state machines and decision-based targeting logic in Unity.**

This project demonstrates fundamental game AI concepts, including state-based behavior, navigation, and throw decision logic for autonomous agents in a dodgeball simulation.

---

## Overview

In this project, each agent (minion) uses a **finite state machine (FSM)** to decide what to do at any moment:
- **Collect balls**
- **Navigate the play area**
- **Choose safe and effective throws**
- **React to teammates and opponents**

The AI uses decision logic to balance aggression and safety, avoid obstacles, and select targets intelligently.

---

##  Project Structure

All student-authored code is included here:


Code outside `src/` is not included; see *How to Run* below.

---

##  Code Highlights

- **Finite State Machine (FSM):**  
  Encapsulates agent behaviors as states (e.g., seek ball, throw, evade) with clear transition logic.
- **Target & Safety Decisions:**  
  Logic in `ShotSelection.cs` evaluates potential throws based on distance, occlusion, and opponent positions.
- **Movement Steering (FuzzyVehicle):**  
  Handles agent movement and navigation, blending steering behaviors.

---

## How to Run

This repository contains **AI scripts only**. The full Unity project and core game framework are **not included** due to course distribution guidelines.

To run locally:
1. Clone the original PrisonDodgeball / GameAI framework.
2. Copy the scripts from `src/` into the framework’s `GameAIStudent/` folder.
3. Open the Unity project and press Play.

---

## What I Learned

- Design and implementation of finite state machines for AI
- Structuring decision logic for autonomous agents
- Integrating navigation and behavior decision making
- Debugging and balancing behavior priorities

---

## Technologies

- **Unity**
- **C#**
- **Game AI architecture**
- **Finite State Machines**
- **Navigation & Steering Logic**

### Quick Code Tour

- **`MinionStateMachine.cs`** – Defines states and transitions based on agent status and environment.
- **`ShotSelection.cs`** – Evaluates possible throw targets and selects the safest, highest-priority option.
