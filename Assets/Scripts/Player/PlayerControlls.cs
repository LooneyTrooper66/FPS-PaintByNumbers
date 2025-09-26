using System.Collections;
using UnityEditor.ShaderGraph;
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
    [SerializeField] float camRunRotation;
    [SerializeField] float camSlidePos;
    [SerializeField] float camSlideRotation;

    [Header("GroundCheck")]

    [SerializeField] Transform groundCheck;
    [SerializeField] float groundDist = 0.4f;
    [SerializeField] LayerMask groundMask;
    [SerializeField] LayerMask wallMask;

    Vector3 velocity;
    [HideInInspector] public MovementState moveState;
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
        Move();
        Jump();
    }

    void Move() // move, dash, slide
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        // lock to only move forward by speed when sliding

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // slide
        if (Input.GetKey(KeyCode.LeftControl) && (justLanded || justDashed) && moveState != MovementState.Sliding)
        {
            Debug.Log("do the slide thing");
            moveState = MovementState.Sliding;
            speed = defaultSpeed * 2.5f;

            cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, -0.1f, cam.transform.localPosition.z);
            //Mathf.Lerp(cam.position.y, camSlidePos, 0.2f);
            //Mathf.Lerp(cam.rotation.x, camSlideRotation, 0.2f);
            // lerp cam to -0.1, tilt x -10
            // move faster but only in forward direction of when started sliding
        }

        if (Input.GetKeyUp(KeyCode.LeftControl) && moveState == MovementState.Sliding)
        {
            Debug.Log("control key up");
            moveState = MovementState.Running;
            speed = defaultSpeed;
            cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, 0.7f, cam.transform.localPosition.z);

            //Mathf.Lerp(cam.position.y, camRunPos, 0.2f);
            //Mathf.Lerp(cam.rotation.x, camRunRotation, 0.2f);
        } 
    }

    void Jump() // jump, dash/air dash, land faster
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDist, groundMask);
        if (isGrounded && velocity.y < 0) velocity.y = -2;

        // jump
        if (Input.GetButtonDown("Jump") && moveState == MovementState.Running) // chaning isGrounded to movestate running to try fix something
        {
            moveState = MovementState.InAir;
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
        speed = defaultSpeed;
        justDashed = false;
    }

    IEnumerator LandedTimer()
    {
        justLanded = true;
        yield return new WaitForSeconds(1);
        justLanded = false;
    }
    #endregion

    void WallControls() // wall slide, wall jump
    {
        // wall slide
        // lock y value, lock rotation to only be 90 degrees to wall, tilt camera sideways depending on which side of character the wall is on

        // wall jump
        if (moveState == MovementState.OnWall)
        {
            // only able to wall jump to break wall slide, cant walljump otherwise

        }
    }
}
