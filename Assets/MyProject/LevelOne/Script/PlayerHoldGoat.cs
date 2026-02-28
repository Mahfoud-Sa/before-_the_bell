using UnityEngine;

public class PlayerHoldGoat : MonoBehaviour
{
   public Transform holdPoint; // Where the goat will be held
    private GameObject goatNearby;
    private GameObject heldGoat;
    private bool isHolding = false;

    void Update()
    {
        // Press and hold button (space, or any custom input)
        if (Input.GetKey(KeyCode.Space)) 
        {
            if (!isHolding && goatNearby != null)
            {
                PickUpGoat();
            }
        }
        else
        {
            if (isHolding)
            {
                DropGoat();
            }
        }
    }

    private void PickUpGoat()
    {
        heldGoat = goatNearby;
        heldGoat.transform.position = holdPoint.position;
        heldGoat.transform.parent = holdPoint; // Make it follow player
        var rb = heldGoat.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.isKinematic = true; // Disable physics while holding
        }
        isHolding = true;
    }

    private void DropGoat()
    {
        if (heldGoat != null)
        {
            heldGoat.transform.parent = null;
            var rb = heldGoat.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.isKinematic = false; // Re-enable physics
            }
            heldGoat = null;
        }
        isHolding = false;
    }

    // Detect when goat is close
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Goat"))
        {
            goatNearby = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Goat") && other.gameObject == goatNearby)
        {
            goatNearby = null;
        }
    }
}
