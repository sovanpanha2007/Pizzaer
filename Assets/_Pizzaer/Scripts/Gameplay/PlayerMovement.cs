using UnityEngine;
using UnityEngine.InputSystem;

// ---------------------------------------------------------------------------
// PlayerMovement
//
// Handles the player's movement in the Town scene:
//   - W A S D            walk (camera-relative)
//   - Left Shift         run
//   - Space              jump
//
// The movement direction is ALWAYS relative to the camera's horizontal
// facing direction (where the camera points on the X/Z plane), so "W"
// moves you toward where the camera is looking.
//
// Requirements:
//   - A CharacterController on the same GameObject (added automatically).
//   - A Camera assigned to `cam` (or left null -> it uses Camera.main).
//
// IMPORTANT - timing:
//   This script runs in LateUpdate, and FreeLookCamera runs in Update.
//   That order guarantees the camera is rotated FIRST, and this script
//   reads the camera's FRESH forward direction in the same frame (no lag).
//
// The jump uses a reliable ground check + "coyote time" + "jump buffer"
// so it feels consistent instead of occasionally dropping the input.
// ---------------------------------------------------------------------------
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float walkSpeed = 3f;        // normal walk speed (m/s)
    public float runSpeed = 6f;         // speed while holding Left Shift
    public float rotationSpeed = 10f;   // how fast the body turns to face movement

    [Header("Jump & Gravity")]
    public float jumpHeight = 1.2f;     // desired jump height (m)
    public float gravity = -9.81f;      // downward acceleration (m/s^2)

    [Header("Ground Check")]
    public float groundCheckDistance = 0.3f; // how far below the feet to look for ground
    public LayerMask groundMask = ~0;        // what counts as ground (everything by default)

    [Header("Jump Feel")]
    public float coyoteTime = 0.1f;      // still allowed to jump this long after leaving ground
    public float jumpBufferTime = 0.1f;  // a Space press is remembered this long before landing

    [Header("References")]
    public Camera cam;                   // the camera used for movement direction (optional)

    CharacterController controller;      // Unity's built-in movement collider
    Vector3 velocity;                    // current vertical speed (gravity/jump)

    bool isGrounded;                     // true when the ground check hits something below
    float lastGroundedTime;              // the last Time.time the player was grounded
    float lastJumpPressedTime;           // the last Time.time Space was pressed

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        if (cam == null) cam = Camera.main;

        // Start these far in the past so there is no accidental first-frame jump.
        lastGroundedTime = -999f;
        lastJumpPressedTime = -999f;
    }

    void LateUpdate()
    {
        if (Keyboard.current == null) return;

        // --- Read WASD input into a 2D direction (x = left/right, y = forward/back).
        Vector2 move = Vector2.zero;
        if (Keyboard.current.wKey.isPressed) move.y += 1f;
        if (Keyboard.current.sKey.isPressed) move.y -= 1f;
        if (Keyboard.current.dKey.isPressed) move.x += 1f;
        if (Keyboard.current.aKey.isPressed) move.x -= 1f;
        move = Vector2.ClampMagnitude(move, 1f); // diagonal isn't faster than straight

        bool run = Keyboard.current.leftShiftKey.isPressed;
        float speed = run ? runSpeed : walkSpeed;

        // --- Build a world-space direction from the camera's horizontal facing.
        Vector3 dir;
        if (cam != null)
        {
            Vector3 forward = cam.transform.forward;
            Vector3 right = cam.transform.right;
            forward.y = 0f; right.y = 0f;      // flatten so we don't move up/down with the camera tilt
            forward.Normalize(); right.Normalize();
            dir = forward * move.y + right * move.x;
        }
        else
        {
            // No camera assigned -> fall back to world axes.
            dir = new Vector3(move.x, 0f, move.y);
        }

        // --- Apply horizontal movement + rotate the body to face where it's going.
        if (move.sqrMagnitude > 0.01f)
        {
            controller.Move(dir * speed * Time.deltaTime);
            Quaternion target = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, target, rotationSpeed * Time.deltaTime);
        }

        // --- Ground check (reliable sphere cast, see CheckGrounded() below).
        isGrounded = CheckGrounded();

        // --- Stick to the ground so the controller doesn't "float" on slopes.
        if (isGrounded && velocity.y < 0f) velocity.y = -2f;

        // --- Coyote time: remember we were grounded, but only while NOT rising,
        //     so a quick second Space press can't cause an accidental double jump.
        if (isGrounded && velocity.y <= 0f) lastGroundedTime = Time.time;

        // --- Jump buffer: remember the exact moment Space was pressed.
        if (Keyboard.current.spaceKey.wasPressedThisFrame) lastJumpPressedTime = Time.time;

        bool canJump   = (Time.time - lastGroundedTime)    <= coyoteTime;     // on/near ground recently
        bool wantsJump = (Time.time - lastJumpPressedTime) <= jumpBufferTime; // pressed Space recently

        if (canJump && wantsJump)
        {
            // v = sqrt(2 * g * h)  -> the upward speed needed to reach `jumpHeight`.
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            lastGroundedTime    = -999f; // consume the coyote window (no double jump)
            lastJumpPressedTime = -999f; // consume the buffered press
        }

        // --- Apply gravity and move vertically.
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    // Casts a small sphere downward from the body center to detect ground.
    // More reliable than controller.isGrounded (which flickers false while
    // running, on slopes, or the frame you start falling).
    bool CheckGrounded()
    {
        Vector3 origin = transform.position + controller.center;
        return Physics.SphereCast(
            origin,
            controller.radius * 0.9f,
            Vector3.down,
            out _,
            controller.height * 0.5f - controller.radius + groundCheckDistance,
            groundMask);
    }
}
