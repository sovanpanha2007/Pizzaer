# Delivery scripts — for Dina

## What they are

- `DeliveryManager` (on the `Core_GameManager` prefab) is the "boss" of the game.
  It keeps the 5 orders, the money, and whether the player has a pizza.
  When the player presses **E** in the shop or a house, it decides what happens.
- `UIManager` (on `UI_Canvas`) shows it on screen: money, task text, "Press E", dialogue, and the win panel with two buttons.

You do not drag anything into `DeliveryManager`. It finds the UI, the marker and the zones by itself when Play starts.

Big picture: [ARCHITECTURE.md](ARCHITECTURE.md).

## Your task (Dina)

Do this in `Town` after the zone, marker and `UI_Canvas` prefabs are merged.

1. Drop `Prefabs/Core/Core_GameManager` into `Town` under `--- GAMEPLAY ---`. Only one.
2. In `DeliveryManager`, replace the 2 test orders with the 5 real ones (text from `docs/STORY.md`).
   Each order: `houseNumber` (1–5), `pizzaName`, `customerName`, `customerLine`, `reward`.
3. Put one `Zone_Shop` at the shop and one `Zone_House` at each house.
   Set `houseNumber` on each `Zone_House` to match its order.
4. Put `Marker_Target` and `UI_Canvas` in the scene.
5. Keep everything **active** and set up **before** you press Play.

## Test it

1. Press Play in `Town`.
2. Walk to the shop and press **E**. Walk to the house in the order and press **E**. Repeat for all 5.
3. Check on screen: money goes up, "Press E" shows inside a zone, dialogue hides after 3 seconds, and the win panel shows after the 5th delivery.
4. Check: a wrong house gives "Sorry, I didn't order pizza", and a second shop visit with a pizza gives "You already have a pizza".
5. On the win panel, **Play again** reloads `Town` and **Main Menu** loads `MainMenu`.
6. The Console must show **0 red errors**.

If you see a red `[Delivery]` error:

| Message | Fix |
|---|---|
| `Missing shop zone, UI_Canvas or Marker_Target` | Add the missing object and make it active |
| `The orders list is empty` | Fill the orders on `Core_GameManager` |
| `House #N is not in the scene` | No `Zone_House` has that number. Fix `houseNumber` |

## Not ready yet

- `Zone_Shop`, `Zone_House`, `Marker_Target` (Viset)
- The real `UI_Canvas` (Soth)
- Final order text in `docs/STORY.md` (Soth)

Ask Panha before you change a method name in `DeliveryManager` or `UIManager`.
