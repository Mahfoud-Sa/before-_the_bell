using UnityEngine;

public class PalmBridge : MonoBehaviour
{
    private Rigidbody rb;
    private bool hasFallen = false;
    private bool isPlayerTouching = false;

    [Header("Tool Settings")]
    public string requiredToolName = "Axe"; // MUST match the sprite name exactly

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        rb.isKinematic = true;
        rb.useGravity = true;
    }

    void Update()
    {
        // If player is touching AND tree didn't fall yet
        if (isPlayerTouching && !hasFallen)
        {
            // Check if Axe is currently selected
            if (AdvancedToolManager.currentToolName == requiredToolName)
            {
                StartFalling();
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerTouching = true;
            Debug.Log("Player touching tree: " + gameObject.name);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerTouching = false;
            Debug.Log("Player left tree: " + gameObject.name);
        }
    }

void StartFalling()
{
    hasFallen = true;

    rb.isKinematic = false;
    rb.useGravity = true;

    rb.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

    Vector3 fallAxis = Vector3.Cross(transform.forward, Vector3.up).normalized;

    rb.AddTorque(fallAxis * 30f, ForceMode.Impulse);

    Debug.Log("Tree falling forward correctly");
}
}