using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private CapsuleCollider coll;
    private SpriteRenderer sprite;
    private Animator anim;
    public float speed = 5f;

    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;

    private float dirX = 0f;
    private bool _moveRightInput = false;
    private bool _moveLeftInput = false;

    private enum MovementState { idle, running, jumping, falling ,cutting}

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<CapsuleCollider>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotation;
            rb.angularVelocity = Vector3.zero;
        }
    }

    private void Update()
    {

        float h = 0f;
        if (MobileInput.moveLeft) h = -1f;
        if (MobileInput.moveRight)  h = 1f;
        transform.Translate(new Vector3(h * speed * Time.deltaTime, 0, 0));
        dirX = 0f;

        if (_moveRightInput) dirX = 1f;
        else if (_moveLeftInput) dirX = -1f;

        if (dirX == 0)
        {
            dirX = Input.GetAxisRaw("Horizontal");
        }

        if (rb != null)
        {
            rb.linearVelocity = new Vector3(dirX * moveSpeed, rb.linearVelocity.y, 0f);
        }

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            PerformJump();
        }

        UpdateAnimationState();
    }

    public void PerformJump()
    {
        if (IsGrounded() && rb != null)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, 0f);
        }
    }

    public void PressRight(bool isPressed)
    {
        _moveRightInput = isPressed;
    }

    public void PressLeft(bool isPressed)
    {
        _moveLeftInput = isPressed;
    }


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
        float extraHeight = 0.1f;
        float distance = coll.bounds.extents.y + extraHeight;
        return Physics.Raycast(coll.bounds.center, Vector3.down, distance, jumpableGround);
    }
}

