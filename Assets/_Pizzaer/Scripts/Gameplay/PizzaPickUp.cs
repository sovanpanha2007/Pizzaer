using UnityEngine;

public class PizzaPickUp : MonoBehaviour
{
    Collider col;
    Rigidbody rb;
    Transform holdPoint;

    // True while the pizza is attached to the player's hold point.
    public bool IsHeld { get { return holdPoint != null && transform.parent == holdPoint; } }

    void Awake()
    {
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }

    // Attach to the player's hold point (empty child of the player).
    public void PickUp(Transform holdPoint)
    {
        this.holdPoint = holdPoint;
        if (rb != null) rb.isKinematic = true;
        if (col != null) col.enabled = false;
        transform.SetParent(holdPoint, false);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    // While held, follow the hold point every frame. This keeps the pizza
    // glued to the player even if something else moves it.
    void LateUpdate()
    {
        if (holdPoint == null) return;
        if (rb != null && !rb.isKinematic) rb.isKinematic = true;
        transform.position = holdPoint.position;
        transform.rotation = holdPoint.rotation;
    }

    // Put it back in the world at a position (used at the house later).
    public void Drop(Vector3 position)
    {
        transform.SetParent(null);
        transform.position = position;
        if (rb != null) rb.isKinematic = false;
        if (col != null) col.enabled = true;
        holdPoint = null;
    }
}
