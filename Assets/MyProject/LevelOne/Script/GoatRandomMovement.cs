using UnityEngine;

public class GoatRandomMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float changeDirectionTime = 3f;

    // X axis limits
    public float startPositionX = -5f;
    public float endPositionX = 5f;

    private Rigidbody rb;
    private Vector3 movement;
    private float timer;

    private bool isHeld = false;
    private Collider col;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        ChangeDirection();
    }

    void Update()
    {
        if (isHeld) return;

        timer += Time.deltaTime;
        if (timer >= changeDirectionTime)
        {
            ChangeDirection();
            timer = 0f;
        }
    }

    void FixedUpdate()
    {
        if (isHeld) return;

        Vector3 move = movement * moveSpeed * Time.fixedDeltaTime;
        Vector3 newPosition = rb.position + move;

        // Clamp X position
        newPosition.x = Mathf.Clamp(newPosition.x, startPositionX, endPositionX);

        rb.MovePosition(newPosition);

        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, 10f * Time.fixedDeltaTime);
        }
    }

    void ChangeDirection()
    {
        float randomX = Random.Range(-1f, 1f);

        // Prevent movement outside the range
        if (transform.position.x <= startPositionX && randomX < 0)
            randomX = Mathf.Abs(randomX);

        if (transform.position.x >= endPositionX && randomX > 0)
            randomX = -Mathf.Abs(randomX);

        movement = new Vector3(randomX, 0f, Random.Range(-1f, 1f)).normalized;
    }

    public void Hold(Transform holdPoint)
    {
        isHeld = true;

        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;
        rb.useGravity = false;

        col.enabled = false;

        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void Drop()
    {
        isHeld = false;

        transform.SetParent(null);

        rb.isKinematic = false;
        rb.useGravity = true;

        col.enabled = true;

        ChangeDirection();
    }
}