using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = 10f;
    public float lookSpeed = 2f;
    public float lookXLimit = 45f;
    public float defaultHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchSpeed = 3f;

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;
    private CharacterController characterController;
    private Animator anim;

    private bool canMove = true;
    private bool isJumping = false;
    private bool isRunning = false;
    private bool inShiftlock = false;


    void SetAnim(string state, bool forceOverride = false)
    {
        var current = anim.GetCurrentAnimatorStateInfo(0);


        if (!current.IsName(state) || forceOverride)
        {
            // Debug.Log("Playing " + state);
            anim.Play(state, 0, 0.0f);
        }

    }

    public void PlayJumpAnim()
    {
        SetAnim("Jump", true);
    }

    public void PlayWalkAnim()
    {
        SetAnim("Walk");
    }

    public void PlayIdleAnim()
    {
        SetAnim("Idle");
    }

    public void PlayRunAnim()
    {
        SetAnim("Run");
    }

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        // Cursor.lockState = CursorLockMode.Locked;
        // Cursor.visible = false;
    }

    // A function to be placed in Update()
    private void checkShiftlock()
    {
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            inShiftlock = !inShiftlock;
        }
    }

    private void manageMovement()
    {
        if (inShiftlock)
        {

        }
        else
        {

        }
    }

    private void manageAnimations()
    {
        // Animations
        if (isJumping)
        {

        }
        else if (isRunning)
        {
            PlayRunAnim();
        }
        else
        {
            PlayIdleAnim();
        }
    }

    void Update()
    {
        checkShiftlock();
        Vector3 forward = playerCamera.transform.TransformDirection(Vector3.forward);
        Vector3 right = playerCamera.transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (curSpeedX == 0 && curSpeedY == 0)
        {
            isRunning = false;
        }
        else
        {
            isRunning = true;
        }

        if (characterController.isGrounded)
        {
            isJumping = false;
        }

        if (Input.GetButton("Jump") && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpPower;
            isJumping = true;
            PlayJumpAnim();
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.R) && canMove)
        {
            characterController.height = crouchHeight;
            walkSpeed = crouchSpeed;
            runSpeed = crouchSpeed;

        }
        else
        {
            characterController.height = defaultHeight;
            walkSpeed = 6f;
            runSpeed = 12f;
        }

        characterController.Move(moveDirection * Time.deltaTime);

        if (canMove)
        {
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
            playerCamera.transform.LookAt(transform);
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }

        manageAnimations();
    }
}