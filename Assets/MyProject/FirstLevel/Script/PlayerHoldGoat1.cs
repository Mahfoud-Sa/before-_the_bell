using UnityEngine;

public class PlayerHoldGoat3D : MonoBehaviour
{
    [Header("Settings")]
    public Transform holdPoint;       // Where the goat will be held
    public float maxDistance = 2f;    // Max distance to pick up
    public GameObject goat;           // Assign your single goat in the Inspector

    private GameObject heldGoat;
    private FixedJoint holdJoint;
    private bool isHolding = false;

    public void PickUpGoat()
    {
        if (isHolding)
        {
            Debug.Log("Already holding a goat.");
            return;
        }

        if (goat == null)
        {
            Debug.LogWarning("No goat assigned!");
            return;
        }

        float distance = Vector3.Distance(holdPoint.position, goat.transform.position);
        Debug.Log($"Distance to goat '{goat.name}': {distance}");

        if (distance > maxDistance)
        {
            Debug.Log($"Goat '{goat.name}' is too far to pick up (maxDistance = {maxDistance})");
            return;
        }

        // Pick up the goat
        heldGoat = goat;

        // Disable goat movement
        GoatRandomMovement goatMovement = heldGoat.GetComponent<GoatRandomMovement>();
        if (goatMovement != null) goatMovement.enabled = false;

        Rigidbody rb = heldGoat.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }

        // Attach FixedJoint to player
        holdJoint = heldGoat.AddComponent<FixedJoint>();
        holdJoint.connectedBody = GetComponent<Rigidbody>();
        holdJoint.autoConfigureConnectedAnchor = false;
        holdJoint.anchor = heldGoat.transform.InverseTransformPoint(heldGoat.transform.position);
        holdJoint.connectedAnchor = holdPoint.localPosition;

        isHolding = true;
        Debug.Log($"Picked up goat '{heldGoat.name}' successfully");
    }

    public void DropGoat()
    {
        if (!isHolding || heldGoat == null)
        {
            Debug.Log("No goat to drop.");
            return;
        }

        if (holdJoint != null)
        {
            Destroy(holdJoint);
        }

        Rigidbody rb = heldGoat.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        GoatRandomMovement goatMovement = heldGoat.GetComponent<GoatRandomMovement>();
        if (goatMovement != null)
        {
            goatMovement.enabled = true;
        }

        Debug.Log($"Dropped goat '{heldGoat.name}'");
        heldGoat = null;
        isHolding = false;
    }

    private void OnDrawGizmos()
    {
        if (holdPoint != null)
        {
            // Draw a sphere showing the max pickup distance
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(holdPoint.position, maxDistance);
        }
    }

    void Update()
    {
        if (goat != null)
        {
            float distance = Vector3.Distance(holdPoint.position, goat.transform.position);
            Debug.DrawLine(holdPoint.position, goat.transform.position, Color.red);
            Debug.Log($"Distance to goat '{goat.name}': {distance}");
        }
    }
}