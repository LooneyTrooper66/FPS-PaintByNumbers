using UnityEngine;
using UnityEngine.InputSystem;

public class UpdatedPlayerControlls : MonoBehaviour
{
    Rigidbody rb;

    [Header("Movement")]
    [SerializeField] float moveSpeed;
    [SerializeField] float groundDrag;
    [SerializeField] InputAction movement;
    Vector3 moveDirection;

    [Header("Ground Check")]
    float playerHeight = 0.7f;
    [SerializeField] LayerMask groundMask;
    bool isGrounded;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    #region // Enable Disable InputAction
    private void OnEnable() => movement.Enable();
    private void OnDisable() => movement.Disable();
    #endregion

    private void Update()
    {
        // Player Movement
        LimitVelocity();

        // Ground Check
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight, groundMask);

        // Handle Drag
        if (isGrounded) rb.linearDamping = groundDrag;
        else rb.linearDamping = 0;
    }

    private void FixedUpdate() => MovePlayer();

    void MovePlayer()
    {
        moveDirection = movement.ReadValue<Vector2>();
        moveDirection = transform.forward * moveDirection.y + transform.right * moveDirection.x;
        rb.linearVelocity += new Vector3(moveDirection.x,0, moveDirection.z);       
    }

    void LimitVelocity()
    {
        Vector3 flatVel = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }
}
