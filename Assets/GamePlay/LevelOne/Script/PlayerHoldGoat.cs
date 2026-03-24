using UnityEngine;

public class PlayerHoldGoat : MonoBehaviour
{
    public Transform holdPoint;

    private GameObject goatNearby;
    private GameObject heldGoat;
    private bool isHolding = false;

    public void ToggleHold()
    {
        if (!isHolding && goatNearby != null)
        {
            PickUpGoat();
        }
        else if (isHolding)
        {
            DropGoat();
        }
    }

    private void PickUpGoat()
    {
        heldGoat = goatNearby;
        heldGoat.transform.position = holdPoint.position;
        heldGoat.transform.parent = holdPoint;

        var rb = heldGoat.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.isKinematic = true;
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
                rb.isKinematic = false;
            }

            heldGoat = null;
        }

        isHolding = false;
    }

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