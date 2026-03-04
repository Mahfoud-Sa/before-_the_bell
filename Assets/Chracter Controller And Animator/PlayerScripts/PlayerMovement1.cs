using SHG.AnimatorCoder;
using System;
using UnityEngine;
public class PlayerMovement1 : AnimatorCoder


{
    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.2f;
    [SerializeField] private LayerMask groundMask;



    [SerializeField] private CapsuleCollider PlayerCollider;
    [SerializeField] private float movementSpeed;
    public static PlayerMovement1 instance;
    private float movement = 0;
    private Rigidbody rb;

    [Header("Jump System")]
    [SerializeField] private float jumpHeight;
    [SerializeField] private float fallMultiplier;
    [SerializeField] private float jumpMultiplies;
    [SerializeField] private float jumpTime;

    bool isJumping;
    float jumpCounter;

    Vector3 vecGravity;


    private SpriteRenderer sprite;
    private bool _moveRightInput = false;
    private bool _moveLeftInput = false;
    private bool _jumpPressed = false;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        Initialize();
        rb = gameObject.GetComponent<Rigidbody>();
        sprite = rb.GetComponent<SpriteRenderer>();

        vecGravity = new Vector3(0,-Physics.gravity.y, 0);
    }

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
        
        rb.linearVelocity = new(movementSpeed * movement, rb.linearVelocity.y);
        SetBool(Parameters.GROUNDED, Physics.CheckSphere(groundCheck.position, groundDistance, groundMask));
        SetBool(Parameters.FALLING, !GetBool(Parameters.GROUNDED) && rb.linearVelocity.y < 0);
    }
    public void PressRight(bool isPressed) => _moveRightInput = isPressed;
    public void PressLeft(bool isPressed) => _moveLeftInput = isPressed;
    public void PressJump(bool isPressed) => _jumpPressed = isPressed;
    //public void PressJump() => _jumpPressed = true;

    public void CheckJump()
    {
        if (GetBool(Parameters.GROUNDED) && (Input.GetKeyDown(KeyCode.Space) || _jumpPressed))
        {
            
            rb.linearVelocity = new(rb.linearVelocity.x, jumpHeight);
            isJumping = true;
            jumpCounter = 0;
           
            //Play(new(Animations.OneFrameJump, true));
            Play(new(Animations.Run_Jump1, true));
            SoundManager.Instance.PlayJump();
        }

        if (rb.linearVelocity.y > 0 && isJumping)
        {
            jumpCounter += Time.deltaTime;
            if (jumpCounter > jumpTime) isJumping = false;

            float t = jumpCounter / jumpTime;
            float currentJumpM = jumpMultiplies;
            if (t > 0.5f)
            {
                currentJumpM = jumpMultiplies * (1 - t);
            }

            rb.linearVelocity += vecGravity * currentJumpM * Time.deltaTime;
        }

        if (Input.GetKeyUp(KeyCode.Space) && !_jumpPressed) 
        {
            isJumping = false;
            jumpCounter = 0;

            if (rb.linearVelocity.y > 0) 
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x,rb.linearVelocity.y * 0.2f, 0);
            }
        }
       

        if (rb.linearVelocity.y < 0)
        {
            rb.linearVelocity -= vecGravity * fallMultiplier * Time.deltaTime;
        }

    }
   
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
        


        //if (movement != 0) sprite.flipX = movement > 0;
        //if (!GetBool(Parameters.GROUNDED))
        //{
        //    // AIR ANIMATIONS
        //    if (movement == 0)
        //    {
        //        Play(new(Animations.Jump, true));
        //    }
        //    else
        //    {
        //        Play(new(Animations.Run_Jump, true)); 
        //    }
        //}
        //else
        //{
        //    // GROUND ANIMATIONS
        //    if (movement == 0)
        //    {
        //        Play(new(Animations.Idle));
        //    }
        //    else
        //    {
        //        Play(new(Animations.Running));
        //    }
        //}
    }
}
