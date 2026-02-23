using UnityEngine;

public class GoatRandomMovement : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float changeDirectionTime = 3f;

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
        rb.MovePosition(rb.position + move);

        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, 10f * Time.fixedDeltaTime);
        }
    }

    void ChangeDirection()
    {
        movement = new Vector3(Random.Range(-1f, 1f), 0f, Random.Range(-1f, 1f)).normalized;
    }

    // ✅ Hold Function — Called when player picks goat
    public void Hold(Transform holdPoint)
    {
        isHeld = true;

        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;
        rb.useGravity = false;

        col.enabled = false; // prevent physics clash

        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    // ✅ Drop Function — optional, can be called later
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