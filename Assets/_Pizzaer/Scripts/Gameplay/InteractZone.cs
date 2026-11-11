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
            if (DeliveryManager.Instance != null) DeliveryManager.Instance.Interact(this);
        }
    }
}
