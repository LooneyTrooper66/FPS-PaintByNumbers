using UnityEngine;
using UnityEngine.InputSystem;

public class UpdatedPlayerControlls : MonoBehaviour
{
    Rigidbody rb;

    [Header("Ground Movement")]
    [SerializeField] float walkSpeed = 30;
    [SerializeField] float dashSpeed = 50;
    float moveSpeed;
    InputSystem_Actions inputActs;
    Vector3 moveDirection;

    float dashForce = 10;
    bool canDash = true;
    float dashCooldown = 2;

    bool canSlide;
    bool isSliding;
    
    [Header("Air Movement")]
    [SerializeField] float jumpForce = 15;
    bool spaceBeingHeld;
    ConstantForce thisGrav;
    float currentGrav = -9.81f;
    float normalGrav = -9.81f;
    float jumpGrav = -4.5f;
    float gravLerpSpeed = 15;

    [Header("Ground Check")]
    [SerializeField] LayerMask groundMask;
    float playerHeight = 1.2f;
    bool isGrounded;

    public MoveState state;
    public enum MoveState
    {
        walkRun, 
        sliding,
        air
    }


    private void Awake() { rb = GetComponent<Rigidbody>(); thisGrav = GetComponent<ConstantForce>(); }

    #region // Enable Disable InputAction
    private void OnEnable()
    {
        inputActs = new InputSystem_Actions();
        inputActs.Player.Enable();

        // not subscribing Move to PlayerInput function for movement as makes it very slow and
        // doesnt update move direction based on direction facing
        inputActs.Player.Move.performed += ctx => SetStateWalkRun();
        inputActs.Player.Slide.performed += ctx => SetSlideState();
        inputActs.Player.Slide.canceled += ctx => SetStateWalkRun();
        inputActs.Player.Jump.performed += ctx => Jump();
        inputActs.Player.Jump.canceled += ctx => EndJump();
        inputActs.Player.Dash.performed += ctx => CheckCanDash();
    }
    private void OnDisable() => inputActs.Disable();
    #endregion

    private void Update()
    {
        // Player Movement
        PlayerInput();
        JumpPhysics();

        // Ground Check
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight, groundMask);
        if (!isGrounded && state != MoveState.air) state = MoveState.air;
    }

    #region // set states
    void SetStateWalkRun() => state = MoveState.walkRun;
    void SetSlideState() => state = MoveState.sliding;
    #endregion

    void PlayerInput()
    {
        // walk/run
        moveDirection = inputActs.Player.Move.ReadValue<Vector2>();
        moveDirection = transform.forward * moveDirection.y + transform.right * moveDirection.x;

        // slide

    }

    #region // walk/run
    private void FixedUpdate() => MovePlayer();
    void MovePlayer() { rb.linearVelocity += new Vector3(moveDirection.x, 0, moveDirection.z); LimitVelocity(); }
    void LimitVelocity()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }
    #endregion

    #region // dash
    void CheckCanDash()
    {
        if (state == MoveState.sliding) return; // cant dash if already sliding
        canDash = false;
        canSlide = true;
        rb.AddForce(transform.forward * dashForce, ForceMode.Impulse);

        Invoke(nameof(ResetDashBool), dashCooldown);
    }
    void ResetDashBool() => canDash = true; 
    #endregion

    #region // variable jump
    void Jump()
    {
        if (!isGrounded) return;
        spaceBeingHeld = true;
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }
    void EndJump() => spaceBeingHeld = false;
    void JumpPhysics()
    {
        // set gravity to currentGrav if not already
        if (thisGrav.force.y != currentGrav) thisGrav.force = new Vector3(0, currentGrav, 0);
        if (!spaceBeingHeld && currentGrav == normalGrav) return;

        // lerping currentGrav depending on jumpHeld
        if (spaceBeingHeld && currentGrav <= jumpGrav) currentGrav = Mathf.MoveTowards(currentGrav, jumpGrav, gravLerpSpeed * Time.deltaTime);
        else if (!spaceBeingHeld && currentGrav >= normalGrav) currentGrav = Mathf.MoveTowards(currentGrav, normalGrav, gravLerpSpeed * Time.deltaTime);
        else if (!spaceBeingHeld && currentGrav != normalGrav) currentGrav = normalGrav;
    }
    #endregion
}
