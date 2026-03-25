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
    [SerializeField] private float moveSpeed = 7f;

    [Header("Animation-driven Jump")]
    [SerializeField] private float jumpHeight = 7f;
    [SerializeField] private float jumpAnimDuration = 0.5f;
    [SerializeField]
    private AnimationCurve jumpMotion = new AnimationCurve(
        new Keyframe(0f, 0f),
        new Keyframe(0.5f, 1f),
        new Keyframe(1f, 0f)
    );

    [Header("Run Jump Boost")]
    [SerializeField] private float runJumpBoost = 3f; // 🔥 اندفاعة اضافية عند القفز اثناء الجري

    private float dirX = 0f;

    private bool _moveRightInput = false;
    private bool _moveLeftInput = false;
    private bool _jumpPressed = false;

    private enum MovementState { idle, running, jumping, runningjumping }

    private bool _justRanAndJumped = false;
    private bool _isJumpingAnimated = false;

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
        // Reset horizontal input
        dirX = 0f;

        if (_moveRightInput) dirX = 1f;
        else if (_moveLeftInput) dirX = -1f;

        if (dirX == 0)
            dirX = Input.GetAxisRaw("Horizontal");

        if (rb != null)
        {
            rb.linearVelocity = new Vector3(dirX * moveSpeed, rb.linearVelocity.y, 0f);
        }

        if (Mathf.Abs(dirX) > 0.1f && IsGrounded())
            SoundManager.Instance?.StartRunSound();
        else
            SoundManager.Instance?.StopRunSound();

        if ((_jumpPressed || Input.GetButtonDown("Jump")) && IsGrounded() && !_isJumpingAnimated)
        {
            _jumpPressed = false;

            _justRanAndJumped = Mathf.Abs(dirX) > 0.01f;

            anim.SetInteger("state",
                (int)(_justRanAndJumped ? MovementState.runningjumping : MovementState.jumping));

            StartCoroutine(AnimatedJump());

            SoundManager.Instance?.PlayJump();
        }

        UpdateAnimationState();
    }

    private IEnumerator AnimatedJump()
    {
        if (rb == null || coll == null) yield break;

        _isJumpingAnimated = true;
        rb.useGravity = false;

        float startY = rb.position.y;
        float currentX = rb.position.x;

        // 🔥 نأخذ السرعة الحالية
        float appliedHorSpeed = rb.linearVelocity.x;

        // 🔥 إضافة اندفاعة اذا كان يجري
        if (Mathf.Abs(dirX) > 0.1f)
        {
            appliedHorSpeed += dirX * runJumpBoost;
        }

        float elapsed = 0f;

        while (elapsed < jumpAnimDuration)
        {
            float t = Mathf.Clamp01(elapsed / jumpAnimDuration);
            float curveValue = jumpMotion.Evaluate(t);
            float targetY = startY + curveValue * jumpHeight;

            // تحريك X و Y معاً
            currentX += appliedHorSpeed * Time.fixedDeltaTime;

            rb.MovePosition(new Vector3(currentX, targetY, rb.position.z));

            yield return new WaitForFixedUpdate();
            elapsed += Time.fixedDeltaTime;
        }

        float finalValue = jumpMotion.Evaluate(1f);
        rb.MovePosition(new Vector3(currentX, startY + finalValue * jumpHeight, rb.position.z));

        rb.useGravity = true;

        // نحافظ على السرعة الأفقية بعد الهبوط
        rb.linearVelocity = new Vector3(appliedHorSpeed, 0f, rb.linearVelocity.z);

        _isJumpingAnimated = false;
    }

    public void PressRight(bool isPressed) => _moveRightInput = isPressed;
    public void PressLeft(bool isPressed) => _moveLeftInput = isPressed;
    public void PressJump() => _jumpPressed = true;

    private void UpdateAnimationState()
    {
        MovementState state;

        if (_isJumpingAnimated)
        {
            state = _justRanAndJumped ? MovementState.runningjumping : MovementState.jumping;
            anim.SetInteger("state", (int)state);

            if (dirX > 0f) sprite.flipX = true;
            else if (dirX < 0f) sprite.flipX = false;

            return;
        }

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

        bool grounded = IsGrounded();

        if (grounded)
            _justRanAndJumped = false;

        if (!grounded && rb != null)
        {
            if (_justRanAndJumped)
                state = MovementState.runningjumping;
            else
                state = MovementState.jumping;
        }

        anim.SetInteger("state", (int)state);
    }

    private bool IsGrounded()
    {
        if (coll == null) return false;

        float extraHeight = 0.1f;
        float distance = coll.bounds.extents.y + extraHeight;

        return Physics.Raycast(
            coll.bounds.center,
            Vector3.down,
            distance,
            jumpableGround
        );
    }
}