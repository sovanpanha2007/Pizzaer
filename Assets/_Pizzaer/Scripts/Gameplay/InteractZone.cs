using UnityEngine;
using UnityEngine.InputSystem;

public enum ZoneType { Shop, House }

// Put on Zone_Shop and Zone_House (root needs a BoxCollider with Is Trigger ON).
[RequireComponent(typeof(BoxCollider))]
public class InteractZone : MonoBehaviour
{
    public ZoneType zoneType = ZoneType.House;

    [Tooltip("Only for houses. Must match the number on the house sign.")]
    public int houseNumber = 1;

    [Header("Pickup (shop zone only)")]
    public PizzaPickUp pizza;
    public Transform holdPoint;

    [Header("Drop (house zone only)")]
    public Transform dropSpot;

    bool playerInside = false;

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInside = true;
        Debug.Log("[Zone] Player entered " + zoneType + " zone.");
        if (DeliveryManager.Instance != null) DeliveryManager.Instance.PlayerEntered(this);
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        playerInside = false;
        Debug.Log("[Zone] Player left " + zoneType + " zone.");
        if (DeliveryManager.Instance != null) DeliveryManager.Instance.PlayerLeft(this);
    }

    void Update()
    {
        if (!playerInside) return;
        if (Keyboard.current == null) return;

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            Debug.Log("[Zone] E pressed in " + zoneType + " zone.");

            if (zoneType == ZoneType.Shop)
                TryPickUp();
            else
                TryDrop();

            if (DeliveryManager.Instance != null) DeliveryManager.Instance.Interact(this);
        }
    }

    void TryPickUp()
    {
        if (pizza == null || holdPoint == null)
        {
            Debug.LogWarning("[Zone] Pizza or Hold Point is not set on " + name + ".");
            return;
        }

        if (pizza.IsHeld)
        {
            Debug.Log("[Zone] Already carrying the pizza.");
            return;
        }

        pizza.PickUp(holdPoint);
        Debug.Log("[Zone] Picked up the pizza.");
    }

    void TryDrop()
    {
        if (pizza == null)
        {
            Debug.LogWarning("[Zone] Pizza is not set on " + name + ".");
            return;
        }

        if (!pizza.IsHeld)
        {
            Debug.Log("[Zone] Not carrying a pizza, nothing to drop.");
            return;
        }

        if (!IsCorrectHouse())
        {
            Debug.Log("[Zone] Wrong house, keeping the pizza.");
            return;
        }

        Vector3 spot = dropSpot != null ? dropSpot.position : transform.position;
        if (dropSpot == null) Debug.LogWarning("[Zone] Drop Spot is not set on " + name + ". Dropping at the house center.");

        pizza.Drop(spot);
        Debug.Log("[Zone] Dropped the pizza.");
    }

    bool IsCorrectHouse()
    {
        DeliveryManager dm = DeliveryManager.Instance;
        if (dm == null) return false;
        if (dm.orders == null || dm.currentIndex < 0 || dm.currentIndex >= dm.orders.Count) return false;
        return dm.orders[dm.currentIndex].houseNumber == houseNumber;
    }
}
