using UnityEngine;

// STUB (A5): fields and public methods only. Viset writes the real code.
// A glowing pillar that floats above the next place to go.
public class TargetMarker : MonoBehaviour
{
    public float height = 4f;       // how high above the target
    public float spinSpeed = 90f;   // degrees per second
    public float bobAmount = 0.3f;  // up/down movement
    public float bobSpeed = 2f;

    // Move above a target and show the marker.
    public void MoveTo(Transform target)
    {
    }

    // Hide the marker (game finished).
    public void Hide()
    {
    }
}
