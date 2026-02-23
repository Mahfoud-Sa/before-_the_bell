using UnityEngine;

public class PlayerHoldGoat1 : MonoBehaviour
{
   public Transform holdPoint; // Empty object where the goat will be held
    private GameObject goatNearby;
    private GameObject heldGoat;
    private FixedJoint2D holdJoint;
    private bool isHolding = false;

    void Update()
    {
        // Press and hold (space for testing)
        if (Input.GetKey(KeyCode.Space))
        {
            if (!isHolding && goatNearby != null)
                PickUpGoat();
        }
        else
        {
            if (isHolding)
                DropGoat();
        }
    }

    private void PickUpGoat()
    {
        heldGoat = goatNearby;

        holdJoint = heldGoat.AddComponent<FixedJoint2D>();
        holdJoint.connectedBody = GetComponent<Rigidbody2D>();
        holdJoint.autoConfigureConnectedAnchor = false;
        holdJoint.connectedAnchor = holdPoint.localPosition;

        Rigidbody2D rb = heldGoat.GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.linearVelocity = Vector2.zero;

        isHolding = true;
    }

    private void DropGoat()
    {
        if (heldGoat != null)
        {
            if (holdJoint != null)
                Destroy(holdJoint);

            Rigidbody2D rb = heldGoat.GetComponent<Rigidbody2D>();
            rb.gravityScale = 1;

            heldGoat = null;
        }

        isHolding = false;
    }

    // This should be on the child trigger collider (GoatDetector)
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Goat"))
            goatNearby = other.gameObject;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Goat") && other.gameObject == goatNearby)
            goatNearby = null;
    }
}
