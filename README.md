# Echoes of Memories

**Echoes of Memories** is a narrative-driven 2D demo developed in **Unity** as an academic project.  
The focus of this project is not scale or complexity, but **clean system design, controlled scene flow, and narrative-based interaction logic**.

The demo demonstrates how small, well-scoped mechanics can be combined to create a coherent interactive experience.

---

## 🎯 Project Purpose

This project was designed to demonstrate understanding of:

- Unity scene management and transitions
- State-based gameplay progression
- UI systems and player interaction
- Runtime data persistence across scenes
- Event-driven logic instead of hard-coded sequences
- Clean, modular C# scripting practices

The project is intentionally built as a **demo**, not a full game.

---

## 🛠 Technical Overview

- **Engine:** Unity (2D)
- **Language:** C#
- **Architecture:** Modular, component-based
- **Platform:** PC

---

## 🎮 Gameplay Mechanisms

1. Mirror
   -  Player is reflected using a mask
3. Time manager  
   - Each 10 minutes in the real time, is an hour in the game
   - The curtains are open during night, and they are closed during daylight
   - The player can sleep only when it is night time  
4. Inventory  
   - A safe place to keep your belongings intact! 
5. Diary
   - The core mechanism of the game
   - The player can keep record of his investigations
   - The player can also find new informations in the diary    
7. MiniGame
   - There is a game inside of the game, so the player never gets bored  
     

---

## ✨ Implemented Systems

### 1. Scene Transition System
- Fade-based transitions
- Controlled spawn routing per scene
- Decoupled from player logic

### 2. Diary System
- Player-written diary entries
- Automatic diary entries triggered by events
- Diary interaction affects world state

### 3. Inventory System
- Runtime-persistent inventory manager
- Inventory UI toggled via input (`I`)
- Items visually represented in UI
- Inventory resets on game restart (intentional for demo clarity)

### 4. Conditional World Logic
- Key object hidden until narrative condition is met
- Locked door provides contextual feedback
- Door unlocks only when required item is obtained

### 5. UI & Interaction Design
- Context-based interaction prompts
- Temporary message popups
- Player control locking during UI interactions
- Minimal HUD to avoid distraction


Progression is controlled by **player actions and game state**, not timers or scripted shortcuts.
---

## ⌨ Controls

| Key | Action |
|----|-------|
| **E** | Interact |
| **I** | Toggle inventory |
| **Esc** | Pause the game |
| **Space** | Dismiss message popups |

---

## 🧠 Design Decisions

- **No save system:**  
  The demo resets on restart to ensure deterministic behavior during evaluation.
- **Minimal UI:**  
  Only essential information is shown to the player.
- **State-driven progression:**  
  World changes are triggered by internal game state rather than scene-specific scripts.

These decisions were made to prioritize **clarity, maintainability, and demonstrability**.

---

## 🗂 Scene List

- `MainMenu`
- `Credits`
- `Guide`
- `Opening`
- `Bedroom`
- `RBathroom`
- `DemoEnd`

---

## ⚠ Scope Limitations

- The project is not intended to be a full commercial game
- No persistence beyond runtime
- Asset polish is secondary to system implementation
- Designed primarily for academic evaluation and demonstration

---

## 📚 Learning Outcomes

Through this project, the following concepts were applied:

- Event-driven programming in Unity
- Cross-scene data handling
- UI state management
- Input handling and player control gating
- Clean separation of gameplay systems

---

## 👤 Author

**Medisa**  
Computer Engineering   
Game Development / Unity / C#

---

*This project was developed as an academic demonstration of interactive system design rather than a complete game.*
