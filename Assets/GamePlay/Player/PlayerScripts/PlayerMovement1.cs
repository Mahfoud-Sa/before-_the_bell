using SHG.AnimatorCoder;
using UnityEngine;

public class PlayerMovement1 : AnimatorCoder
{
    public static PlayerMovement1 instance;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.2f;
    [SerializeField] private LayerMask groundMask;

    [Header("Base Movement")]
    [SerializeField] private float baseMovementSpeed = 50f;
    [SerializeField] private float baseJumpHeight = 7f;

    private float currentSpeed;
    private float currentJump;

    private float movement = 0f;
    private Rigidbody rb;
    private SpriteRenderer sprite;

    [Header("Mud System (Hit-based)")]
    [SerializeField] private float minSpeed = 0f;

    [Header("Mud Area (Zone-based)")]
    private bool isInMudArea = false;
    private float areaSpeed = -1f;

    [Header("Jump Physics")]
    [SerializeField] private float fallMultiplier = 2.5f;
    [SerializeField] private float jumpMultiplier = 2f;
    [SerializeField] private float jumpTime = 0.3f;

    private bool isJumping;
    private float jumpCounter;
    private Vector3 vecGravity;

    // Mobile input
    private bool _moveRightInput = false;
    private bool _moveLeftInput = false;
    private bool _jumpPressed = false;

    // ---------------- INIT ----------------

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        Initialize();

        rb = GetComponent<Rigidbody>();
        sprite = GetComponent<SpriteRenderer>();

        rb.useGravity = true;
        rb.WakeUp();

        vecGravity = new Vector3(0, -Physics.gravity.y, 0);

        currentSpeed = baseMovementSpeed;
        currentJump = baseJumpHeight;
    }

    // ---------------- UPDATE ----------------

    void Update()
    {
        movement = Input.GetAxisRaw("Horizontal");

        if (_moveRightInput) movement = 1f;
        else if (_moveLeftInput) movement = -1f;

        DefaultAnimation(0);
        CheckJump();
    }

    private void FixedUpdate()
    {
        if (Time.timeScale == 0f) return;

        float finalSpeed = isInMudArea ? areaSpeed : currentSpeed;

        rb.linearVelocity = new Vector3(finalSpeed * movement, rb.linearVelocity.y, 0);

        bool isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        SetBool(Parameters.GROUNDED, isGrounded);
        SetBool(Parameters.FALLING, !isGrounded && rb.linearVelocity.y < 0);
    }

    // ---------------- STATIC MUD SYSTEM ----------------

    public static void HitMud(float amount,float minSpeed)
    {
        if (instance == null) return;

        instance.currentSpeed -= amount;
        instance.currentJump -= amount * 0.1f;

        instance.currentSpeed = Mathf.Max(instance.currentSpeed, minSpeed);

        if (instance.currentSpeed <= 0f)
        {
            GameManager.Instance.GameOver();
        }
    }

    public static void CleanMud(float amount)
    {
        if (instance == null) return;

        instance.currentSpeed += amount;
        instance.currentJump += amount * 0.1f;

        instance.currentSpeed = Mathf.Min(instance.currentSpeed, instance.baseMovementSpeed);
        instance.currentJump = Mathf.Min(instance.currentJump, instance.baseJumpHeight);
    }

    // ---------------- MUD AREA SYSTEM ----------------

    public void EnterMudArea(float speedValue)
    {
        isInMudArea = true;
        areaSpeed = speedValue;
    }

    public void ExitMudArea()
    {
        isInMudArea = false;
        areaSpeed = -1f;
    }

    // ---------------- MOBILE INPUT ----------------

    public void PressRight(bool isPressed) => _moveRightInput = isPressed;
    public void PressLeft(bool isPressed) => _moveLeftInput = isPressed;
    public void PressJump(bool isPressed) => _jumpPressed = isPressed;

    // ---------------- JUMP SYSTEM ----------------

    void CheckJump()
    {
        bool isGrounded = GetBool(Parameters.GROUNDED);

        if (isGrounded && (Input.GetKeyDown(KeyCode.Space) || _jumpPressed))
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, currentJump, 0);

            isJumping = true;
            jumpCounter = 0;

            Play(new(Animations.Run_Jump1, true));
            SoundManager.Instance?.PlayJump();
        }

        if (rb.linearVelocity.y > 0 && isJumping)
        {
            jumpCounter += Time.deltaTime;

            if (jumpCounter > jumpTime)
                isJumping = false;

            float t = jumpCounter / jumpTime;
            float currentJumpM = jumpMultiplier;

            if (t > 0.5f)
                currentJumpM *= (1 - t);

            rb.linearVelocity += vecGravity * currentJumpM * Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.Space) && !_jumpPressed)
        {
            isJumping = false;
            jumpCounter = 0;

            if (rb.linearVelocity.y > 0)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y * 0.2f, 0);
            }
        }

        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity -= vecGravity * fallMultiplier * Time.deltaTime;
        }
    }

    // ---------------- ANIMATION ----------------

    public override void DefaultAnimation(int layer)
    {
        if (movement == 0)
        {
            SoundManager.Instance?.StopRunSound();
            Play(new(Animations.Idle1));
        }
        else
        {
            SoundManager.Instance?.StartRunSound();
            Play(new(Animations.Running1));
        }

        if (movement != 0)
        {
            sprite.flipX = movement > 0;
        }
    }
}