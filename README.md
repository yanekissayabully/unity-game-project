# REWIND HEIST

> *One thief. One vault. Every time you rewind, your past self keeps working for you.*

A solo 2D pixel-art puzzle-platformer for PC, made in Unity for the **Unity Game Engine 1 (Fall 2026)** course project.

| | |
|---|---|
| **Genre** | 2D puzzle-platformer, time-loop heist |
| **Platform** | PC (Windows build) |
| **Engine** | Unity 6000.6.1f1, 2D |
| **Team** | Solo project |
| **Session length** | 1–4 minutes per level, 6 levels |

---

## 1. Concept pitch (3-minute version)

**One sentence:** You are a thief who can rewind time by 30 seconds at most, but your previous run does not disappear, it becomes a clone that repeats your exact moves, so you cooperate with yourself to crack the vault.

**Hook.** A heist usually needs a crew: one person on the switch, one holding the door, one stealing the loot. In Rewind Heist *you are the whole crew*. You play a run, press rewind, and your first run becomes a "ghost" that does exactly what you did. Then you play again, alongside it, and then again with two ghosts, three ghosts.

**Example puzzle.** The vault door only stays open while a pressure plate is held down, and the plate is far from the door. Run 1: stand on the plate and wait. Rewind. Run 2: your ghost holds the plate, you walk through the open door, grab the gem and leave.

**Why it works solo:**
- The game has **one core idea** (record your inputs, replay them as clones) and every level is a new combination of the same few parts: buttons, doors, plates, guards, lasers.
- Pixel-art style with small sprites, so all art is simple and consistent.
- Clones are the same character recoloured, so no extra animation work.

---

## 2. Core gameplay loop

```
Start a loop (clones from earlier loops replay from the start)
      │
      ▼
Play your run: move, jump, press buttons, grab the gem
      │
      ├── Caught / ran out of time ──► the loop restarts (recorded clones stay)
      │
      ├── Press R ──► this run is saved as a new clone, a new loop begins
      │
      └── Gem taken and exit reached ──► level complete, stars awarded
```

---

## 3. Mechanics

### 3.1 Controls
| Action | Keys |
|---|---|
| Move | `A` / `D` or `←` / `→` |
| Jump (hold for higher jump) | `Space` |
| **Rewind: save this run as a clone and start a new loop** | `R` |
| Restart the whole level (delete all clones) | `T` |
| Pause | `Esc` |

### 3.2 The rewind system
- The game records the player's **inputs on every physics step** (move direction, jump pressed, jump held).
- When the player presses `R` (or the loop timer of 30 seconds ends), the recording is given to a new **clone**. Everyone returns to the start and the loop begins again.
- A clone replays the recorded inputs exactly, so it jumps and walks the same way the player did.
- Clones can press buttons, stand on plates, collect the gem and trigger lasers, just like the player.
- The number of clones is limited (2 in the first levels, 3 later), so the player has to plan each run.

### 3.3 Level objects
| Object | Behaviour |
|---|---|
| **Button** | Pressed for a short time, toggles a door or laser. |
| **Pressure plate** | Active only while somebody (player or clone) stands on it. |
| **Door** | Opens when its button or plate is active. |
| **Laser** | Kills the one who touches it (loop restarts). Can be switched off by a button. |
| **Guard** | Patrols a platform and has a vision zone. If it sees the player or any clone, the loop fails. |
| **Gem** | Must be collected by the player or any clone in the same loop in order to finish. |
| **Exit** | Opens the level end, but only for the live player. |

### 3.4 Win / lose
- **Win:** the gem is collected and the live player reaches the exit.
- **Lose (loop fails):** the player or a clone touches a laser or is seen by a guard, or time runs out. The loop restarts instantly, clones stay.

---

## 4. Progression and saved data

**Levels.** 6 levels with a slow ramp of difficulty. Each level unlocks the next.

| # | Name | New element |
|---|---|---|
| 1 | Warm-up | Move, jump, gem, exit (tutorial) |
| 2 | Two of Me | Button + door, first clone |
| 3 | Hold the Line | Pressure plate (clone has to stay on it) |
| 4 | Red Beams | Lasers that a button switches off |
| 5 | Night Shift | Guard with vision zone |
| 6 | The Vault | Everything together, 3 clones needed |

**Skills and rewards:**
- **Stars (1–3)** per level, for using few clones and finishing fast.
- Stars unlock gadgets: **Double Jump** (a new movement skill) and **+1 clone slot**.
- Levels get harder over time as new objects appear.

**Saved between sessions (JSON file in `Application.persistentDataPath`):**
- unlocked levels, stars and best time per level
- unlocked gadgets
- all settings (see below)

---

## 5. UI, menus and settings

**Main menu:** Play / Continue, Level select, Settings, Quit.

**In-game HUD:** loop timer, number of clones used / available, gem status icon, level name.

**Pause menu:** Resume, Restart level, Settings, Back to menu.

**Settings (saved between sessions):**
- Master / Music / SFX volume
- Fullscreen on/off and resolution
- Screen shake on/off
- Clone transparency (so the player can adjust visibility)

The UI uses `Canvas Scaler → Scale With Screen Size` and anchors, so it adapts to different resolutions and aspect ratios.

---

## 6. Visual and audio style

- **Style:** pixel art with a limited palette. Dark purple walls, cyan thief, violet semi-transparent clones, red guards and lasers, gold gem, green exit. Sprites are 16×16 (doors 16×32), imported with Point filter and 16 pixels per unit.
- **Rewind effect:** short screen flash and a "tape" sound when pressing `R`.
- **Clone look:** same sprite as the player, recoloured and slightly transparent, so the player always knows who is who.
- **Audio:** chiptune music loop, jump, button click, door, laser, rewind and fail sounds. Sounds will be generated or taken from free CC0 libraries (credits in `CREDITS.md`).

---

## 7. Technical plan

**Controls:** Unity Input Manager (`Horizontal`, `Jump` axes) plus `R`, `T` and `Esc` keys.

**Key scripts**

| Script | Responsibility |
|---|---|
| `InputFrame` | A small struct with one physics step of input (move, jump pressed, jump held) |
| `PlayerInputReader` | Reads keyboard and produces an `InputFrame` for each physics step |
| `PlayerMotor` | Movement and jumping with a `Rigidbody2D`. Shared by the player and the clones |
| `LivePlayer` | Reads input, moves the motor and records every `InputFrame` |
| `CloneReplayer` | Replays a recorded list of `InputFrame` through its own motor |
| `TimeLoopManager` | Starts loops, spawns clones, resets the level, limits clones, loop timer |
| `Button`, `PressurePlate`, `Door`, `Laser` | Level logic that reacts to the player or clones and resets with each loop |
| `Guard` | Patrol and vision check |
| `LevelManager` | Gem and exit logic, stars, level completion |
| `SaveSystem` | JSON load/save of progress and settings |
| `SettingsManager` | Applies volume, fullscreen, etc. |
| `MainMenu` / `HUD` / `PauseMenu` | UI |

**Important design choice:** the player and the clones use the **same `PlayerMotor`**. The only difference is where the input comes from (keyboard or a recording). This keeps the clones accurate and the code short.

**Physics layers:** `Player`, `Clone`, `Ground`. Player and Clone layers ignore each other so they do not push each other around.

**Planned folder structure**

```
Assets/
├── _Project/
│   ├── Scripts/
│   │   ├── Player/        (InputFrame, PlayerInputReader, PlayerMotor, LivePlayer)
│   │   ├── TimeLoop/      (CloneReplayer, TimeLoopManager)
│   │   ├── LevelObjects/  (Button, PressurePlate, Door, Laser, Guard)
│   │   ├── Systems/       (LevelManager, SaveSystem, SettingsManager)
│   │   └── UI/
│   ├── Prefabs/
│   ├── Scenes/            (MainMenu, Level_01 … Level_06)
│   ├── Art/
│   │   └── Sprites/
│   ├── Audio/
│   └── Settings/
└── ThirdParty/
```

**Code style:** C# conventions throughout: `PascalCase` for classes and methods, `_camelCase` for private fields, one class per file, namespace `RewindHeist`, no copy-pasted code left in a foreign style.

**Version control:** `main` is always buildable. Work happens in short feature branches (`feature/time-loop`, `feature/save-system`), small commits with clear messages.

---

## 8. Development plan (by milestone)

| Milestone | Goal | Done when |
|---|---|---|
| **1. Concept** | Repo, README, pitch | Unity project on GitHub, this README, 3-min pitch ready |
| **2. Core prototype** | Player movement + rewind and clones in a grey-box level | Move, jump, press `R`, clone repeats the run exactly |
| **3. Vertical slice** | Levels 1–3, buttons, doors, plates, HUD, main menu | Playable from menu to level end, basic save |
| **4. Content** | Lasers, guards, levels 4–6, gadgets, settings with saving | Full progression and persistent data |
| **5. Final** | Polish, bug fixing, audio, Windows build | Stable build, clean project structure, README with screenshots |

---

## 9. Mapping to the project criteria

| Criterion | How Rewind Heist covers it |
|---|---|
| Source Control | This GitHub repository, Unity project committed |
| Concept pitch | Section 1, 3-minute pitch |
| Inputs | A/D + Space + R, simple, jump buffer and coyote time for responsive feel |
| Mechanics | Input recording, clones, buttons, plates, doors, lasers, guards |
| Progress | 6 levels with rising difficulty, stars, unlockable gadgets, saved progress |
| Visuals | One consistent pixel-art style and palette |
| UI | Main menu, HUD, pause menu, scalable canvas |
| Settings | Volume, fullscreen, shake, clone transparency, all saved |
| Content | 6 levels, 5 interactive object types, 2 hazards (laser, guard), gadgets |
| Build | Windows PC build |
| Project structure | Section 7, folders by type |
| Code style | One consistent C# style |
| Bonus | The "cooperate with your past self" mechanic and the input-recording system |

---

## 10. Scope control (if time runs short)

Cut in this order, the game stays complete without them:
1. Guard enemy (lasers are enough for hazards)
2. Double Jump gadget
3. Level 6
4. Clone transparency and screen shake options

**Stretch goals (only if everything above is done):** a level editor with shareable level codes, ghost replay of the best run, time-attack mode.

---

## 11. Credits

Design, programming and pixel art: solo. Third-party audio and assets will be listed in `CREDITS.md`.
