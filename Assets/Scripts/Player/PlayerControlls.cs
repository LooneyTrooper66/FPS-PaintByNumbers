using Unity.Hierarchy;
using UnityEngine;

public class PlayerControlls : MonoBehaviour
{
    [SerializeField] CharacterController controller;
    [SerializeField] float speed = 12f;
    [SerializeField] float jumpHeight = 3f;
    float defaultGravity = -9.81f;
    float gravity = -9.81f;

    [SerializeField] Transform groundCheck;
    [SerializeField] float groundDist = 0.4f;
    [SerializeField] LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;


    private void Update()
    {
        // move, dash, crouch, jump, slide, wall jump, wall slide
        Move();
        Jump();
    }

    void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void Jump()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDist, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2;
        }

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // ground slam ish?
        if (Input.GetKey(KeyCode.LeftControl) && !isGrounded) { gravity *= 1.05f; }
        else { gravity = defaultGravity; }
    }

    void Slide()
    {

    }
}
