using UnityEngine;

public class playerMove : MonoBehaviour
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
    private float jumpLockTimer = 0f; 
    private float actionTimer = 0f;

    private enum MovementState { idle, running, jumping, falling, cutting, watering }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<CapsuleCollider>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
		rb.linearVelocity = Vector3.zero; 
    rb.Sleep();

        if (rb != null)
        {
			rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionZ;
            rb.angularVelocity = Vector3.zero;
        }
    }

    private void Update()
    {
        if (jumpLockTimer > 0) jumpLockTimer -= Time.deltaTime;
        if (actionTimer > 0) actionTimer -= Time.deltaTime;

        bool isGrounded = IsGrounded();

        if (isGrounded && actionTimer <= 0)
        {
            dirX = 0f;
            if (_moveRightInput) dirX = 1f;
            else if (_moveLeftInput) dirX = -1f;
            if (dirX == 0) dirX = Input.GetAxisRaw("Horizontal");
        }
        else
        {
            dirX = 0f;
        }
        if (rb != null)
        {
            rb.linearVelocity = new Vector3(dirX * moveSpeed, rb.linearVelocity.y, 0f);
        }
        if (Input.GetButtonDown("Jump") && isGrounded && dirX == 0f && actionTimer <= 0)
        {
            PerformJump();
        }

        UpdateAnimationState();
    }

    public void StartActionAnim() 
    {
        actionTimer = 0.5f;
    }

    public void PerformJump()
    {
        if (IsGrounded() && rb != null && dirX == 0f)
        {
            rb.linearVelocity = new Vector3(0f, jumpForce, 0f);
            jumpLockTimer = 1.25f; 
        }
    }

    public void PressRight(bool isPressed) => _moveRightInput = isPressed;
    public void PressLeft(bool isPressed) => _moveLeftInput = isPressed;

    private void UpdateAnimationState()
    {
        MovementState state;
        bool isGrounded = IsGrounded();

        if (actionTimer > 0f)
        {
            state = MovementState.watering;
        }
        else if (jumpLockTimer > 0f)
        {
            state = MovementState.jumping;
        }
        else if (!isGrounded)
        {
            state = MovementState.falling;
        }
        else
        {
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
        }
        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        if (coll == null) return false;
        Vector3 spherePosition = new Vector3(coll.bounds.center.x, coll.bounds.min.y + 0.05f, coll.bounds.center.z);
        Collider[] colliders = Physics.OverlapSphere(spherePosition, 0.12f, jumpableGround);
        foreach (Collider c in colliders)
        {
            if (c.transform.root != this.transform.root) return true;
        }
        return false;
    }
}