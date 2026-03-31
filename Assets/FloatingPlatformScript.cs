using UnityEngine;

public class FloatingPlatform : MonoBehaviour
{
    public enum MoveDirection { X, Y, Z }

    [Header("Movement Settings")]
    public MoveDirection direction = MoveDirection.X;
    public float moveDistance = 5f;
    public float speed = 2f;
    [Tooltip("Start platform movement in reverse?")]
    public bool startReverse = false;

    [Header("Optional Smoothness")]
    public bool useEasing = true;
    public float easeSpeed = 2f;

    [Header("Optional Wait Time")]
    public bool waitAtEdges = true;
    public float waitTime = 1f;

    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 target;
    private Rigidbody rb;
    private float t = 0f; // for easing
    private bool waiting = false;

    private void Awake()
    {
        // Ensure Rigidbody exists
        rb = GetComponent<Rigidbody>();
        if (rb == null)
            rb = gameObject.AddComponent<Rigidbody>();

        rb.isKinematic = true;
        rb.useGravity = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    private void Start()
    {
        startPos = transform.position;

        Vector3 dirVector = Vector3.zero;
        switch (direction)
        {
            case MoveDirection.X: dirVector = transform.right; break;
            case MoveDirection.Y: dirVector = transform.up; break;
            case MoveDirection.Z: dirVector = transform.forward; break;
        }

        endPos = startPos + dirVector * moveDistance;

        // If startReverse is true, swap start and end positions
        if (startReverse)
        {
            Vector3 temp = startPos;
            startPos = endPos;
            endPos = temp;
        }

        target = endPos;
    }

    private void FixedUpdate()
    {
        if (waiting) return;

        if (useEasing)
        {
            t += Time.fixedDeltaTime * easeSpeed;
            Vector3 newPos = Vector3.Lerp(startPos, endPos, Mathf.SmoothStep(0f, 1f, t));
            rb.MovePosition(newPos);

            if (t >= 1f)
            {
                SwapStartEnd();
            }
        }
        else
        {
            Vector3 newPos = Vector3.MoveTowards(rb.position, target, speed * Time.fixedDeltaTime);
            rb.MovePosition(newPos);

            if (Vector3.Distance(newPos, target) < 0.01f)
            {
                SwapStartEnd();
            }
        }
    }

    private void SwapStartEnd()
    {
        if (waitAtEdges)
        {
            waiting = true;
            Invoke(nameof(ResumeMovement), waitTime);
        }

        Vector3 temp = startPos;
        startPos = endPos;
        endPos = temp;
        t = 0f;
        target = endPos;
    }

    private void ResumeMovement()
    {
        waiting = false;
    }

    // Parent the player while standing on platform
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
    }

    // Optional: manually reverse at any time
    public void ReverseMovement()
    {
        Vector3 temp = startPos;
        startPos = endPos;
        endPos = temp;
        t = 1f - t;
        target = endPos;
    }
}