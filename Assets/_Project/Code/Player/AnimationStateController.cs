using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationStateController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Animator animator;
    private int isWalkingHash;
    private int isJumpingUpHash;
    void Start()
    {
        this.animator = GetComponent<Animator>();
        this.isWalkingHash = Animator.StringToHash("isWalking");
        this.isJumpingUpHash = Animator.StringToHash("isJumpingUp");
    }

    // Update is called once per frame
    void Update()
    {
        bool isJumpingUp = animator.GetBool(isJumpingUpHash);
        bool isWalking = animator.GetBool(isWalkingHash);
        bool forwardPressed = Keyboard.current.wKey.isPressed;
        bool jumpPressed = Keyboard.current.spaceKey.isPressed;
        if (!isWalking && forwardPressed)
        {
            animator.SetBool(isWalkingHash, true);
        }
        if (isWalking && !forwardPressed)
        {
            animator.SetBool(isWalkingHash, false);
        }
        if(!isJumpingUp && jumpPressed)
        {
           
            animator.SetBool(isJumpingUpHash, true);
        }
        if (isJumpingUp && !jumpPressed) 
        {
           
            animator.SetBool(isJumpingUpHash, false);
        }
        //else if (Keyboard.current.sKey.isPressed)
        //{
        //    animator.SetBool("isWalking", true);
        //}
        //else if (Keyboard.current.aKey.isPressed)
        //{
        //    animator.SetBool("isWalking", true);
        //}
        //else if (Keyboard.current.dKey.isPressed)
        //{
        //    animator.SetBool("isWalking", true);
        //}


    }
}
