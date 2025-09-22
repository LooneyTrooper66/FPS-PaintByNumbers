using Unity.Hierarchy;
using UnityEngine;
using System.Collections;

public class PlayerControlls : MonoBehaviour
{
    [Header("General")]

    [SerializeField] CharacterController controller;
    [SerializeField] float speed = 12f;
    float defaultSpeed = 12f;
    [SerializeField] float jumpHeight = 3f;
    [SerializeField] float gravity = -9.81f;
    float defaultGravity = -9.81f;

    [Header("Camera Effects")]

    [SerializeField] Transform cam;
    [SerializeField] Vector3 camRunPos;
    [SerializeField] Quaternion camRunRotation;
    [SerializeField] Vector3 camSlidePos;
    [SerializeField] Quaternion camSlideRotation;

    [Header("GroundCheck")]

    [SerializeField] Transform groundCheck;
    [SerializeField] float groundDist = 0.4f;
    [SerializeField] LayerMask groundMask;
    [SerializeField] LayerMask wallMask;

    Vector3 velocity;
    private MovementState moveState;
    bool isGrounded; // use 'InAir' instead when to do with things that cant be done if on the ground OR on a wall
    bool justLanded = false; bool justDashed = false; // use to check if crouch in time to slide

    public enum MovementState
    {
        Running,
        Sliding, // cant dash while sliding
        InAir,
        OnWall
    }


    private void Start()
    {
        defaultGravity = gravity;
        defaultSpeed = speed;
        moveState = MovementState.Running;
    }

    private void Update()
    {
        // move, dash, jump, air dash, land faster, wall slide, wall jump, slide
        Move();
        Jump();
    }

    void Move() // move, dash, slide
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // slide
        if (Input.GetKey(KeyCode.LeftControl) && (justLanded || justDashed))
        {
            moveState = MovementState.Sliding;
            // lerp cam to -0.1, tilt x -10
            // move faster but only in forward direction of when started sliding
        }

        if (Input.GetKeyUp(KeyCode.LeftControl) && moveState == MovementState.Sliding)
        {
            moveState = MovementState.Running;
            // lerp cam back to normal
            // speed back to normal, 
        } 


        // landed check for slide
        if (isGrounded && !justLanded) justLanded = true;
        if (justLanded) StartCoroutine(LandedTimer());
        if (justLanded && !isGrounded) { StopCoroutine(LandedTimer()); justLanded = false; }
    }

    void Jump() // jump, dash/air dash, land faster
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDist, groundMask);
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2;
        }

        // jump
        if(Input.GetButtonDown("Jump") && isGrounded && moveState != MovementState.Sliding)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            moveState = MovementState.InAir;
        }

        // ground slam ish?
        if (Input.GetKey(KeyCode.LeftControl) && !isGrounded) { gravity *= 1.03f; }
        else { gravity = defaultGravity; moveState = MovementState.Running; }

        // dash/air dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && (moveState != MovementState.Sliding || moveState != MovementState.OnWall)) StartCoroutine(DashTimer());
    }

    IEnumerator DashTimer()
    {
        speed = speed * 2;
        justDashed = true;
        yield return new WaitForSeconds(1);
        speed = defaultSpeed;
        justDashed = false;
    }

    void WallControls() // wall slide, wall jump
    {
        // wall slide
        // lock y value, tilt camera sideways depending on which side of character the wall is on

        // wall jump
        if (moveState == MovementState.OnWall)
        {
            // only able to wall jump to break wall slide, cant walljump otherwise

        }
    }

    IEnumerator LandedTimer()
    {
        yield return new WaitForSeconds(1);
        justLanded = false;
    }
}
