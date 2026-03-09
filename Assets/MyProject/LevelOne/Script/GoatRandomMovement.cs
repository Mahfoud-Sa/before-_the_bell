using UnityEngine;

public class GoatWanderAI : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float movementRange = 5f;

    [Header("Random Walk / Idle Time")]
    public float minWalkTime = 4f;
    public float maxWalkTime = 7f;
    public float minIdleTime = 1f;
    public float maxIdleTime = 3f;

    [Header("Flip Target")]
    [Tooltip("Drag the object that should flip when the goat changes direction")]
    public Transform flippedGameObject;

    private Rigidbody rb;

    private bool isWalking = true;
    private bool isHeld = false;

    private float timer = 0f;
    private float currentStateTime = 0f;

    private int direction = 1;

    private Vector3 startPos;
    private float fixedZ;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        startPos = transform.position;
        fixedZ = transform.position.z;

        rb.constraints = RigidbodyConstraints.FreezeRotation |
                         RigidbodyConstraints.FreezePositionZ;

        SetRandomWalkTime();
        PickNewDirection();
    }

    void FixedUpdate()
    {
        if (isHeld) return;

        timer += Time.fixedDeltaTime;

        if (isWalking)
        {
            Move();

            if (timer >= currentStateTime)
                StopWalking();
        }
        else
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);

            if (timer >= currentStateTime)
                StartWalking();
        }

        // Lock Z axis
        transform.position = new Vector3(transform.position.x, transform.position.y, fixedZ);
    }

    void Move()
    {
        float xVelocity = moveSpeed * direction;

        rb.linearVelocity = new Vector3(xVelocity, rb.linearVelocity.y, 0);

        FlipObject(direction);

        if (transform.position.x >= startPos.x + movementRange)
        {
            direction = -1;
        }
        else if (transform.position.x <= startPos.x - movementRange)
        {
            direction = 1;
        }
    }
void FlipObject(int dir)
{
    if (flippedGameObject == null) return;

    Vector3 scale = flippedGameObject.localScale;

    // reversed flip
    scale.x = Mathf.Abs(scale.x) * (dir < 0 ? 1 : -1);

    flippedGameObject.localScale = scale;
}

    void PickNewDirection()
    {
        direction = Random.value > 0.5f ? 1 : -1;
    }

    void StopWalking()
    {
        isWalking = false;
        timer = 0f;

        currentStateTime = Random.Range(minIdleTime, maxIdleTime);

        rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
    }

    void StartWalking()
    {
        isWalking = true;
        timer = 0f;

        SetRandomWalkTime();
        PickNewDirection();
    }

    void SetRandomWalkTime()
    {
        currentStateTime = Random.Range(minWalkTime, maxWalkTime);
    }

    public void Hold(Transform holdPoint)
    {
        isHeld = true;

        rb.linearVelocity = Vector3.zero;
        rb.isKinematic = true;
        rb.useGravity = false;

        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
    }

    public void Drop()
    {
        isHeld = false;

        transform.SetParent(null);

        rb.isKinematic = false;
        rb.useGravity = true;

        StartWalking();
    }
}