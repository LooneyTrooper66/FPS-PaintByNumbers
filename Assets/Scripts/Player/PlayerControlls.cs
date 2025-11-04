using System.Collections;
using UnityEngine;

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
    [SerializeField] float camRunPos;
    [SerializeField] float camSlidePos;

    [Header("GroundCheck")]

    [SerializeField] Transform groundCheck;
    [SerializeField] float groundDist = 0.4f;
    [SerializeField] LayerMask groundMask;
    [SerializeField] LayerMask wallMask;

    Vector3 velocity;
    public MovementState moveState;
    bool isGrounded; // use 'InAir' instead when to do with things that cant be done if on the ground OR on a wall
    [HideInInspector] public bool justLanded = false; bool justDashed = false; // use to check if crouch in time to slide

    public enum MovementState  // IMPORTANT: NEVER SET MOVESTATE IN UPDATE OR GETKEY
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
    }

    private void Update()
    {
        // move, dash, jump, air dash, land faster, wall slide, wall jump, slide
        GroundMovement();
        AirMovement();
    }

    void GroundMovement() // move, dash, slide
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // slide
        if (Input.GetKey(KeyCode.LeftControl) && (justLanded || justDashed) && moveState != MovementState.Sliding)
        {
            moveState = MovementState.Sliding;
            speed = defaultSpeed * 3f;

            cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, -0.1f, cam.transform.localPosition.z);
            //Mathf.Lerp(cam.localPosition.y, camSlidePos, 0.2f);
        }

        if ((Input.GetKeyUp(KeyCode.LeftControl)) && moveState == MovementState.Sliding)
        {
            moveState = MovementState.Running;
            speed = defaultSpeed;

            cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, 0.7f, cam.transform.localPosition.z);
            //Mathf.Lerp(cam.localPosition.y, camRunPos, 0.2f);
        }
    }

    void AirMovement() // jump, dash/air dash, land faster
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDist, groundMask);
        if (isGrounded && velocity.y < 0) velocity.y = -2;

        // jump
        if (Input.GetButtonDown("Jump") && moveState == MovementState.Running)
        {
            moveState = MovementState.InAir;
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        else if(Input.GetButtonDown("Jump") && moveState == MovementState.Sliding)
        {
            moveState = MovementState.InAir;
            speed = defaultSpeed;
            cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, 0.7f, cam.transform.localPosition.z);
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        if (!isGrounded) moveState = MovementState.InAir; 
        if (isGrounded && moveState == MovementState.InAir) { StopCoroutine(LandedTimer()); StartCoroutine(LandedTimer()); }

        // ground slam ish?
        if (Input.GetKey(KeyCode.LeftControl) && !isGrounded) { gravity = -200f; }
        else if (isGrounded && moveState != MovementState.Sliding) { gravity = defaultGravity; moveState = MovementState.Running; }

        // dash/air dash
        if (Input.GetKeyDown(KeyCode.LeftShift) && (moveState != MovementState.Sliding || moveState != MovementState.OnWall)) StartCoroutine(DashTimer());
    }

    #region // checks to make the slide work + dash functionality
    IEnumerator DashTimer()
    {
        speed = defaultSpeed * 3;
        justDashed = true;
        yield return new WaitForSeconds(0.5f);
        if(moveState != MovementState.Sliding) speed = defaultSpeed;
        justDashed = false;
    }

    IEnumerator LandedTimer()
    {
        justLanded = true;
        yield return new WaitForSeconds(1);
        justLanded = false;
    }
    #endregion
}
