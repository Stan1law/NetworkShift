# Network Shift

## Overview

**Network Shift** is a Unity 6 educational networking simulation game designed for college-level students learning basic computer networking through interactive gameplay.

Players take on the role of an office employee troubleshooting network outages, learning fundamental networking concepts like network switches, Ethernet cables, ports, and device connectivity—all through hands-on interaction rather than traditional quizzes.

---

## Project Information

- **Engine:** Unity 6
- **Target Audience:** College-level students (networking fundamentals)
- **Development Status:** In Progress
- **Current Mission:** Mission 01 — Network Outage

---

## Current Progress

### Mission 01 — Network Outage ✅ (In Progress)

**Objective Flow:**
1. ✅ Inspect the Network Switch
2. ✅ Find the Ethernet Cable
3. 🚧 Connect the Cable (Enhanced UI in development)

**Completed Systems:**
- Player movement and interaction (raycast-based)
- Network Switch interactable object
- Interaction UI feedback system
- Mission objective manager
- Cable visual representation (LineRenderer)

**In Development:**
- Cable Connection UI Panel (port selection interface)
- Enhanced cable connection flow with visual feedback

---

## Architecture

### Scene Structure

```
Mission_01 (Main Game Scene)
├── Environment
│   ├── Floor, Walls
│   └── Rooms (Server, BreakRoom, Reception, Storage, Meeting)
├── Gameplay Objects
│   ├── NetworkSwitch
│   └── EthernetCable
├── Player
├── HUDCanvas
│   ├── Crosshair
│   ├── InteractionPanel
│   ├── ObjectivePanel
│   └── CableConnectionPanel (NEW)
└── MissionObjectiveManager
```

### Script Structure

```
Assets/Scripts/
├── Player/
│   └── PlayerInteract.cs
│
├── Interaction/
│   ├── Interactable.cs
│   └── InteractionUI.cs
│
└── Missions/
    ├── MissionObjectiveManager.cs
    ├── NetworkSwitchInteractable.cs
    └── CableConnectionVisual.cs
```

---

## Key Systems

### PlayerInteract.cs
- **Purpose:** Handles player interaction with world objects
- **Method:** Raycast from player camera
- **Input:** E key to interact with objects in range
- **Dependency:** Interactable objects must have the Interactable component

### Interactable.cs
- **Purpose:** Base class for all interactable world objects
- **Key Methods:**
  - `Interact()` — Called when player presses E
  - `OnInteractEnter()` — Called when player looks at object
  - `OnInteractExit()` — Called when player stops looking at object

### InteractionUI.cs
- **Purpose:** Displays interaction prompts
- **Display Format:** `[E] Interact` or similar feedback
- **Dependency:** Listens to PlayerInteract for look events

### MissionObjectiveManager.cs
- **Purpose:** Manages Mission 01 objectives
- **Responsibilities:**
  - Track objective completion state
  - Display objectives on HUD
  - Update objective status dynamically
- **Current Objectives:**
  1. Inspect Network Switch
  2. Find Ethernet Cable
  3. Connect Cable

### NetworkSwitchInteractable.cs
- **Purpose:** Handles Network Switch interaction
- **Current Flow:**
  - Player presses E on switch
  - Currently initiates cable connection
  - **Planned:** Open CableConnectionPanel UI instead

### CableConnectionVisual.cs
- **Purpose:** Manages cable visual representation
- **Components:**
  - LineRenderer (draws cable in 3D world)
  - CableConnectionPoint (start position)
  - CableEndPoint (end position)
- **Key Methods:**
  - `ShowCable()` — Makes cable visible
  - `HideCable()` — Hides cable
  - `ConnectCable()` — Animates/displays connection

---

## Next Steps

### Phase 1: Cable Connection UI
- [ ] Create CableConnectionPanel prefab in HUDCanvas
- [ ] Design network switch port display (6-10 ports)
- [ ] Implement RJ45 connector visual representation
- [ ] Create port detection system
- [ ] Implement port selection interaction

### Phase 2: Visual Polish
- [ ] Add cable dragging animation
- [ ] Add port highlighting on hover
- [ ] Add success/failure feedback
- [ ] Sound effects and visual feedback

### Phase 3: Future Missions
- [ ] Mission 02 design
- [ ] Additional networking concepts
- [ ] Increased complexity and interactivity

---

## Development Guidelines

See `DEVELOPMENT.md` for detailed development rules and best practices.

### Key Principles
1. **Inspect before modifying** — Always review existing code first
2. **Preserve working systems** — Don't rewrite functioning code
3. **Incremental changes** — Make small, testable modifications
4. **Communication** — Plan changes before implementation
5. **Documentation** — Keep code and systems documented

---

## Setup Instructions

### Prerequisites
- Unity 6 (or compatible version)
- Visual Studio 2022 (or later) for C# scripting
- Git

### Getting Started
1. Clone this repository:
   ```bash
   git clone https://github.com/Stan1law/NetworkShift.git
   ```

2. Open the project in Unity:
   - File → Open Project
   - Select the `NetworkShift` folder

3. Load Mission 01 scene:
   - Assets/Scenes/Mission_01.unity

4. Play the game:
   - Press Play in the Unity Editor
   - Use WASD to move
   - Use mouse to look around
   - Press E to interact with objects

---

## Project Structure

```
NetworkShift/
├── Assets/
│   ├── Scripts/
│   │   ├── Player/
│   │   ├── Interaction/
│   │   └── Missions/
│   ├── Scenes/
│   ├── Prefabs/
│   ├── Materials/
│   └── ...
├── ProjectSettings/
├── README.md
├── DEVELOPMENT.md
├── .gitignore
└── ...
```

---

## Contributing

This project is maintained by Stan1law as an educational simulation.

For development contribution guidelines, see `DEVELOPMENT.md`.

---

## License

(To be decided)

---

## Contact

For questions or suggestions about Network Shift, please open an issue on GitHub.
