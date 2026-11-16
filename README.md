# 🍕 Pizzaer

> A 3D third-person pizza delivery adventure game with light casual RPG elements, developed in **Unity 6 (6000.3.8f1)** using the **Universal Render Pipeline (URP)**.

---

## 📑 Table of Contents
1. [GamePlay](#-gameplay)
2. [Scene](#-scene)
3. [Scene Flow](#-scene-flow)
4. [Asset Use](#-asset-use)
5. [Role](#-role)
6. [Instruction To Play](#-instruction-to-play)

---

## 🎮 GamePlay

### 1. Synopsis & Story
**Grandpa Lim's Pizza Shop** is celebrating its grand opening today in a bustling suburban town! Unfortunately, disaster strikes on day one: the hired delivery boy suddenly quits without notice. 

As the only remaining helper, you must step in, take orders from **Chef Lim**, and navigate through town to deliver 5 piping-hot pizzas to waiting neighborhood customers. Save the shop's reputation, earn tips, and complete the opening day triumphantly!

---

### 2. Core Game Loop
The gameplay loop is built around focused delivery objectives guided by an intuitive waypoint system:

```
       ┌────────────────────────────────────────────────────────┐
       ▼                                                        │
[1] Head to Grandpa Lim's Pizza Shop                            │
       │                                                        │
       ▼ (Step into zone & press [E])                           │
[2] Receive Order from Chef Lim + Pick Up Pizza                 │
       │  (Target marker points to customer's house)            │
       ▼                                                        │
[3] Navigate Town & Deliver to Target House                     │
       │  (Step into house zone & press [E])                    │
       ▼                                                        │
[4] Customer Dialogue & Collect Cash Tip                        │
       │                                                        │
       ├──── More deliveries remaining? ────────────────────────┘
       │
       ▼ After completing 5th delivery
[5] Victory Screen (Shop Saved! Total Cash: $60)
```

---

### 3. Core Mechanics & Rules
- **Single Pizza Capacity:** The player can only carry **one pizza at a time**. Attempting to pick up another pizza while already carrying one prompts Chef Lim to say: *"You already have a pizza. Go deliver it!"*
- **Dynamic Waypoint Marker:** A glowing, rotating, and bobbing 3D beacon beacon (`TargetMarker`) hovers above your immediate objective—first pointing to the Pizza Shop, then to the designated customer's house, and back again.
- **Wrong House Safeguard:** If the player visits an incorrect house or interacts without the pizza, the homeowner politely replies: *"Sorry, I didn't order pizza."* (no financial penalty, error sound effect plays).
- **Interactive Proximity Triggers:** Approaching interactive locations automatically shows a prompt (*"Press E to Interact"*).
- **Typewriter Dialogue & Timed UI:** Dialogue appears with a typewriter animation and auto-hides after 3 seconds (or can be dismissed).

---

### 4. The 5 Delivery Orders

| Order # | Customer Name | Destination | Pizza Type | Customer Dialogue | Reward |
|:-------:|:--------------|:-----------:|:-----------|:------------------|:------:|
| **#1** | **Mrs. Dara** | House #1 | 🧀 Cheese Pizza | *"Just in time for lunch. Thank you!"* | **$10** |
| **#2** | **Mr. Sok** | House #4 | 🍕 Pepperoni Pizza | *"It smells great! Here is your money."* | **$10** |
| **#3** | **Nita** | House #2 | 🍍 Hawaiian Pizza | *"Pineapple on pizza is the best!"* | **$10** |
| **#4** | **Uncle Rith** | House #5 | 🥦 Veggie Pizza | *"Fresh and hot. Good job, kid!"* | **$15** |
| **#5** | **Grandma Srey** | House #3 | 🦐 Seafood Pizza | *"You saved the shop's first day!"* | **$15** |
| **TOTAL** | — | — | — | **All 5 orders delivered successfully** | **$60** |

---

## 🗺️ Scene

The project organizes environments, menus, and development sandboxes under `Assets/_Pizzaer/Scenes/`:

### 1. Production Scenes (Build Scenes)
These are configured in Unity's Build Settings (`EditorBuildSettings.asset`):

| Scene File | Build Index | Description |
|:---|:---:|:---|
| **`GameMenu.unity`** | `0` | **Main Menu Scene**<br>Contains the start menu UI: Title Panel (Play, Quit), Story Introduction Panel (Narrative exposition, Start Game button), background music controller (`MainMenuAudio`), and cursor management. |
| **`Town.unity`** | `1` | **Main Gameplay Level**<br>The fully integrated open level featuring roads, intersections, pedestrian crossings, garden/park, fences, perimeter barriers, Grandpa Lim's Pizza Shop, and 5 numbered delivery houses. Houses core managers (`Core_GameManager`, `Core_Audio`), the third-person player character (`Player-with-Cam`), interactive zones (`Zone_Shop`, `Zone_House`), navigation beacon (`Marker_Target`), and game HUD (`UI_Canvas`). |

### 2. Assembly & Sandbox Scenes
| Scene File | Purpose |
|:---|:---|
| **`Main.unity`** | Full level staging and environment assembly scene used during development to calibrate lighting, props, and city layout. |
| **`Sandbox/Sandbox_Panha.unity`** | Lead programmer sandbox for verifying delivery order logic, `DeliveryManager` state machine, and HUD communication in isolation. |
| **`Sandbox/Sandbox_Soth.unity`** | UI and Audio developer sandbox for testing dialogue typewriter effects, UI canvases, button callbacks, and sound transitions. |
| **`Sandbox/Sandbox_Viseth.unity`** | Gameplay developer sandbox for tuning third-person character locomotion, jump feel, Cinemachine camera tracking, and trigger colliders. |

---

## 🔄 Scene Flow

```mermaid
stateDiagram-v2
    [*] --> GameMenu: Launch Pizzaner.exe / Play in Editor

    state GameMenu {
        [*] --> TitlePanel
        TitlePanel --> StoryPanel: Click [Play]
        StoryPanel --> TitlePanel: Back / Cancel
        TitlePanel --> [*]: Click [Quit]
        StoryPanel --> Town: Click [Start]
    }

    state Town {
        [*] --> Initializing: Load Town.unity
        Initializing --> Exploring: DeliveryManager Ready\nMarker points to Shop
        
        state DeliveryCycle {
            Exploring --> InShopZone: Enter Shop Zone
            InShopZone --> CarriedPizza: Press [E] (Chef Lim dialogue & Pizza acquired)
            CarriedPizza --> InHouseZone: Follow Marker to target House #
            InHouseZone --> Delivered: Press [E] (Customer Dialogue & Earn Tip)
            Delivered --> Exploring: Marker points to Shop (Next Order)
        }

        Delivered --> WinScreen: Order 5 Completed ($60 Total)
        
        state WinScreen {
            [*] --> VictoryPrompt: Freeze timeScale\nShow Win Panel
            VictoryPrompt --> Town: Click [Play Again] (Reload Town)
            VictoryPrompt --> GameMenu: Click [Main Menu] (Return to Menu)
        }
    }
```

### Flow Breakdown:
1. **Application Launch:** The player enters `GameMenu`. Background menu music (`MUS_Menu.mp3`) loops.
2. **Title Screen:** The Title Panel displays the title with **Play** and **Quit** buttons.
3. **Story Exposition:** Clicking **Play** transitions to the **Story Panel**, revealing the narrative of Chef Lim and Grandpa Lim's shop with typewriter text animation.
4. **Transition to Gameplay:** Clicking **Start** invokes `SceneManager.LoadScene("Town")`.
5. **Gameplay Loop in Town:**
   - Player controls the delivery worker in third person.
   - Visits Pizza Shop -> receives pizza and house target -> visits designated customer house -> delivers pizza and earns reward -> returns to shop.
6. **Victory Screen:** Upon finishing the 5th order, the delivery manager triggers the win sound (`SFX_Win.mp3`), stops the navigation marker, freezes the world timescale (`Time.timeScale = 0f`), releases mouse lock, and displays the **Win Panel**.
7. **Replayability:** The player can select:
   - **Play Again:** Restores timescale to `1.0` and restarts `Town.unity`.
   - **Main Menu:** Restores timescale to `1.0` and redirects to `GameMenu.unity`.

---

## 📦 Asset Use

All external assets are carefully attributed and comply with open licenses (**CC0 / Public Domain** or **CC-BY 3.0**):

| Category | Asset Pack / Item | Author / Source | License | Location in Project |
|:---|:---|:---|:---:|:---|
| **Character** | **Food Worker** | J-Toastie ([Poly Pizza](https://poly.pizza/m/6cjgeTS8Pl)) | CC-BY 3.0 | `Assets/_Pizzaer/Models/Player/Food Worker by J-Toastie - 6cjgeTS8Pl/` |
| **Character** | **Adventurer** | Quaternius | CC0 | `Assets/_Pizzaer/Models/Player/Adventurer by Quaternius - 5EGWBMpuXq/` |
| **Navigation** | **Map Marker 📍** | Chris Ross ([Poly Pizza](https://poly.pizza/m/6ij4nru_NPh)) | CC-BY 3.0 | `Assets/_Pizzaer/Models/Map Marker 📍 by Chris Ross - 6ij4nru_NPh/` |
| **Environment** | **Kenney City Kit (Roads)** | [Kenney.nl](https://kenney.nl/assets/city-kit-roads) | CC0 | `Assets/_Pizzaer/Models/kenney_city-kit-roads/` |
| **Environment** | **Kenney City Kit (Suburban)** | [Kenney.nl](https://kenney.nl/assets/city-kit-suburban) | CC0 | `Assets/_Pizzaer/Models/kenney_city-kit-suburban_20/` |
| **Environment** | **Kenney City Kit (Commercial)** | [Kenney.nl](https://kenney.nl/assets/city-kit-commercial) | CC0 | `Assets/_Pizzaer/Models/kenney_city-kit-commercial_2 1.1/` |
| **Environment** | **Kenney City Kit (Industrial)** | [Kenney.nl](https://kenney.nl/assets/city-kit-industrial) | CC0 | `Assets/_Pizzaer/Models/kenney_city-kit-industrial_2 1.0/` |
| **Environment** | **Kenney Nature Kit** | [Kenney / OpenGameArt](https://opengameart.org/content/nature-kit) | CC0 | `Assets/_Pizzaer/Models/Kenney_Nauture/` |
| **Props** | **Kenney Food Kit** | [Kenney.nl](https://kenney.nl/assets/food-kit) | CC0 | `Assets/_Pizzaer/Models/Kenney_Food/` |
| **Props** | **Props (Desk, Traffic Barrier, Street T, Town Center)** | Quaternius | CC0 | `Assets/_Pizzaer/Models/` |
| **Props** | **Large Table** | Voxel_dev | CC0 | `Assets/_Pizzaer/Models/Large Table by Voxel_dev - gJ0mTnQw82/` |
| **UI** | **Kenney UI Pack & UI Adventure** | [Kenney.nl](https://kenney.nl/assets/ui-pack) | CC0 | `Assets/_Pizzaer/Art/UI/` |
| **Font** | **LiberationSans SDF** | TextMeshPro (Unity Technologies) | Included | `Assets/TextMesh Pro/` |
| **SFX** | **Interface Sounds & RPG Audio** (`SFX_Pickup`, `SFX_Deliver`, `SFX_Wrong`, `SFX_Click`) | [Kenney.nl](https://kenney.nl/) | CC0 | `Assets/_Pizzaer/Audio/SFX/` |
| **SFX** | **Music Jingles** (`SFX_Win`) | [Kenney.nl](https://kenney.nl/assets/music-jingles) | CC0 | `Assets/_Pizzaer/Audio/SFX/SFX_Win.mp3` |
| **Music** | **Two Simple Game Music Loops** (`MUS_Menu.mp3`, `MUS_Town.mp3`) | qubodup ([OpenGameArt](https://opengameart.org/node/174263)) | CC0 | `Assets/_Pizzaer/Audio/Music/` |

---

## 👥 Role

The project was developed collaboratively with clear separation of ownership across architecture, gameplay engineering, level design, UI, and audio:

| Team Member | GitHub Account | Primary Role | Core Responsibilities & Deliverables | Key Files Owned |
|:---|:---|:---|:---|:---|
| **Dina** | [`dina-seng`](https://github.com/dina-seng) | **Level Builder, Integrator & Build Engineer** | • Designed and assembled the 3D town level (`Town.unity`, `Main.unity`).<br>• Configured environment prefabs (`Env_`, barriers, garden, roads, houses, shops).<br>• Supervised URP pipeline setup, lighting, and colliders.<br>• Managed Git merges, resolved integration conflicts, and produced the standalone Windows release build. | `Scenes/Town.unity`<br>`Scenes/Main.unity`<br>`Prefabs/Environment/*`<br>`Models/`, `Materials/`<br>`install/Pizzaner.exe` |
| **Panha (Sue)** | [`sovanpanha2007`](https://github.com/sovanpanha2007) | **Team Lead & Core Systems Engineer** | • Coordinated agile project milestones, daily standups, and documentation.<br>• Programmed core game architecture: `DeliveryManager.cs` and `DeliveryOrder.cs`.<br>• Programmed HUD and screen controller: `UIManager.cs`.<br>• Integrated order progression, cash tracking, win conditions, and UI notifications. | `Scripts/Core/*`<br>`Scripts/UI/UIManager.cs`<br>`Prefabs/Core/Core_GameManager`<br>`docs/PLAN.md`, `docs/ARCHITECTURE.md` |
| **Viset** | [`lancd`](https://github.com/lancd) | **Gameplay & Mechanics Engineer** | • Implemented 3rd-person movement with jump, sprint, and camera rotation (`PlayerMovement.cs`).<br>• Built spatial interaction trigger zones (`InteractZone.cs`).<br>• Engineered dynamic 3D floating navigational marker (`TargetMarker.cs`).<br>• Programmed pizza pickup and attachment system (`PizzaPickUp.cs`) and `MainMenu.cs`. | `Scripts/Gameplay/*`<br>`Scripts/UI/MainMenu.cs`<br>`Prefabs/Player/*`<br>`Prefabs/Gameplay/*` |
| **Soth** | [`PSoth911`](https://github.com/PSoth911) | **UI/UX Designer, Audio Lead & Narrative Writer** | • Authored the storyline, character dialogue, and order script (`docs/STORY.md`).<br>• Designed UI canvas aesthetics (HUD, prompt boxes, dialogue window, win screen).<br>• Programmed `TypewriterEffect.cs` for immersive dialogue animations.<br>• Curated and integrated all sound effects, music tracks, and `AudioManager.cs`. | `Scenes/GameMenu.unity`<br>`Scripts/Audio/*`<br>`Scripts/UI/TypewriterEffect.cs`<br>`Prefabs/Core/Core_Audio`<br>`Art/UI/`, `Audio/`, `docs/STORY.md` |

---

## 🕹️ Instruction To Play

### 1. System Requirements & Setup
- **Platform:** Windows 10 / 11 (64-bit)
- **Engine Version:** Unity 6.3 (6000.3.8f1) URP (for running from source)

#### Running the Standalone Build:
1. Open the project root folder.
2. Navigate to `install/`.
3. Double-click **`Pizzaner.exe`** to launch immediately.

#### Running in Unity Editor:
1. Open the project in **Unity Hub** using **Unity 6 (6000.3.8f1)**.
2. In the Project window, navigate to `Assets/_Pizzaer/Scenes/`.
3. Open **`GameMenu.unity`** (or `Town.unity`).
4. Click the **Play** button at the top of the Unity Editor.

---

### 2. Controls Scheme

| Key / Input | Action | Description |
|:---:|:---|:---|
| <kbd>W</kbd> <kbd>A</kbd> <kbd>S</kbd> <kbd>D</kbd> | **Movement** | Walk forward, left, backward, and right (direction is relative to the camera view). |
| <kbd>Mouse</kbd> | **Look / Camera Orbit** | Rotate the third-person camera around the delivery character. |
| <kbd>Left Shift</kbd> (Hold) | **Sprint / Run** | Accelerates movement speed from walking ($3\text{ m/s}$) to running ($6\text{ m/s}$). |
| <kbd>Space</kbd> | **Jump** | Jump over curbs, barriers, or obstacles (includes coyote time and jump buffering). |
| <kbd>E</kbd> | **Interact** | Contextual action button when inside a trigger zone:<br>• **At Shop:** Talk to Chef Lim & pick up order/pizza.<br>• **At House:** Deliver pizza to customer & collect payment. |
| <kbd>Left Click</kbd> | **Menu Selection** | Click buttons across menus (Play, Start, Play Again, Quit). |

---

### 3. Step-by-Step Walkthrough to Complete the Game
1. **Start Game:** On the title screen, click **Play**, read Chef Lim's story on the intro card, then click **Start**.
2. **Follow the Marker:** Look for the glowing floating marker (`TargetMarker`) hovering in the town sky. Head straight toward it to reach **Grandpa Lim's Pizza Shop**.
3. **Pick Up Order:** Walk inside the shop's front door zone until the prompt *"Press E"* appears on screen. Press <kbd>E</kbd>. Chef Lim will provide your delivery destination and attach the pizza to your hands.
4. **Deliver to Customer:** The marker will shift directly over the roof of the assigned house (e.g., House #1, House #4, House #2, House #5, House #3). Walk or sprint to the designated house.
5. **Drop Off & Collect Cash:** Step inside the customer's front yard zone and press <kbd>E</kbd>. The resident will thank you, the cash counter will update, and the delivery sound will play.
6. **Return & Repeat:** Return to the Pizza Shop for the next order. Repeat until all **5 deliveries** are completed.
7. **Victory:** After the 5th delivery, celebrate as the **Shop Saved!** victory screen appears with your total earnings of **$60**. Click **Play Again** to restart or **Main Menu** to return to the title screen!

---

### 4. Gameplay Demo Video
A recorded demonstration of the complete gameplay loop is located at:
- `install/demo/PizznerGamePlay.mp4` 

Google Drive Demonstration Video:[ https://drive.google.com/file/d/12qnA9KUoWsw21ieyH_9MScg-a5WfcdJF/view](https://drive.google.com/file/d/12qnA9KUoWsw21ieyH_9MScg-a5WfcdJF/view?usp=sharing)
