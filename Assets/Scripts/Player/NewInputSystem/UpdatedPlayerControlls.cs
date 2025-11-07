using UnityEngine;
using UnityEngine.InputSystem;

public class UpdatedPlayerControlls : MonoBehaviour
{
    Rigidbody rb;

    [Header("Movement")]
    [SerializeField] float moveSpeed = 15;
    [SerializeField] float groundDrag = 5;
    InputSystem_Actions inputActs;
    Vector3 moveDirection;
    [SerializeField] float jumpForce = 12;
    bool spaceBeingHeld;
    float currentGrav = -9.81f;
    float normalGrav = -9.81f;
    float jumpGrav = -5;
    float gravLerpSpeed = 15;

    [Header("Ground Check")]
    [SerializeField] LayerMask groundMask;
    float playerHeight = 1;
    bool isGrounded;


    private void Awake() => rb = GetComponent<Rigidbody>();

    #region // Enable Disable InputAction
    private void OnEnable()
    {
        inputActs = new InputSystem_Actions();
        inputActs.Player.Enable();
        
        // not subscribing Move to PlayerInput function for movement as makes it very slow and
        // doesnt update move direction based on direction facing
        inputActs.Player.Jump.performed += ctx => Jump();
        inputActs.Player.Jump.canceled += ctx => EndJump();
    }
    private void OnDisable() => inputActs.Disable();
    #endregion

    private void Update()
    {
        // Player Movement
        PlayerInput();
        LimitVelocity();
        JumpPhysics();

        // Ground Check
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight, groundMask);

        // Handle Drag
       /* if (isGrounded) rb.linearDamping = groundDrag;
        else rb.linearDamping = 0;*/
    }

    void PlayerInput()
    {
        // walk/run
        moveDirection = inputActs.Player.Move.ReadValue<Vector2>();
        moveDirection = transform.forward * moveDirection.y + transform.right * moveDirection.x;
    }

    private void FixedUpdate() => MovePlayer();
    void MovePlayer() => rb.linearVelocity += new Vector3(moveDirection.x, 0, moveDirection.z);

    void LimitVelocity()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }

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
        if (Physics.gravity.y != currentGrav) Physics.gravity = new Vector3(0, currentGrav, 0);
        if (!spaceBeingHeld && currentGrav == normalGrav) return;

        // lerping currentGrav depending on jumpHeld
        if (spaceBeingHeld && currentGrav <= jumpGrav) currentGrav = Mathf.MoveTowards(currentGrav, jumpGrav, gravLerpSpeed * Time.deltaTime);
        else if (!spaceBeingHeld && currentGrav >= normalGrav) currentGrav = Mathf.MoveTowards(currentGrav, normalGrav, gravLerpSpeed * Time.deltaTime);
        else if (!spaceBeingHeld && currentGrav != normalGrav) currentGrav = normalGrav;
        
        Debug.Log(currentGrav);
    }
    #endregion
}
