using UnityEngine;

public class DogFollowAI : MonoBehaviour
{
    [Header("Detection Settings")]
    public Transform player; // اسحب كائن الطالب هنا
    public float detectionRange = 5f; // المسافة التي يراك الكلب منها
    public float stopDistance = 0.5f; // المسافة التي يتوقف عندها الكلب بجانبك

    [Header("Movement Settings")]
    public float moveSpeed = 4f;
    public float movementRange = 8f;

    [Header("Random Walk / Idle Time")]
    public float minWalkTime = 3f;
    public float maxWalkTime = 6f;
    public float minIdleTime = 1f;
    public float maxIdleTime = 2f;

    [Header("Flip Target")]
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

        rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;

        SetRandomWalkTime();
        PickNewDirection();
    }

    void FixedUpdate()
    {
        if (isHeld || player == null) return;

        // حساب المسافة بين الكلب والطالب
        float distanceToPlayer = Vector2.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            ChasePlayer();
        }
        else
        {
            WanderLogic();
        }

        // قفل محور Z دائماً
        transform.position = new Vector3(transform.position.x, transform.position.y, fixedZ);
    }

    void ChasePlayer()
    {
        // تحديد الاتجاه نحو اللاعب (يمين أو يسار فقط)
        float moveDir = player.position.x > transform.position.x ? 1 : -1;
        direction = (int)moveDir;

        float distanceX = Mathf.Abs(player.position.x - transform.position.x);

        if (distanceX > stopDistance)
        {
            rb.linearVelocity = new Vector3(moveDir * moveSpeed, rb.linearVelocity.y, 0);
            FlipObject(direction);
        }
        else
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
        }
    }

    void WanderLogic()
    {
        timer += Time.fixedDeltaTime;

        if (isWalking)
        {
            MoveWander();
            if (timer >= currentStateTime) StopWalking();
        }
        else
        {
            rb.linearVelocity = new Vector3(0, rb.linearVelocity.y, 0);
            if (timer >= currentStateTime) StartWalking();
        }
    }

    void MoveWander()
    {
        rb.linearVelocity = new Vector3(moveSpeed * direction, rb.linearVelocity.y, 0);
        FlipObject(direction);

        // العودة إذا خرج عن النطاق المسموح
        if (transform.position.x >= startPos.x + movementRange) direction = -1;
        else if (transform.position.x <= startPos.x - movementRange) direction = 1;
    }

    void FlipObject(int dir)
    {
        if (flippedGameObject == null) return;
        Vector3 scale = flippedGameObject.localScale;
        // قلب الكائن بناءً على الاتجاه
        scale.x = Mathf.Abs(scale.x) * (dir < 0 ? 1 : -1);
        flippedGameObject.localScale = scale;
    }

    void SetRandomWalkTime() => currentStateTime = Random.Range(minWalkTime, maxWalkTime);
    void PickNewDirection() => direction = Random.value > 0.5f ? 1 : -1;

    void StopWalking()
    {
        isWalking = false;
        timer = 0f;
        currentStateTime = Random.Range(minIdleTime, maxIdleTime);
    }

    void StartWalking()
    {
        isWalking = true;
        timer = 0f;
        SetRandomWalkTime();
        PickNewDirection();
    }
}