using UnityEngine;
using UnityEngine.InputSystem;

// ---------------------------------------------------------------------------
// FreeLookCamera
//
// A third-person camera that ORBITS around a target (the player).
//
// Controls:
//   - Hold Right Mouse Button + move mouse   -> rotate the camera around the player
//   - Scroll Wheel                            -> zoom in / out
//
// The camera only rotates WHILE the right mouse button is held; releasing it
// freezes the camera's angle. The cursor is locked during rotation and
// unlocked when released.
//
// IMPORTANT - timing:
//   This script runs in Update, and PlayerMovement runs in LateUpdate.
//   That order guarantees the camera rotates FIRST, so movement can read the
//   camera's FRESH forward direction in the same frame.
// ---------------------------------------------------------------------------
public class FreeLookCamera : MonoBehaviour
{
    [Header("Target")]
    public Transform target;            // the object to orbit around (the player)

    [Header("Orbit")]
    public float distance = 5f;         // how far the camera stays from the target
    public float minDistance = 2f;      // closest zoom
    public float maxDistance = 10f;     // farthest zoom
    public float sensitivity = 2f;      // mouse speed multiplier
    public float minPitch = -30f;       // lowest allowed look-down angle
    public float maxPitch = 60f;        // highest allowed look-up angle

    [Header("Collision (optional)")]
    public LayerMask collisionMask = ~0; // what the camera should avoid clipping through

    float yaw;        // horizontal rotation (around Y axis)
    float pitch = 20f; // vertical rotation (up/down)

    void Awake()
    {
        // If no target was assigned, try to find the player by its tag.
        if (target == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) target = p.transform;
        }

        // Start from the camera's current angle so it doesn't snap on the first frame.
        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
    }

    void Update()
    {
        if (target == null || Mouse.current == null) return;

        bool holding = Mouse.current.rightButton.isPressed;

        // --- Rotate ONLY while holding right-click.
        if (holding)
        {
            Vector2 delta = Mouse.current.delta.ReadValue();
            yaw   += delta.x * sensitivity;   // move mouse left/right -> spin around
            pitch -= delta.y * sensitivity;   // move mouse up/down   -> look up/down
            pitch  = Mathf.Clamp(pitch, minPitch, maxPitch); // don't flip over the top/bottom
        }

        // --- Optional: scroll wheel zoom.
        float scroll = Mouse.current.scroll.ReadValue().y;
        if (Mathf.Abs(scroll) > 0.01f)
        {
            distance -= scroll * 0.5f;
            distance  = Mathf.Clamp(distance, minDistance, maxDistance);
        }

        // --- Lock the cursor while orbiting so the mouse can keep turning freely.
        Cursor.lockState = holding ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible   = !holding;

        // --- Position the camera behind/around the target based on yaw/pitch.
        Quaternion rotation  = Quaternion.Euler(pitch, yaw, 0f);
        Vector3   desiredPos = target.position - rotation * Vector3.forward * distance;

        // --- Optional: if something is between the target and the camera,
        //     pull the camera in so it doesn't clip through the wall/object.
        if (Physics.Linecast(target.position, desiredPos, out RaycastHit hit, collisionMask))
            desiredPos = hit.point;

        transform.position = desiredPos;
        transform.rotation = rotation;
    }
}
