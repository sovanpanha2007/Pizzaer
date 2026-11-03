using System.Collections.Generic;
using UnityEngine;

// STUB (A5): fields and public methods only. Panha writes the real logic in D1.
// The "boss" of the game. Only one in the scene (on Core_GameManager).
public class DeliveryManager : MonoBehaviour
{
    public static DeliveryManager Instance;

    [Header("Orders (fill 5 in the Inspector)")]
    public List<DeliveryOrder> orders = new List<DeliveryOrder>();

    [Header("Text")]
    public string chefName = "Chef Lim";

    [Header("Runtime (read only)")]
    public int money = 0;
    public int currentIndex = 0;
    public bool hasPizza = false;

    // Called by InteractZone when the player walks in.
    public void PlayerEntered(InteractZone zone)
    {
    }

    // Called by InteractZone when the player walks out.
    public void PlayerLeft(InteractZone zone)
    {
    }

    // Called by InteractZone when the player presses E inside a zone.
    public void Interact(InteractZone zone)
    {
    }
}
