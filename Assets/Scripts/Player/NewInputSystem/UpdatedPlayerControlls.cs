using UnityEngine;
using UnityEngine.InputSystem;

public class UpdatedPlayerControlls : MonoBehaviour
{
    Rigidbody rb;

    [Header("Ground Movement")]
    [SerializeField] float runSpeed = 30;
    [SerializeField] float slideSpeed = 40;
    float moveSpeed;
    InputSystem_Actions inputActs;
    Vector3 moveDirection;
    [SerializeField] float dashForce = 20;
    bool canDash = true;
    bool justDashed = false;
    float dashCooldown = 1;
    [SerializeField] float slideForce = 20;
    [SerializeField] GameObject playerHead;
    bool slideBeingHeld = false;
    bool canSlide = false;
    bool isSliding = false;
    float maxSlideTime = 3;
    float slideTimer = 0;
    float startYHeight = 0.7f;
    float slideYHeight = 0.2f;
    bool isRunning = false;

    [Header("Air Movement")]
    [SerializeField] float jumpForce = 15;
    bool spaceBeingHeld;
    ConstantForce thisGrav;
    float currentGrav = -9.81f;
    float normalGrav = -9.81f;
    float jumpGrav = -4.5f;
    float gravLerpSpeed = 15;
    bool justLanded;

    [Header("Ground Check")]
    [SerializeField] LayerMask groundMask;
    float playerHeight = 1.1f;
    bool isGrounded;
    public MoveState state;
    public enum MoveState
    {
        run, 
        slide,
        air
    }


    [Header("Audio/SFX")]
    public AudioSource playerSource;
    public AudioClip[] footsteps;
    public AudioClip onJump;
    public AudioClip forceDown;
    public AudioClip onLand;
    public AudioClip slideRelease;
    public AudioSource sustainSource;
    bool sustainPlaying = false;
    float stepSoundTimer = 0;


    private void Awake() { rb = GetComponent<Rigidbody>(); thisGrav = GetComponent<ConstantForce>(); }

    #region // Enable Disable InputAction
    private void OnEnable()
    {
        inputActs = new InputSystem_Actions();
        inputActs.Player.Enable();

        // not subscribing Move to PlayerInput function for movement as makes it very slow and
        // doesnt update move direction based on direction facing
        inputActs.Player.Slide.started += ctx => SlideHeld();
        inputActs.Player.Slide.canceled += ctx => SlideReleased();
        inputActs.Player.Jump.performed += ctx => Jump();
        inputActs.Player.Jump.canceled += ctx => EndJump();
        inputActs.Player.Dash.performed += ctx => CheckCanDash();
        inputActs.Player.Move.started += ctx => StartRunSFX();
        inputActs.Player.Move.canceled += ctx => StopRunSFX();
    }
    private void OnDisable() => inputActs.Disable();
    #endregion

    private void Update()
    {
        // Player Movement
        if (RoundTimer.i.gamePlaying)
        {
            PlayerInput();
            JumpPhysics();
            SetStates();
        }

        // Ground Check
        isGrounded = Physics.Raycast(transform.position, Vector3.down, playerHeight, groundMask);
        if (justDashed || justLanded || state == MoveState.air) canSlide = true; else canSlide = false;

        // play footsteps
        if (isRunning && isGrounded && state == MoveState.run && RoundTimer.i.gamePlaying)
        {
            stepSoundTimer += Time.deltaTime;
            if (stepSoundTimer >= 0.25f)
            {
                playerSource.pitch = Random.Range(0.9f, 1);
                playerSource.PlayOneShot(footsteps[Random.Range(0, 2)]);
                stepSoundTimer = 0;
            }
        }
    }
    void PlayerInput()
    {
        // set speed
        if (state == MoveState.slide && moveSpeed != slideSpeed) moveSpeed = slideSpeed;
        else if (state != MoveState.slide && moveSpeed != runSpeed) moveSpeed = runSpeed;

        // run
        moveDirection = inputActs.Player.Move.ReadValue<Vector2>();
        moveDirection = transform.forward * moveDirection.y + transform.right * moveDirection.x;

        // slide
        if (slideBeingHeld)
        {
            slideTimer += Time.deltaTime;
            if (slideTimer <= maxSlideTime) Slide();
            else SetStateRun();
        }
        else if (!slideBeingHeld && state == MoveState.slide) SetStateRun(); 
    }

    #region // set states
    void SetStates()
    {
        if (!isGrounded && state != MoveState.air) state = MoveState.air;
        else if (isGrounded && isSliding && state != MoveState.slide) { state = MoveState.slide; justLanded = true; Invoke(nameof(ResetJustLanded), 1); }
        else if (isGrounded && state == MoveState.air) { state = MoveState.run; playerSource.PlayOneShot(onLand); justLanded = true; Invoke(nameof(ResetJustLanded), 1); }
    }
    void SetStateRun() 
    { 
        state = MoveState.run; 
        playerHead.transform.localPosition = new Vector3(0, startYHeight, 0);
        if (sustainPlaying)
        {
            sustainSource.Pause();
            playerSource.pitch = 1;
            playerSource.PlayOneShot(slideRelease);
            sustainPlaying = false;
        }
        isSliding = false; slideTimer = 0; slideBeingHeld = false; 
    }
    void ResetJustLanded() => justLanded = false;
    void SlideHeld() { slideBeingHeld = true; playerSource.PlayOneShot(forceDown); }
    void SlideReleased() 
    { 
        slideBeingHeld = false; 
        if (sustainPlaying)
        {
            sustainSource.Pause();
            playerSource.pitch = 1;
            playerSource.PlayOneShot(slideRelease);
        }
        sustainPlaying = false; 
    }
    void StartRunSFX() => isRunning = true;
    void StopRunSFX() => isRunning = false;
    #endregion

    #region // run
    private void FixedUpdate() => MovePlayer();
    void MovePlayer() { rb.linearVelocity += new Vector3(moveDirection.x, 0, moveDirection.z); LimitVelocity(); }
    void LimitVelocity()
    {
        Vector3 flatVal = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        if (flatVal.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVal.normalized * moveSpeed;
            rb.linearVelocity = new Vector3(limitedVel.x, rb.linearVelocity.y, limitedVel.z);
        }
    }
    #endregion

    #region // dash
    void CheckCanDash()
    {
        if (state == MoveState.slide || canDash == false) return; // cant dash if already sliding, cant dash if cooldown not over
        canDash = false;
        justDashed = true;
        playerSource.pitch = Random.Range(0.8f, 1.1f);
        playerSource.PlayOneShot(onJump);
        rb.AddForce(transform.forward * dashForce, ForceMode.Impulse);

        Invoke(nameof(ResetJustDashed), 1);
        Invoke(nameof(ResetDashBool), dashCooldown);
    }
    void ResetJustDashed() => justDashed = false;
    void ResetDashBool() => canDash = true; 
    #endregion

    #region // slide
    void Slide()
    {
        if (canSlide) isSliding = Physics.Raycast(transform.position, Vector3.down, 8, groundMask);

        if (isSliding)
        {
            if (!isGrounded)
            {
                rb.AddForce(-transform.up * 30);
            }
            else if (isGrounded && justLanded)
            {
                if (!sustainPlaying) { playerSource.PlayOneShot(onLand); sustainSource.Play(); sustainPlaying = true; }
                state = MoveState.slide;
                justLanded = false;
                playerHead.transform.localPosition = new Vector3(0, slideYHeight, 0);
            }
            else rb.AddForce(moveDirection.normalized * slideForce);
        }
    }
    #endregion

    #region // variable jump
    void Jump()
    {
        if (!isGrounded) return;
        spaceBeingHeld = true;
        playerSource.pitch = Random.Range(0.8f, 1.1f);
        playerSource.PlayOneShot(onJump);
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