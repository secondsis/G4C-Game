using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
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
    
    // Don't need crouching until buildings exist
    // public float crouchHeight = 1f;
    // public float crouchSpeed = 3f;

    private Vector3 _moveDirection = Vector3.zero;
    private float _camRotationX = 0;
    private float _camRotationY = 0;
    private float deltaCamRotationX = 0;
    private Quaternion _originalCamRotation;
    private Quaternion _playerRotation;
    private CharacterController _characterController;
    private Animator _animator;

    [FormerlySerializedAs("_canMove")] public bool canMove = true;
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

    // public void PlayWalkAnim()
    // {
    //     SetAnim("Walk");
    // }

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
    
    // Based on Roblox's ShiftLock feature (idk if other games have it) (also cuz i use a laptop so RMB is annoying)
    private void CheckShiftLock()
    {
        // Can't actually be Shift because Shift is to sprint
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            ToggleShiftLockMode(!_inShiftlock);
        }
    }

    private void ToggleShiftLockMode(bool shiftLockEnabled)
    {
        _inShiftlock = shiftLockEnabled;
        if (_inShiftlock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            // Later, change this to a ShiftLock icon
            Cursor.visible = false;
            _originalCamRotation = cameraPivot.rotation;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None; 
            Cursor.visible = true;
        }
    }

    private void ManageAnimations()
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

    private void MovementInShiftLock()
    {
        Vector3 forward = playerCamera.transform.TransformDirection(Vector3.forward);
        Vector3 right = playerCamera.transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
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

        if (Input.GetButton("Jump") && canMove && _characterController.isGrounded)
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

        // if (Input.GetKey(KeyCode.R) && _canMove)
        // {
        //     _characterController.height = crouchHeight;
        //     walkSpeed = crouchSpeed;
        //     runSpeed = crouchSpeed;
        //
        // }
        // else
        {
            _characterController.height = defaultHeight;
            walkSpeed = 6f;
            runSpeed = 12f;
        }

        _characterController.Move(_moveDirection * Time.deltaTime);
        
        if (canMove)
        {
            // CANT FIGURE OUT WHY THE CAMERA ROTATES ON AN ANGLE LIKE A SLANTED CIRCLE
            deltaCamRotationX += Input.GetAxis("Mouse X") * lookSpeed;
            cameraPivot.rotation = Quaternion.Euler(0f, _originalCamRotation.y * deltaCamRotationX, 0f);
            DebugScript.BetterDebug(cameraPivot.rotation);
            // cameraPivot.rotation.Set();
            // cameraPivot.LookAt(transform);
            characterTransform.rotation = Quaternion.Slerp(characterTransform.rotation, Quaternion.LookRotation(cameraPivot.right), Time.deltaTime * 10f);
        }
    }

    private void MovementNormal()
    { 
        Vector3 forward = playerCamera.transform.TransformDirection(Vector3.forward);
        Vector3 right = playerCamera.transform.TransformDirection(Vector3.right);

        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        
        float curSpeedX = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? (isRunning ? runSpeed : walkSpeed) * Input.GetAxis("Horizontal") : 0;
        
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

        if (Input.GetButton("Jump") && canMove && _characterController.isGrounded)
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

        // if (Input.GetKey(KeyCode.R) && _canMove)
        // {
        //     _characterController.height = crouchHeight;
        //     walkSpeed = crouchSpeed;
        //     runSpeed = crouchSpeed;
        //
        // }
        // else
        {
            _characterController.height = defaultHeight;
            walkSpeed = 6f;
            runSpeed = 12f;
        }

        _characterController.Move(_moveDirection * Time.deltaTime);

        if (canMove)
        {
            if (Input.GetMouseButton(1))
            {
                _camRotationX += Input.GetAxis("Mouse X") * lookSpeed;
                _camRotationY += Input.GetAxis("Mouse Y") * lookSpeed;
            }

            _camRotationY = Mathf.Clamp(_camRotationY, -lookYLimit, lookYLimit);

            cameraPivot.transform.localRotation = Quaternion.Euler(0, _camRotationX, _camRotationY);
            
            // Need to calculate _playerRotationY based on camera's current rotation
            if (Input.GetAxis("Horizontal") >= 0.1f)
            {
                _playerRotation = Quaternion.LookRotation(cameraPivot.forward *-1, Vector3.up);
            } else if (Input.GetAxis("Horizontal") <= -0.1f)
            {
                _playerRotation = Quaternion.LookRotation(cameraPivot.forward, Vector3.up);
            } else if (Input.GetAxis("Vertical") >= 0.1f)
            {
                _playerRotation = Quaternion.LookRotation(cameraPivot.right, Vector3.up);
            } else if (Input.GetAxis("Vertical") <= -0.1f)
            {
                _playerRotation = Quaternion.LookRotation(cameraPivot.right * -1, Vector3.up);
            }
            
            // Idk any better way to do it off the top of my mind
            if (Input.GetAxis("Horizontal") >= 0.1f && Input.GetAxis("Vertical") >= 0.1f)
            {
                _playerRotation = Quaternion.LookRotation(cameraPivot.right + (cameraPivot.forward * -1), Vector3.up);
            } else if (Input.GetAxis("Horizontal") >= 0.1f && Input.GetAxis("Vertical") <= -0.1f)
            {
                _playerRotation = Quaternion.LookRotation((-1 * cameraPivot.right) + (cameraPivot.forward * -1), Vector3.up);
            } else if (Input.GetAxis("Horizontal") <= -0.1f && Input.GetAxis("Vertical") >= 0.1f)
            {
                _playerRotation = Quaternion.LookRotation(cameraPivot.right + (cameraPivot.forward), Vector3.up);
            } else if (Input.GetAxis("Horizontal") <= -0.1f && Input.GetAxis("Vertical") <= -0.1f)
            {
                _playerRotation = Quaternion.LookRotation((cameraPivot.right * -1) + (cameraPivot.forward), Vector3.up);
            }
            
            // Spherical lerp (treats vectors as directions instead of positions) what the physics
            characterTransform.rotation = Quaternion.Slerp(characterTransform.rotation,_playerRotation, Time.deltaTime * 10f);
        }
    }

    // Manages Player movement (and camera rotation) and checks for ShiftLock mode
    private void ManageMovement()
    {
        CheckShiftLock();
        if (_inShiftlock)
        {
            MovementInShiftLock();
        }
        else
        {
            MovementNormal();
        }
    }
    
    private void Update()
    {
        ManageMovement();
        ManageAnimations();
    }
}