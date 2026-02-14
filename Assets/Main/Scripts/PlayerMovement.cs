using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    // TODO: Rework movement system this week 2/10-2/15
    public Camera playerCamera;
    private Transform cameraPivot;
    private Transform characterTransform;
    
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = 10f;
    public float lookSpeed = 6f;
    public float lookXLimit = 45f;
    public float lookYLimit = 45f;
    public float defaultHeight = 2f;
    public float crouchHeight = 1f;
    public float crouchSpeed = 3f;

    private Vector3 _moveDirection = Vector3.zero;
    private float _camRotationX = 0;
    private float _camRotationY = 0;
    private Quaternion _playerRotation;
    private CharacterController _characterController;
    private Animator _animator;

    private bool _canMove = true;
    private bool _isJumping = false;
    private bool _isRunning = false;
    private bool _inShiftlock = false;

    private void SetAnim(string state, bool forceOverride = false)
    {
        var current = _animator.GetCurrentAnimatorStateInfo(0);
        
        if (!current.IsName(state) || forceOverride)
        {
            _animator.Play(state, 0, 0.0f);
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

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();
        cameraPivot = playerCamera.transform.parent;
        characterTransform = transform.Find("Character");
        Cursor.lockState = CursorLockMode.Confined;
        // Cursor.visible = false;
    }

    // A function to be placed in Update()
    private void checkShiftlock()
    {
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            _inShiftlock = !_inShiftlock;
        }
    }

    private void manageMovement()
    {
        if (_inShiftlock)
        {

        }
        else
        {
            
        }
    }

    private void manageAnimations()
    {
        // Animations
        if (_isJumping)
        {
            
        }
        else if (_isRunning)
        {
            PlayRunAnim();
        }
        else
        {
            PlayIdleAnim();
        }
    }
    
    //     private void Update()
    // {
    //     checkShiftlock();
    //     Vector3 forward = playerCamera.transform.TransformDirection(Vector3.forward);
    //     Vector3 right = playerCamera.transform.TransformDirection(Vector3.right);
    //
    //     bool isRunning = Input.GetKey(KeyCode.LeftShift);
    //     float curSpeedX = _canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
    //     float curSpeedY = _canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
    //     float movementDirectionY = _moveDirection.y;
    //     if(curSpeedX != 0 && curSpeedY != 0)
    //         _moveDirection = (forward * curSpeedX + (right * curSpeedY)) / 1.41421356237f;
    //     else _moveDirection = (forward * curSpeedX) + (right * curSpeedY);
    //
    //     if (curSpeedX == 0 && curSpeedY == 0)
    //     {
    //         this._isRunning = false;
    //     }
    //     else
    //     {
    //         this._isRunning = true;
    //     }
    //
    //     if (_characterController.isGrounded)
    //     {
    //         _isJumping = false;
    //     }
    //
    //     if (Input.GetButton("Jump") && _canMove && _characterController.isGrounded)
    //     {
    //         _moveDirection.y = jumpPower;
    //         _isJumping = true;
    //         PlayJumpAnim();
    //     }
    //     else
    //     {
    //         _moveDirection.y = movementDirectionY;
    //     }
    //
    //     if (!_characterController.isGrounded)
    //     {
    //         _moveDirection.y -= gravity * Time.deltaTime;
    //     }
    //
    //     if (Input.GetKey(KeyCode.R) && _canMove)
    //     {
    //         _characterController.height = crouchHeight;
    //         walkSpeed = crouchSpeed;
    //         runSpeed = crouchSpeed;
    //
    //     }
    //     else
    //     {
    //         _characterController.height = defaultHeight;
    //         walkSpeed = 6f;
    //         runSpeed = 12f;
    //     }
    //
    //     _characterController.Move(_moveDirection * Time.deltaTime);
    //
    //     if (_canMove)
    //     {
    //         _rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
    //         _rotationX = Mathf.Clamp(_rotationX, -lookXLimit, lookXLimit);
    //         playerCamera.transform.localRotation = Quaternion.Euler(_rotationX, 0, 0);
    //         playerCamera.transform.LookAt(transform);
    //         transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    //     }
    //
    //     manageAnimations();
    // }

    // Problem: 1. Player changes directions instantly  2. Player's directions are inaccurate
    private void Update()
    {
        // checkShiftlock();
        
        // Gets the direction of the forward and right relative to the camera
        Vector3 forward = playerCamera.transform.TransformDirection(Vector3.forward);
        Vector3 right = playerCamera.transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        
        float curSpeedX = _canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = _canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        
        float movementDirectionY = _moveDirection.y;
        
        if(curSpeedX != 0 && curSpeedY != 0)
            _moveDirection = (forward * curSpeedX + (right * curSpeedY)) / 1.41421356237f;
        else _moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (curSpeedX == 0 && curSpeedY == 0)
        {
            _isRunning = false;
        }
        else
        {
            _isRunning = true;
        }

        if (_characterController.isGrounded)
        {
            _isJumping = false;
        }

        if (Input.GetButton("Jump") && _canMove && _characterController.isGrounded)
        {
            _moveDirection.y = jumpPower;
            _isJumping = true;
            PlayJumpAnim();
        }
        else
        {
            _moveDirection.y = movementDirectionY;
        }

        if (!_characterController.isGrounded)
        {
            _moveDirection.y -= gravity * Time.deltaTime;
        }

        if (Input.GetKey(KeyCode.R) && _canMove)
        {
            _characterController.height = crouchHeight;
            walkSpeed = crouchSpeed;
            runSpeed = crouchSpeed;

        }
        else
        {
            _characterController.height = defaultHeight;
            walkSpeed = 6f;
            runSpeed = 12f;
        }

        _characterController.Move(_moveDirection * Time.deltaTime);

        if (_canMove)
        {
            if (Input.GetMouseButton(1))
            {
                _camRotationX += Input.GetAxis("Mouse X") * lookSpeed;
                _camRotationY += Input.GetAxis("Mouse Y") * lookSpeed;
            }

            _camRotationY = Mathf.Clamp(_camRotationY, -lookYLimit, lookYLimit);

            cameraPivot.transform.localRotation = Quaternion.Euler(0, _camRotationX, _camRotationY);

            Quaternion temp = Quaternion.identity;
            // Need to calculate _playerRotationY based on camera's current rotation
            if (Input.GetAxis("Horizontal") >= 0.1f)
            {
                _playerRotation = Quaternion.LookRotation(cameraPivot.forward, Vector3.up);
                temp = Quaternion.LookRotation(cameraPivot.forward, Vector3.up);
                DebugScript.BetterDebug(Quaternion.LookRotation(cameraPivot.forward, Vector3.up));
            } else if (Input.GetAxis("Horizontal") <= -0.1f)
            {
                _playerRotation = -Quaternion.LookRotation(cameraPivot.forward).y;
                temp = Quaternion.LookRotation(cameraPivot.forward * -1, Vector3.up);
            }

            if (Input.GetAxis("Vertical") >= 0.1f)
            {
                _playerRotation = 90f;
            } else if (Input.GetAxis("Vertical") <= -0.1f)
            {
                _playerRotation = 270f;
            }

            // Idk any better way to do it off the top of my mind
            if (Input.GetAxis("Horizontal") >= 0.1f && Input.GetAxis("Vertical") >= 0.1f)
            {
                _playerRotation = 135f;
            } else if (Input.GetAxis("Horizontal") >= 0.1f && Input.GetAxis("Vertical") <= -0.1f)
            {
                _playerRotation = 225f;
            } else if (Input.GetAxis("Horizontal") <= -0.1f && Input.GetAxis("Vertical") >= 0.1f)
            {
                _playerRotation = 45f;
            } else if (Input.GetAxis("Horizontal") <= -0.1f && Input.GetAxis("Vertical") <= -0.1f)
            {
                _playerRotation = 315f;
            }
            
            // if (Input.GetAxis("Horizontal") >= 0.1f)
            // {
            //     _playerRotationY = 180f;
            // } else if (Input.GetAxis("Horizontal") <= -0.1f)
            // {
            //     _playerRotationY = 0f;
            // }
            //
            // if (Input.GetAxis("Vertical") >= 0.1f)
            // {
            //     _playerRotationY = 90f;
            // } else if (Input.GetAxis("Vertical") <= -0.1f)
            // {
            //     _playerRotationY = 270f;
            // }
            //
            // // Idk any better way to do it off the top of my mind
            // if (Input.GetAxis("Horizontal") >= 0.1f && Input.GetAxis("Vertical") >= 0.1f)
            // {
            //     _playerRotationY = 135f;
            // } else if (Input.GetAxis("Horizontal") >= 0.1f && Input.GetAxis("Vertical") <= -0.1f)
            // {
            //     _playerRotationY = 225f;
            // } else if (Input.GetAxis("Horizontal") <= -0.1f && Input.GetAxis("Vertical") >= 0.1f)
            // {
            //     _playerRotationY = 45f;
            // } else if (Input.GetAxis("Horizontal") <= -0.1f && Input.GetAxis("Vertical") <= -0.1f)
            // {
            //     _playerRotationY = 315f;
            // }
            
            // Spherical lerp (treats vectors as directions instead of positions) what the physics
            // characterTransform.rotation = Quaternion.Slerp(characterTransform.rotation, Quaternion.AngleAxis(_playerRotationY, Vector3.up), Time.deltaTime * 10f);
            characterTransform.rotation = Quaternion.Slerp(characterTransform.rotation, temp, Time.deltaTime * 10f);
        }

        manageAnimations();
    }
}