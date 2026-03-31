using SHG.AnimatorCoder;
using UnityEngine;
using System.Collections.Generic;

public class PlayerMovement1 : AnimatorCoder
{
    public static PlayerMovement1 instance;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.2f;
    [SerializeField] private LayerMask groundMask;

    [Header("Spot images")]
    [SerializeField] private GameObject hotspot01;
    [SerializeField] private GameObject hotspot02;
    [SerializeField] private GameObject hotspot03;
    [SerializeField] private GameObject hotspot04;
    [SerializeField] private GameObject hotspot05;

    private GameObject[] hotspots;

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

        hotspots = new GameObject[] { hotspot01, hotspot02, hotspot03, hotspot04, hotspot05 };
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

    private bool _jumpActionReceived = false;
    private bool _stopJumpActionReceived = false;

    // ---------------- UPDATE ----------------

    void Update()
    {
        movement = Input.GetAxisRaw("Horizontal");

        if (_moveRightInput) movement = 1f;
        else if (_moveLeftInput) movement = -1f;

        DefaultAnimation(0);

        // Capture jump inputs in Update
        if (Input.GetKeyDown(KeyCode.Space) || _jumpPressed)
            _jumpActionReceived = true;
            
        if (Input.GetKeyUp(KeyCode.Space) && !_jumpPressed)
            _stopJumpActionReceived = true;
    }

    private void FixedUpdate()
    {
        if (Time.timeScale == 0f) return;

        float finalSpeed = isInMudArea ? areaSpeed : currentSpeed;

        rb.linearVelocity = new Vector3(finalSpeed * movement, rb.linearVelocity.y, 0);

        bool isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        SetBool(Parameters.GROUNDED, isGrounded);
        SetBool(Parameters.FALLING, !isGrounded && rb.linearVelocity.y < 0);

        CheckJump(isGrounded);
    }

    // ---------------- HOTSPOT SYSTEM ----------------

    private void ActivateRandomHotspot()
    {
        List<GameObject> inactiveHotspots = new List<GameObject>();

        foreach (var spot in hotspots)
        {
            if (spot != null && !spot.activeSelf)
                inactiveHotspots.Add(spot);
        }

        if (inactiveHotspots.Count == 0) return;

        int randomIndex = Random.Range(0, inactiveHotspots.Count);
        inactiveHotspots[randomIndex].SetActive(true);
    }

    private void DeactivateRandomHotspot()
    {
        List<GameObject> activeHotspots = new List<GameObject>();

        foreach (var spot in hotspots)
        {
            if (spot != null && spot.activeSelf)
                activeHotspots.Add(spot);
        }

        if (activeHotspots.Count == 0) return;

        int randomIndex = Random.Range(0, activeHotspots.Count);
        activeHotspots[randomIndex].SetActive(false);
    }

    // ---------------- MUD SYSTEM ----------------

    public static void HitMud(float amount, float minSpeed)
    {
        if (instance == null) return;

        // Decrease speed & jump (balanced)
        instance.currentSpeed -= amount;
        instance.currentJump -= amount * 0.1f;

        // Clamp minimum speed
        instance.currentSpeed = Mathf.Max(instance.currentSpeed, minSpeed);

        // Activate visual effect
        instance.ActivateRandomHotspot();

        Debug.Log("HitMud -> Speed: " + instance.currentSpeed);

        if (instance.currentSpeed <= 0f)
        {
            Debug.Log("Game Over: Player is stuck in the mud!");
            GameManager.Instance.GameOver();
        }
    }

    public static void CleanMud(float amount)
    {
        if (instance == null) return;

        // Restore speed & jump
        instance.currentSpeed += amount;
        instance.currentJump += amount * 0.1f;

        // Clamp to base values
        instance.currentSpeed = Mathf.Min(instance.currentSpeed, instance.baseMovementSpeed);
        instance.currentJump = Mathf.Min(instance.currentJump, instance.baseJumpHeight);

        // Reverse visual effect
        instance.DeactivateRandomHotspot();

        Debug.Log("CleanMud -> Speed: " + instance.currentSpeed);
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

    void CheckJump(bool isGrounded)
    {
        if (isGrounded && _jumpActionReceived)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, currentJump, 0);

            isJumping = true;
            jumpCounter = 0;

            Play(new(Animations.Run_Jump1, true));
            SoundManager.Instance?.PlayJump();
            _jumpActionReceived = false;
        }
        else
        {
            _jumpActionReceived = false;
        }

        if (rb.linearVelocity.y > 0 && isJumping)
        {
            jumpCounter += Time.fixedDeltaTime;

            if (jumpCounter > jumpTime)
                isJumping = false;

            float t = jumpCounter / jumpTime;
            float currentJumpM = jumpMultiplier;

            if (t > 0.5f)
                currentJumpM *= (1 - t);

            rb.linearVelocity += vecGravity * currentJumpM * Time.fixedDeltaTime;
        }

        if (_stopJumpActionReceived)
        {
            isJumping = false;
            jumpCounter = 0;

            if (rb.linearVelocity.y > 0)
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y * 0.2f, 0);
            }
            _stopJumpActionReceived = false;
        }

        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity -= vecGravity * fallMultiplier * Time.fixedDeltaTime;
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