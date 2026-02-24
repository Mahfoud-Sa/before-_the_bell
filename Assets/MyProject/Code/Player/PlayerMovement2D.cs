using UnityEngine;

public class PlayerMovement2D : MonoBehaviour
{
    private Rigidbody rb;
    private CapsuleCollider coll;
    private SpriteRenderer sprite;
    private Animator anim;

    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;

    private float dirX = 0f;
    private bool _moveRightInput = false;
    private bool _moveLeftInput = false;

    private enum MovementState { idle, running, jumping, falling }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<CapsuleCollider>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        // Prevent the 3D Rigidbody from rotating when it collides / receives torque.
        // This keeps the 2D sprite upright while still using 3D physics for movement.
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            rb.angularVelocity = Vector3.zero;
        }
    }

    private void Update()
    {
        // 1. Reset input
        dirX = 0f;

        // 2. Check Mobile Input first
        if (_moveRightInput) dirX = 1f;
        else if (_moveLeftInput) dirX = -1f;

        // 3. Fallback to Keyboard (for testing on laptop)
        if (dirX == 0)
        {
            dirX = Input.GetAxisRaw("Horizontal");
        }

        // 4. Apply Movement
        // Use 3D Rigidbody velocity (x = horizontal, y = vertical)
        if (rb != null)
        {
            rb.linearVelocity = new Vector3(dirX * moveSpeed, rb.linearVelocity.y, 0f);
        }

        // 5. Jump Logic (Works for Spacebar OR Button click)
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            PerformJump();
        }

        UpdateAnimationState();
    }

    // Call this from your Jump Button's "OnClick" event in the Inspector
    public void PerformJump()
    {
        if (IsGrounded() && rb != null)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, 0f);
        }
    }

    // --- These are called by the MobileButton script ---
    public void PressRight(bool isPressed)
    {
        _moveRightInput = isPressed;
    }

    public void PressLeft(bool isPressed)
    {
        _moveLeftInput = isPressed;
    }

    // --------------------------------------------------

    private void UpdateAnimationState()
    {
        MovementState state;

        // 1. Determine Running vs Idle
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

        // 2. Only check Jumping/Falling if we are NOT grounded
        bool isGrounded = IsGrounded();

        if (!isGrounded && rb != null)
        {
            if (rb.linearVelocity.y > .1f)
            {
                state = MovementState.jumping;
            }
            else if (rb.linearVelocity.y < -.1f)
            {
                state = MovementState.falling;
            }
        }

        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        if (coll == null) return false;

        // Use 3D physics check (raycast down from capsule center).
        // This replaces the 2D BoxCast so it's consistent with a 3D Rigidbody + CapsuleCollider.
        float extraHeight = 0.1f;
        float distance = coll.bounds.extents.y + extraHeight;
        return Physics.Raycast(coll.bounds.center, Vector3.down, distance, jumpableGround);
    }
}