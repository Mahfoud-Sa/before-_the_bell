using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement3D : MonoBehaviour
{
    private Rigidbody rb;
    private CapsuleCollider coll;
    private SpriteRenderer sprite;
    private Animator anim;

    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 14f;
    [SerializeField] private float horizontalSmoothing = 10f;
    [SerializeField] private float groundCheckExtra = 0.1f;

    private float dirX = 0f;
    private float dirZ = 0f;

    // mobile input support (optional)
    private bool _moveRightInput = false;
    private bool _moveLeftInput = false;
    private bool _moveForwardInput = false;
    private bool _moveBackwardInput = false;

    private enum MovementState { idle, running, jumping, falling }

   

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<CapsuleCollider>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        // prevent the Rigidbody from rotating the GameObject due to physics
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.angularVelocity = Vector3.zero;
    }

    private void Update()
    {
        // Read inputs (mobile first)
        dirX = 0f;
        dirZ = 0f;

        if (_moveRightInput) dirX = 1f;
        else if (_moveLeftInput) dirX = -1f;

        if (_moveForwardInput) dirZ = 1f;
        else if (_moveBackwardInput) dirZ = -1f;

        // fallback to keyboard for testing
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        if (Mathf.Approximately(dirX, 0f)) dirX = h;
        if (Mathf.Approximately(dirZ, 0f)) dirZ = v;

        // Jump input
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            PerformJump();
        }

        UpdateAnimationState();

        // ensure the transform doesn't get rotated by anything (sprite remains upright)
        transform.rotation = Quaternion.identity;
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        // safety guard
        rb.angularVelocity = Vector3.zero;
    }

    private void ApplyMovement()
    {
        Vector3 current = rb.linearVelocity;
        Vector3 target = new Vector3(dirX * moveSpeed, current.y, dirZ * moveSpeed);

        float vx = Mathf.Lerp(current.x, target.x, horizontalSmoothing * Time.fixedDeltaTime);
        float vz = Mathf.Lerp(current.z, target.z, horizontalSmoothing * Time.fixedDeltaTime);

        rb.linearVelocity = new Vector3(vx, current.y, vz);
    }

    // Call this from your Jump Button's "OnClick" event in the Inspector
    public void PerformJump()
    {
        if (IsGrounded())
        {
            Vector3 v = rb.linearVelocity;
            v.y = jumpForce;
            rb.linearVelocity = v;
        }
    }

    // --- Mobile button hooks (optional) ---
    public void PressRight(bool isPressed) => _moveRightInput = isPressed;
    public void PressLeft(bool isPressed) => _moveLeftInput = isPressed;
    public void PressForward(bool isPressed) => _moveForwardInput = isPressed;
    public void PressBackward(bool isPressed) => _moveBackwardInput = isPressed;
    // --------------------------------------------------

    private void UpdateAnimationState()
    {
        MovementState state;

        // decide running vs idle based on horizontal magnitude on XZ plane
        Vector2 planar = new Vector2(dirX, dirZ);
        if (planar.magnitude > 0.01f)
        {
            state = MovementState.running;
            // flip sprite only when moving left/right
            if (dirX > 0f) sprite.flipX = false;
            else if (dirX < 0f) sprite.flipX = true;
        }
        else
        {
            state = MovementState.idle;
        }

        bool grounded = IsGrounded();

        if (!grounded)
        {
            if (rb.linearVelocity.y > .1f) state = MovementState.jumping;
            else if (rb.linearVelocity.y < -.1f) state = MovementState.falling;
        }

        

        // animator updates (add matching params in Animator)
        
        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        if (coll == null) return false;

        // cast a ray down from the collider center to detect ground (3D)
        float rayLength = coll.bounds.extents.y + groundCheckExtra;
        Vector3 origin = coll.bounds.center;
        bool hit = Physics.Raycast(origin, Vector3.down, rayLength, jumpableGround);

#if UNITY_EDITOR
        Debug.DrawRay(origin, Vector3.down * rayLength, hit ? Color.green : Color.red);
#endif

        return hit;
    }
}