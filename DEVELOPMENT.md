# Development Guidelines

## Overview

This document outlines the development process, rules, and best practices for Network Shift. Follow these guidelines to maintain code quality, preserve existing systems, and ensure smooth collaboration.

---

## Core Development Rules

### 1. **INSPECT BEFORE MODIFYING**
Before making any changes to a script or system:
- Read the existing code thoroughly
- Understand how it connects to other systems
- Check for dependencies and side effects
- Ask yourself: "What will break if I change this?"

### 2. **PRESERVE WORKING SYSTEMS**
- Do NOT rewrite functioning code unnecessarily
- Do NOT create duplicate systems
- Do NOT remove existing functionality without explicit approval
- When fixing bugs, make minimal, targeted changes

### 3. **INCREMENTAL CHANGES**
- Make small, testable modifications
- Test after each change
- Commit frequently with clear messages
- If something breaks, you can roll back easily

### 4. **COMMUNICATION FIRST**
- **Before major changes:** Tell the developer which files you plan to modify
- **Explain why:** What problem does this solve?
- **Show the plan:** What will the change look like?
- **Get approval:** Wait for confirmation before implementing

### 5. **PRESERVE INSPECTOR REFERENCES**
- If scripts have public fields set in the Inspector, preserve them
- Do not remove or rename fields without updating all references
- If you must refactor, provide a migration path

### 6. **DO NOT MODIFY UNRELATED SCRIPTS**
- If you're working on the cable UI, don't touch PlayerInteract
- If you're working on objectives, don't touch InteractionUI
- Keep changes focused and contained

---

## Current Mission 01 Connection Flow

### Current Implementation
```
Player presses E on NetworkSwitch
         ↓
NetworkSwitchInteractable.Interact() called
         ↓
CableConnectionVisual.ConnectCable() called
         ↓
LineRenderer shows cable in world
         ↓
Mission objective updates
         ↓
COMPLETE
```

### Planned Enhancement
```
Player presses E on NetworkSwitch
         ↓
CableConnectionPanel.SetActive(true)
         ↓
Cable Connection UI opens
         ↓
Player sees Network Switch ports (1-10)
         ↓
Player selects/clicks a port
         ↓
Port detection validates selection
         ↓
CableConnectionPanel.SetActive(false)
         ↓
CableConnectionVisual.ShowCable() called
         ↓
LineRenderer shows cable in world
         ↓
Mission objective "Connect Cable" completes
```

### Scripts Involved in Connection Flow
1. **PlayerInteract.cs** — Detects E key press, calls Interact()
2. **NetworkSwitchInteractable.cs** — Responds to E key, will open UI
3. **CableConnectionPanel.cs** — NEW: Manages connection UI
4. **CableConnectionVisual.cs** — Shows cable after connection
5. **MissionObjectiveManager.cs** — Updates objective status

---

## Script Dependency Map

```
PlayerInteract.cs
    ↓
Interactable.cs (base class)
    ↓
NetworkSwitchInteractable.cs (extends Interactable)
    ↓
CableConnectionVisual.cs
CableConnectionPanel.cs (planned)
    ↓
MissionObjectiveManager.cs
```

### Key Dependencies
- **NetworkSwitchInteractable** depends on **Interactable** base class
- **NetworkSwitchInteractable** will call **CableConnectionPanel.SetActive()**
- **CableConnectionPanel** will call **CableConnectionVisual.ShowCable()** on success
- **MissionObjectiveManager** listens for completion event

---

## Development Process

### When Starting a New Feature

1. **Read this document** — Ensure you understand the guidelines
2. **Inspect existing code** — Look at the scripts you'll be modifying
3. **Plan your changes** — Write out which files will change and why
4. **Get approval** — Show the plan to the developer
5. **Implement incrementally** — Make small changes, test often
6. **Commit with clear messages** — Help future developers understand your work
7. **Update documentation** — Keep README and this file in sync

### Example: Adding Cable Connection UI

**Step 1 — Inspection**
```
Read: NetworkSwitchInteractable.cs
Read: CableConnectionVisual.cs
Understand: How does the current connection work?
```

**Step 2 — Planning**
```
Files to modify:
  - NetworkSwitchInteractable.cs (call UI instead of connecting immediately)
  
Files to create:
  - CableConnectionPanel.cs (new UI controller)
  
No changes to:
  - PlayerInteract.cs
  - Interactable.cs
  - MissionObjectiveManager.cs
  - CableConnectionVisual.cs
```

**Step 3 — Approval**
```
"I plan to modify NetworkSwitchInteractable to open the cable UI panel
instead of connecting the cable immediately. I'll create a new
CableConnectionPanel.cs script to manage the UI state and port selection.
Ready to proceed?"
```

**Step 4 — Implementation**
```
Create CableConnectionPanel.cs
Modify NetworkSwitchInteractable.cs
Test in Unity
Commit with clear message
```

---

## Code Style Guidelines

### Naming Conventions
- **Classes:** PascalCase (e.g., `CableConnectionPanel`)
- **Methods:** PascalCase (e.g., `OnPortSelected()`)
- **Variables:** camelCase (e.g., `selectedPort`)
- **Constants:** UPPER_SNAKE_CASE (e.g., `PORT_COUNT = 10`)

### C# Script Structure
```csharp
using System;
using UnityEngine;

/// <summary>
/// Brief description of what this script does.
/// </summary>
public class ClassName : MonoBehaviour
{
    // Public fields (Inspector-visible)
    public int publicField;
    
    // Serialized private fields (Inspector-visible but not public)
    [SerializeField] private int privateField;
    
    // Private fields (not visible in Inspector)
    private int internalField;
    
    // Constants
    private const int CONSTANT_VALUE = 10;
    
    // Properties
    public bool IsActive { get; set; }
    
    // Unity lifecycle methods (in order)
    private void Awake() { }
    private void Start() { }
    private void Update() { }
    private void LateUpdate() { }
    
    // Public methods
    public void PublicMethod() { }
    
    // Private methods
    private void PrivateMethod() { }
    
    // Event handlers (naming: On + EventName)
    private void OnPortSelected(int portNumber) { }
}
```

### Comments and Documentation
```csharp
/// <summary>
/// Handles player interaction with network switch.
/// Responds to E key press and initiates cable connection UI.
/// </summary>
public class NetworkSwitchInteractable : Interactable
{
    /// <summary>
    /// Opens the cable connection panel when player interacts.
    /// </summary>
    public override void Interact()
    {
        // Implementation here
    }
}
```

---

## Testing Checklist

Before committing changes, verify:

- [ ] Code compiles without errors
- [ ] No console warnings (unless necessary)
- [ ] Feature works as intended in Play mode
- [ ] Existing functionality still works
- [ ] No unintended changes to other systems
- [ ] Inspector references are preserved
- [ ] Commit message clearly describes the change

---

## Git Workflow

### Commit Message Format
```
[Feature/Fix/Refactor] Brief description

Detailed explanation of what changed and why.
- Bullet point for specific changes
- Reference which files were modified
```

### Example Commits
```
[Feature] Add cable connection UI panel

Creates CableConnectionPanel script and UI hierarchy.
- New CableConnectionPanel.cs script
- Manages port selection and cable connection flow
- Integrates with existing NetworkSwitchInteractable
- Preserves CableConnectionVisual for final cable display

Modified: NetworkSwitchInteractable.cs (now opens UI)
Created: Assets/Scripts/UI/CableConnectionPanel.cs
```

---

## When Things Go Wrong

### If You Break Something
1. **Stop** — Do not make more changes
2. **Identify** — What broke and why?
3. **Revert** — Use `git revert` or `git checkout` to restore
4. **Understand** — Read the broken code before trying again
5. **Plan** — Think through the dependency chain
6. **Report** — Tell the developer what happened

### If You're Unsure
**Ask before acting.** It's better to ask a question than to break working code.

### Common Issues
- **Removed a field but forgot to update Inspector references** → Use Find & Replace in Visual Studio
- **Changed method signature but didn't update callers** → Use Find All References
- **Broke InteractionUI by modifying Interactable** → Check all classes that extend Interactable

---

## Future Development Phases

### Phase 1: Cable Connection UI ✅ Next
- [ ] Create CableConnectionPanel
- [ ] Design port selection system
- [ ] Implement visual feedback
- [ ] Test full connection flow

### Phase 2: Visual Polish
- [ ] Add cable dragging animation
- [ ] Enhanced port visuals
- [ ] Sound effects
- [ ] Success/failure animations

### Phase 3: Additional Missions
- [ ] Mission 02 design and implementation
- [ ] New networking concepts
- [ ] Expanded complexity

---

## Questions?

If anything in this document is unclear, ask before proceeding.
