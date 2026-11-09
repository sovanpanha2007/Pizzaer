# Delivery scripts — how to use them

For: Dina (Town scene), Viset (zones, marker, audio), Soth (UI).
Owner of the scripts: Panha. Big picture: **[ARCHITECTURE.md](ARCHITECTURE.md)** §4–5.

## 1. What it is

`DeliveryManager` is the "boss" of the game. It keeps the order list, the money, and whether the player has a pizza.
Zones tell it what the player did. It tells the UI, the marker and the audio what to show.
**Messages go one way:** Zones → DeliveryManager → UI / Marker / Audio.

Files (in `Assets/_Pizzaer/Scripts/`):
- `Core/DeliveryOrder.cs` — data for one order (not a component).
- `Core/DeliveryManager.cs` — the logic. Lives on the `Core_GameManager` prefab.

## 2. What each person does

| Person | Do this |
|---|---|
| **Dina** | Drop `Core_GameManager` into `Town` under `--- GAMEPLAY ---`. Fill 5 orders (text from `docs/STORY.md`). Set `houseNumber` on each `Zone_House` to match its order. |
| **Viset** | Zones must call `PlayerEntered`, `PlayerLeft`, `Interact` (see table). `TargetMarker` must have `MoveTo(Transform)` and `Hide()`. |
| **Soth** | `UIManager` must have the 5 methods in the table below. Assign all panels and texts in the Inspector. |

You do **not** need to drag anything into `DeliveryManager`. It finds the UI, marker and zones by itself at `Start`.

## 3. Methods

**You call these on `DeliveryManager.Instance`:**

| Method | Who calls it | When |
|---|---|---|
| `PlayerEntered(InteractZone zone)` | `InteractZone` | Player walks into a zone → shows "Press E" |
| `PlayerLeft(InteractZone zone)` | `InteractZone` | Player walks out → hides "Press E" |
| `Interact(InteractZone zone)` | `InteractZone` | Player presses E inside a zone → all game rules |

**`DeliveryManager` calls these. Keep the names and parameters:**

| Method | Class | Used for |
|---|---|---|
| `SetMoney(int)` | `UIManager` | Money on the HUD |
| `SetOrder(string)` | `UIManager` | Current task text |
| `ShowPrompt(bool)` | `UIManager` | "Press E" on/off |
| `ShowDialogue(string speaker, string line)` | `UIManager` | Chef / customer talk |
| `ShowWin(int totalMoney)` | `UIManager` | After the last order |
| `MoveTo(Transform)` / `Hide()` | `TargetMarker` | Marker above next target / hide at the end |

Until the real UI exists, `UIManager` writes to the Console (`[UI] ...`) when a field is not assigned. No crash.

## 4. Order fields (`DeliveryOrder`)

| Field | Meaning |
|---|---|
| `houseNumber` | Which house gets this pizza. Must match a `Zone_House`. |
| `pizzaName` | Shown in the dialogue and task text |
| `customerName` | Speaker name when delivered |
| `customerLine` | What the customer says when delivered |
| `reward` | Money added on delivery |

Orders are done **in list order**. The 5th delivery ends the game.

## 5. How a delivery works

State inside `DeliveryManager`: `hasPizza`, `currentIndex` (current order), `money`.

1. Start → marker over the shop.
2. E at the shop, `hasPizza` is false → `hasPizza = true`, chef says which house, marker moves to that house.
3. E at the shop again while `hasPizza` → "You already have a pizza."
4. E at the right house with a pizza → money + reward, customer line, next order, marker back to the shop.
5. E at a wrong house (or without a pizza) → "Sorry, I didn't order pizza."
6. After the last order → marker hides, `ShowWin(money)`.

## 6. The scene must have

- Exactly **one** `InteractZone` with type **Shop**.
- One `InteractZone` (type House) for **every** `houseNumber` used in the orders. Numbers must be unique.
- One `UIManager` and one `TargetMarker`.
- Everything **active** in the Hierarchy (`FindFirstObjectByType` skips inactive objects).
- Player root with tag **Player**. Zone BoxCollider with **Is Trigger** on.
- Only **one** `Core_GameManager`.

## 7. Console messages

| Message | Meaning / fix |
|---|---|
| `[Delivery] Missing shop zone, UI_Canvas or Marker_Target in the scene!` | Add the missing object, make sure it is active. The game loop stays off. |
| `[Delivery] The orders list is empty...` | Fill the `orders` list on `Core_GameManager`. |
| `[Delivery] House #N is not in the scene!` | An order uses house N but no `Zone_House` has that number. Fix the number. |
| `[Delivery] More than one shop zone...` | Remove the extra shop zone. |
| `[Delivery] Two houses share number N.` | Give each house a different `houseNumber`. |

## 8. Test it alone (your sandbox)

1. Open your `Sandbox_<Name>` scene.
2. Put in: ground, player (tag Player), `Core_GameManager` with 2 test orders, a `UIManager` object, a `TargetMarker` object, 2 cubes with Trigger + `InteractZone` (one Shop, one House #1).
3. Press Play. Visit the shop, then house #1, then the shop again, then a wrong house.
4. The Console must show 0 errors.

## 9. Rules

- Keep the public method names above. If you need a new one or a change, ask Panha first.
- Do not put game rules in the UI or in zones. Rules live only in `DeliveryManager`.
