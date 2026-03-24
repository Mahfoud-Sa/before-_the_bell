using UnityEngine;

public class GoatPickup : MonoBehaviour
{
    private Rigidbody rb;
    private Animator animator;

    private bool isPlayerTouching = false;
    private bool isHeld = false;

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
            
            //if (Input.GetKeyDown(KeyCode.E))
            //{
                PickUpGoat();
            //}
        }

        if (isHeld && holdPosition != null)
        {
            transform.position = holdPosition.position;
            transform.rotation = holdPosition.rotation;
        }
    }

    void PickUpGoat()
    {
        Debug.Log("GOAT PICKED UP");

        isHeld = true;

        rb.isKinematic = false;
        rb.useGravity = false;

        transform.SetParent(holdPosition);
        transform.localPosition = Vector3.zero;

        if (animator != null)
        {
            animator.enabled = false;
        }
    }

    public void SetPlayerTouching(bool state)
    {
        isPlayerTouching = state;
    }
}