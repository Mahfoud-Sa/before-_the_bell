using UnityEngine;

public class GoatPickup : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animator;

    private bool isPlayerTouching = false;
    private bool isHeld = false;

    [Header("Tool Settings")]
    public string requiredToolName = "Gradle";

    [Header("Hold Settings")]
    public Transform holdPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (isPlayerTouching && !isHeld)
        {
            if (AdvancedToolManager.currentToolName == requiredToolName)
            {
                PickUpGoat();
            }
        }

        if (isHeld)
        {
            transform.position = holdPosition.position;
            transform.rotation = holdPosition.rotation;
        }
    }

    void PickUpGoat()
    {
        Debug.Log("GOAT PICKED UP");

        isHeld = true;

        rb.isKinematic = true;
        rb.useGravity = false;

        transform.SetParent(holdPosition);
        transform.localPosition = Vector3.zero;

        // Stop animation
        if (animator != null)
        {
            animator.enabled = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerTouching = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isPlayerTouching = false;
        }
    }
}