using UnityEngine;

public enum ZoneType { Shop, House }

// STUB (A5): fields only. Viset writes the trigger and E-key logic.
// Put on Zone_Shop and Zone_House (root needs a BoxCollider with Is Trigger ON).
public class InteractZone : MonoBehaviour
{
    public ZoneType zoneType = ZoneType.House;

    [Tooltip("Only for houses. Must match the number on the house sign.")]
    public int houseNumber = 1;
}
