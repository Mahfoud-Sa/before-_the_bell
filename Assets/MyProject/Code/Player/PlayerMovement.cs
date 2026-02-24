using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private CapsuleCollider coll;
    private SpriteRenderer sprite;
    private Animator anim;

    [Header("Ground / Movement")]
    [SerializeField] private LayerMask jumpableGround;
    [SerializeField] private float moveSpeed = 5f;

    [Header("Animation-driven Jump")]
    [SerializeField] private float jumpHeight = 4f; // peak height multiplier for the animation curve
    [SerializeField] private float jumpAnimDuration = 0.5f; // duration of the animation-driven jump (seconds)
    [SerializeField] private AnimationCurve jumpMotion = new AnimationCurve(
        new Keyframe(0f, 0f), new Keyframe(0.5f, 1f), new Keyframe(1f, 0f)
    );

    private float dirX = 0f;
    private bool _moveRightInput = false;
    private bool _moveLeftInput = false;

    private enum MovementState { idle, running, jumping, runningjumping }

    // Flags for animation-driven jump
    private bool _justRanAndJumped = false;
    private bool _isJumpingAnimated = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<CapsuleCollider>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();

        // Prevent the 3D Rigidbody from rotating when it collides / receives torque.
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

        // 4. Apply horizontal movement (keep Z locked to 0)
        if (rb != null)
        {
            rb.linearVelocity = new Vector3(dirX * moveSpeed, rb.linearVelocity.y, 0f);
        }
        // ÊÔÛíá ÕæÊ ÇáÌÑí
        if (Mathf.Abs(dirX) > 0.1f && IsGrounded())
        {
            SoundManager.Instance?.StartRunSound();
        }
        else
        {
            SoundManager.Instance?.StopRunSound();
        }
        // 5. Jump input: start the animation-driven jump
        if (Input.GetButtonDown("Jump") && IsGrounded() && !_isJumpingAnimated)
        {
            // mark whether we were moving when jumped so animator uses runningjumping
            _justRanAndJumped = Mathf.Abs(dirX) > 0.01f;
            // immediately set animator to start jump animation (prevents frame-lag)
            anim.SetInteger("state", (int)(_justRanAndJumped ? MovementState.runningjumping : MovementState.jumping));
            StartCoroutine(AnimatedJump());
            SoundManager.Instance?.PlayJump();
        }

        UpdateAnimationState();
    }

    // Animated jump coroutine: drives vertical position along jumpMotion curve
    private IEnumerator AnimatedJump()
    {
        if (rb == null || coll == null) yield break;

        _isJumpingAnimated = true;

        // Disable gravity while animation-driven jump is active.
        rb.useGravity = false;

        // Capture start Y so curve is applied relative to current ground level
        float startY = rb.position.y;
        float elapsed = 0f;

        // Run using FixedUpdate timing to use MovePosition safely with physics
        while (elapsed < jumpAnimDuration)
        {
            float t = Mathf.Clamp01(elapsed / jumpAnimDuration);
            float curveValue = jumpMotion.Evaluate(t); // 0..1 shape
            float targetY = startY + curveValue * jumpHeight;

            // Move vertically while preserving horizontal position (use MovePosition)
            rb.MovePosition(new Vector3(rb.position.x, targetY, rb.position.z));

            yield return new WaitForFixedUpdate();
            elapsed += Time.fixedDeltaTime;
        }

        // Ensure final position is applied
        float finalValue = jumpMotion.Evaluate(1f);
        rb.MovePosition(new Vector3(rb.position.x, startY + finalValue * jumpHeight, rb.position.z));

        // Re-enable gravity so natural falling/landing resumes
        rb.useGravity = true;

        // Clear vertical velocity so physics continues smoothly
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);

        // Let one physics step process fall; then clear flags on landing detection in UpdateAnimationState
        _isJumpingAnimated = false;
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

        // If an animation-driven jump is active, force the jump animation state
        if (_isJumpingAnimated)
        {
            state = _justRanAndJumped ? MovementState.runningjumping : MovementState.jumping;
            anim.SetInteger("state", (int)state);
            // flip sprite according to current movement direction
            if (dirX > 0f) sprite.flipX = true;
            else if (dirX < 0f) sprite.flipX = false;
            return;
        }

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
        if (isGrounded)
        {
            // reset running-jump flag when landed
            _justRanAndJumped = false;
        }

        if (!isGrounded && rb != null)
        {
            // prefer runningjumping if we jumped while moving
            if (_justRanAndJumped)
            {
                state = MovementState.runningjumping;
            }
            else if (rb.linearVelocity.y > .1f)
            {
                state = MovementState.jumping;
            }
        }

        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        if (coll == null) return false;

        // Raycast down from capsule center
        float extraHeight = 0.1f;
        float distance = coll.bounds.extents.y + extraHeight;
        return Physics.Raycast(coll.bounds.center, Vector3.down, distance, jumpableGround);
    }
}