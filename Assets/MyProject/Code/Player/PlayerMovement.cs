using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private CapsuleCollider coll;
    private SpriteRenderer sprite;
    private Animator anim;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;

    [Header("Ground Check")]
    [SerializeField] private LayerMask jumpableGround;

    private float dirX = 0f;
    private bool _moveRightInput = false;
    private bool _moveLeftInput = false;

    private enum MovementState { idle, running, jumping, falling }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<CapsuleCollider>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        if (rb == null) Debug.LogError("❌ Rigidbody missing!");
        if (coll == null) Debug.LogError("❌ CapsuleCollider missing!");
        if (anim == null) Debug.LogError("❌ Animator missing!");
    }

    private void Start()
    {
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.angularVelocity = Vector3.zero;
        rb.useGravity = true;

        Debug.Log("✅ Player initialized");
    }

    private void Update()
    {
        // RESET INPUT
        dirX = 0f;

        // MOBILE INPUT
        if (_moveRightInput) dirX = 1f;
        else if (_moveLeftInput) dirX = -1f;

        // KEYBOARD FALLBACK
        if (dirX == 0f)
            dirX = Input.GetAxisRaw("Horizontal");

        // APPLY MOVEMENT (3D Rigidbody)
        rb.linearVelocity = new Vector3(dirX * moveSpeed, rb.linearVelocity.y, 0f);

        // JUMP INPUT
        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("⬆ Jump button pressed");

            if (IsGrounded())
            {
                Debug.Log("🟢 Grounded → Jump!");
                PerformJump();
            }
            else
            {
                Debug.Log("🔴 Not grounded → No jump");
            }
        }

        UpdateAnimationState();
    }

    public void PerformJump()
    {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, 0f);
    }

    // MOBILE BUTTONS
    public void PressRight(bool isPressed) => _moveRightInput = isPressed;
    public void PressLeft(bool isPressed) => _moveLeftInput = isPressed;

    private void UpdateAnimationState()
    {
        MovementState state;

        if (dirX > 0f)
        {
            state = MovementState.running;
            sprite.flipX = true;
        }
        else if (dirX < 0f)
        {
            state = MovementState.running;
            sprite.flipX = false;
        }
        else
        {
            state = MovementState.idle;
        }

        if (!IsGrounded())
        {
            if (rb.linearVelocity.y > 0.1f)
                state = MovementState.jumping;
            else if (rb.linearVelocity.y < -0.1f)
                state = MovementState.falling;
        }

        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        float extraHeight = 0.1f;
        float distance = coll.bounds.extents.y + extraHeight;

        Vector3 origin = coll.bounds.center;

        bool hit = Physics.Raycast(
            origin,
            Vector3.down,
            distance,
            jumpableGround
        );

        // VISUAL DEBUG
        Debug.DrawRay(
            origin,
            Vector3.down * distance,
            hit ? Color.green : Color.red
        );

        return hit;
    }
    public void PressJump()
{
    if (IsGrounded())
    {
        PerformJump();
    }
}
}
