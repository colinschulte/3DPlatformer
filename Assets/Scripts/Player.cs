using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Animator animator;
    [SerializeField] private UIMovement uiMovement;
    private PauseGame pause;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private ParticleSystem StepCloud;
    [SerializeField] private ParticleSystem StompCloud;
    private CinemachineFreeLook freelookCam;
    private bool firstUpdate = true;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float maxSpeed;
    [SerializeField] private float maxAcceleration;
    [SerializeField] private float maxAirAcceleration;
    [SerializeField] private float maxDeceleration;
    [SerializeField] private float maxAirDeceleration;
    [SerializeField] private float defaultMaxSpeed;
    [SerializeField] private float defaultMaxAccel;
    [SerializeField] private float defaultMaxAirAccel;
    [SerializeField] private float defaultMaxDecel;
    [SerializeField] private float defaultMaxAirDecel;
    [SerializeField] private Vector3 velocity;
    public Vector3 moveDirection;
    private Vector3 lastMoveDirection;
    public CharacterController controller;

    private Vector3 lastForward;
    private Vector3 lastJumpForward;
    public bool canMove;
    public bool canTurn;

    public float jumpForce;
    public float gravityScale;

    [SerializeField] private float coyoteTime;
    private float coyoteCounter;

    public bool canJump = true;
    [SerializeField] private float jumpTime;
    [SerializeField] private float jumpFactor = 1f;
    [SerializeField] private float fallFactor = 0.4f;
    [SerializeField] private int jumpCounter = 1;
    private bool firstJumpActive;
    private bool secondJumpActive;
    [SerializeField] private bool isDoubleJumping;
    [SerializeField] private bool isTripleJumping;
    private float secondJumpTimer;
    private float thirdJumpTimer;
    [SerializeField] private float hoverTime;
    [SerializeField] private float hoverTimer;
    public bool canHover;
    public bool isHovering;
    public bool hitHead;

    [SerializeField] private float bounceForce;
    public bool bounceStart = false;
    public bool isBouncing = false;

    [SerializeField] private bool jumpPressed;
    [SerializeField] private bool jumpReleased;
    [SerializeField] private bool dashPressed;
    [SerializeField] private bool dashReleased;
    [SerializeField] private bool crouchPressed;
    [SerializeField] private bool crouchReleased;
    [SerializeField] private bool climbPressed;
    [SerializeField] private bool climbReleased;
    [SerializeField] private bool radarPressed;
    [SerializeField] private bool radarReleased;

    [SerializeField] private bool isCrouching;
    [SerializeField] private bool isSkidding;
    [SerializeField] private float skidTime;
    [SerializeField] private float skidCounter;
    [SerializeField] private bool isStopped;
    [SerializeField] private bool isBackflipping;
    [SerializeField] private bool isLongJumping;
    [SerializeField] private bool canSomersault;
    [SerializeField] private float somersaultTime;
    [SerializeField] private float somersaultCounter;
    [SerializeField] private bool isSomersaulting;
    public bool enemyStomped;
    [SerializeField] private int groundPoundPower;
    [SerializeField] private bool canGroundPound;
    public bool isGroundPounding;
    [SerializeField] private float groundPoundHangtime;
    [SerializeField] private float groundPoundHangcount;

    public bool isHanging;
    [SerializeField] private bool wasHanging;
    public bool canHang;
    [SerializeField] private float ledgeClimbFactor;
    public float hangTime;
    private float hangCounter;
    [SerializeField] private float hangOffsetX;
    [SerializeField] private float hangOffsetY;
    [SerializeField] private Vector3 hangPos;
    [SerializeField] private Vector3 pullUpEnd;

    [SerializeField] public float UIWait;
    public float cheeseTimer;
    public float brickTimer;
    public float crackerTimer;
    public float healthTimer;

    public bool isClimbing;
    private bool canClimb;
    public Climb climbObject;
    [SerializeField] private float climbSpeed;
    [SerializeField] private float climbPull;
    [SerializeField] private float climbAcceleration;
    [SerializeField] private RaycastHit fwdHit;
    [SerializeField] private Vector3 lastNormal;
    [SerializeField] private float climbOffsetX;
    [SerializeField] private float climbOffsetY;
    [SerializeField] private Vector3 climbPos;
    [SerializeField] private float climbReach;

    [SerializeField] public bool isNearSign = false;
    private bool isReading = true;
    [SerializeField] public GameObject currentSign;
    private ReadSign readSign;

    private Vector3 wallNormal;
    [SerializeField] private float wallPushback;
    [SerializeField] private bool canWallJump;
    [SerializeField] private bool isWallJumping;
    [SerializeField] private float wallJumpTime;
    private float wallJumpCounter;
    //private Vector3 lastWallNormal;

    [SerializeField] private float dashSpeed;
    private bool canDash = true;
    [SerializeField] private bool isDashing = false;
    [SerializeField] private float dashTime;
    private float dashCounter;
    [SerializeField] private float dashCooldown;
    private float dashCooldownCount;

    [SerializeField] private bool canRadar;
    [SerializeField] private float radarTime;
    private float radarCounter;

    [SerializeField] private Camera MainCamera;
    [SerializeField] private Transform cameraTransform;

    public float rotateSpeed;

    public GameObject playerModel;
    [SerializeField] private GameObject dashModel;
    [SerializeField] private GameObject arrow;
    [SerializeField] public GameObject interactArrow;

    public float knockbackForce;
    public float knockbackTime;
    private float knockbackCounter;

    public PlayerControls playerControls;

    private InputAction move;
    private InputAction jump;
    private InputAction crouch;
    private InputAction dash;
    private InputAction climb;
    private InputAction centerCamera;
    private InputAction radar;

    [SerializeField] private bool isSliding;
    [SerializeField] private Vector3 slopeSlideVelocity;

    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource dashSound;
    private Vector3 surfaceNormal;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        pause = FindObjectOfType<PauseGame>();
        dashCounter = dashTime;
        lastForward = playerModel.transform.forward;
        lastJumpForward = playerModel.transform.forward;

    }

    private void OnEnable()
    {
        move = playerControls.Player.Move;
        move.Enable();
        jump = playerControls.Player.Jump;
        jump.Enable();
        crouch = playerControls.Player.Crouch;
        crouch.Enable();
        dash = playerControls.Player.Dash;
        dash.Enable();
        climb = playerControls.Player.Climb;
        climb.Enable();
        radar = playerControls.Player.Radar;
        radar.Enable();
        centerCamera = playerControls.Player.RecenterCamera;
        centerCamera.Enable();
    }

    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
        crouch.Disable();
        dash.Disable();
        climb.Disable();
        radar.Disable();
        centerCamera.Disable();
    }

    private void Update()
    {
        if (jump.triggered)
        {
            jumpPressed = true;
        }
        else if (jump.WasReleasedThisFrame())
        {
            jumpReleased = true;
        }

        if (dash.triggered)
        {
            dashPressed = true;
        }
        else if (dash.WasReleasedThisFrame())
        {
            dashReleased = true;
        }

        if (crouch.triggered)
        {
            crouchPressed = true;
        }
        else if (crouch.WasReleasedThisFrame())
        {
            crouchReleased = true;
        }

        if (climb.triggered)
        {
            climbPressed = true;
        }
        else if (climb.WasReleasedThisFrame())
        {
            climbReleased = true;
        }

        if (radar.triggered)
        {
            radarPressed = true;
        }
        else if (radar.WasReleasedThisFrame())
        {
            radarReleased = true;
        }

        if (centerCamera.triggered)
        {
            FindObjectOfType<CenterCamera>().centerCamera(true);
        }
        else if (centerCamera.WasReleasedThisFrame())
        {
            FindObjectOfType<CenterCamera>().centerCamera(false);
        }
    }

    private void FixedUpdate()
    {
        if (firstUpdate)
        {
            if (levelManager.gameManager != null)
            {
                if (levelManager.gameManager.lastScene == 4)
                {
                    transform.position = new Vector3(39.8f, 3.9f, 57.5f);
                }
                firstUpdate = false;
            }
        }
        if (knockbackCounter <= 0)
        {
            if (!wasHanging)
            {
                //float xStore = velocity.x;
                float yStore = velocity.y;
                //float zStore = velocity.z;
                if (climbPressed)
                {
                    canClimb = true;
                }
                else if (climbReleased)
                {
                    canClimb = false;
                }

                Vector3 LineFwdStart = new Vector3(transform.position.x, transform.position.y, transform.position.z);

                Vector3 LineFwdEnd = Vector3.zero;
                if (isClimbing)
                {
                    climbReach = 0.8f;
                }
                else
                {
                    climbReach = 0.3f;
                }
                LineFwdEnd = new Vector3(transform.position.x, transform.position.y, transform.position.z) + (playerModel.transform.forward * climbReach);
                fwdHit = new RaycastHit();
                Physics.Linecast(LineFwdStart, LineFwdEnd, out fwdHit, LayerMask.GetMask("Climbable"));

                if (fwdHit.collider != null && !isWallJumping && !isHanging)
                {
                    velocity = Vector3.zero;
                    isClimbing = true;
                    climbObject = fwdHit.collider.GetComponent<Climb>();
                    climbPos = new Vector3(fwdHit.point.x, transform.position.y, fwdHit.point.z);
                    Vector3 offset = transform.forward * climbOffsetX + transform.up * climbOffsetY;
                    climbPos += offset;
                    //transform.position = climbPos;
                    wallNormal = fwdHit.normal;
                    playerModel.transform.forward = -wallNormal;
                    lastNormal = fwdHit.normal;
                }
                else
                {
                    isClimbing = false;
                    //Debug.Log("NOT ON WALL");
                }

                //if (isClimbing && !controller.isGrounded)
                if (isClimbing)
                {
                    canDash = false;
                    isDashing = false;
                    isBackflipping = false;
                    isSomersaulting = false;
                    canGroundPound = false;
                    moveDirection = move.ReadValue<Vector2>();
                    if (climbObject.canSideClimb)
                    {
                        moveDirection = (transform.up * moveDirection.y) + (playerModel.transform.right * moveDirection.x);
                    }
                    else
                    {
                        moveDirection = (transform.up * moveDirection.y);
                    }
                    float magnitude = moveDirection.magnitude;
                    magnitude = Mathf.Clamp01(magnitude);
                    moveDirection = moveDirection.normalized;
                    moveDirection = magnitude * climbSpeed * moveDirection;
                    moveDirection += (playerModel.transform.forward * climbPull);
                    canWallJump = true;
                }
                else
                {
                    if (canMove)
                    {
                        moveDirection = move.ReadValue<Vector2>();
                        moveDirection = (transform.forward * moveDirection.y) + (transform.right * moveDirection.x);
                        float magnitude = moveDirection.magnitude;
                        magnitude = Mathf.Clamp01(magnitude);
                        moveDirection = moveDirection.normalized;
                        moveDirection = magnitude * moveSpeed * moveDirection;
                    }
                    moveDirection.y = yStore;

                    if (Vector3.Dot(new Vector3(velocity.x, 0, velocity.z), new Vector3(moveDirection.x, 0, moveDirection.z)) < -0.5 && !isSkidding)
                    {
                        isSkidding = true;
                        skidCounter = skidTime;
                        somersaultCounter = 0;
                    }
                }

                //if on the ground, reset y movement and coyote counter
                if (controller.isGrounded && !isClimbing)
                {
                    if (!pause.gamePaused && MainCamera.isActiveAndEnabled && !isReading)
                    {
                        canMove = true;
                        canTurn = true;
                    }
                    canWallJump = false;
                    if (StepCloud.isStopped)
                    {
                        StepCloud.Play();
                    }
                    if (isGroundPounding)
                    {
                        StompCloud.Play();
                    }
                    isGroundPounding = false;
                    canGroundPound = true;
                    isBackflipping = false;
                    isLongJumping = false;
                    isSomersaulting = false;
                    isDoubleJumping = false;
                    isTripleJumping = false;
                    isBouncing = false;
                    canHover = false;
                    isHovering = false;
                    wasHanging = false;
                    maxSpeed = defaultMaxSpeed;
                    maxAcceleration = defaultMaxAccel;
                    maxAirAcceleration = defaultMaxAirAccel;
                    maxDeceleration = defaultMaxDecel;
                    maxAirDeceleration = defaultMaxAirDecel;
                    gravityScale = 5f;

                    if (slopeSlideVelocity != Vector3.zero)
                    {
                        isSliding = true;
                    }
                    if (isSliding == false)
                    {
                        moveDirection.y = -2f;
                    }

                    coyoteCounter = coyoteTime;

                }
                else
                {
                    coyoteCounter -= Time.fixedDeltaTime;
                }
                if (!controller.isGrounded)
                {
                    StepCloud.Stop();
                }
                //check if Jump is pressed
                if (jumpPressed)
                {
                    if (coyoteCounter > 0f)
                    {
                        canJump = true;
                    }
                    else
                    {
                        canJump = false;
                    }

                    //remove text
                    if (isReading)
                    {
                        readSign = currentSign.GetComponent<ReadSign>();
                        readSign.EndRead();
                        canJump = false;
                        isReading = false;
                    }

                    if (isHanging)
                    {
                        hangCounter = hangTime;
                        canMove = false;
                        canJump = true;
                        jumpFactor = 0.5f;
                        isHanging = false;
                        pullUpEnd = transform.position + (playerModel.transform.forward * 0.5f) + (transform.up * 1f);
                        wasHanging = true;
                    }

                    if (canJump)
                    {
                        lastJumpForward = playerModel.transform.forward;
                        canTurn = false;
                        canHover = true;
                        maxSpeed = defaultMaxSpeed;

                        if (!wasHanging)
                        {
                            if (isCrouching)
                            {
                                if (!isStopped)
                                {
                                    //long jump
                                    jumpFactor = 0.5f;
                                    //maxAirAcceleration = 50f;
                                    velocity += playerModel.transform.forward * 10;
                                    isLongJumping = true;
                                    canMove = true;
                                }
                                else
                                {
                                    //high jump
                                    jumpFactor = 1.3f;
                                    isBackflipping = true;
                                    canMove = true;
                                }
                            }
                            else
                            {
                                if (canSomersault)
                                {
                                    //somersault
                                    jumpFactor = 1.3f;
                                    isSomersaulting = true;
                                }
                                else if (jumpCounter == 1)
                                {
                                    jumpFactor = 1f;
                                    jumpCounter++;
                                    firstJumpActive = true;
                                }
                                else if (jumpCounter == 2 && firstJumpActive && secondJumpTimer > 0f)
                                {
                                    jumpFactor = 1f;
                                    jumpCounter++;
                                    secondJumpTimer = 0f;
                                    firstJumpActive = false;
                                    secondJumpActive = true;
                                    isDoubleJumping = true;
                                }
                                else if (jumpCounter == 3 && secondJumpActive && thirdJumpTimer > 0f)
                                {
                                    jumpFactor = 1.3f;
                                    jumpCounter++;
                                    thirdJumpTimer = 0f;
                                    firstJumpActive = false;
                                    secondJumpActive = false;
                                    isTripleJumping = true;
                                }
                            }
                        }

                        moveDirection.y = jumpForce * jumpFactor;
                        jumpSound.Play();
                        coyoteCounter = 0f;
                    }
                    else if (canWallJump)
                    {
                        velocity = Vector3.zero;
                        if (isClimbing)
                        {
                            wallNormal = lastNormal;
                        }
                        moveDirection = wallNormal * wallPushback;
                        moveDirection.y = jumpForce * jumpFactor;
                        playerModel.transform.forward = wallNormal;
                        jumpSound.Play();
                        wallJumpCounter = wallJumpTime;
                        canWallJump = false;
                        canMove = false;
                        isClimbing = false;
                        isWallJumping = true;
                    }
                }
                if (jumpCounter > 3)
                {
                    jumpCounter = 1;
                }
                if (secondJumpTimer >= jumpTime)
                {
                    jumpFactor = 1f;
                    jumpCounter = 1;
                    secondJumpTimer = 0f;
                    firstJumpActive = false;
                }

                if (thirdJumpTimer >= jumpTime)
                {
                    jumpFactor = 1f;
                    jumpCounter = 1;
                    thirdJumpTimer = 0f;
                    secondJumpActive = false;
                }
                if (firstJumpActive && coyoteCounter > 0f)
                {
                    secondJumpTimer += Time.fixedDeltaTime;
                }

                if (secondJumpActive && coyoteCounter > 0f)
                {
                    thirdJumpTimer += Time.fixedDeltaTime;
                }

                if (moveDirection.y <= 0f && !controller.isGrounded && canHover)
                {
                    canHover = false;
                    isHovering = true;
                    hoverTimer = hoverTime;
                }

                if (isHovering)
                {
                    moveDirection.y = 1f;

                    if (hoverTimer <= 0)
                    {
                        moveDirection.y = -5f;
                        isHovering = false;
                    }
                    else
                    {
                        hoverTimer -= Time.fixedDeltaTime;
                    }
                }

                if (hangCounter < 0)
                {

                    canHang = true;
                }
                else
                {
                    hangCounter -= Time.fixedDeltaTime;
                }

                //if Jump is let go then start falling
                if (jumpReleased && moveDirection.y > 0 && !isBackflipping && !isLongJumping && !isSomersaulting && !isBouncing && !wasHanging)
                {
                    moveDirection.y = moveDirection.y * fallFactor;
                    canHover = false;
                    isHovering = false;
                }

                if (isWallJumping)
                {
                    wallJumpCounter -= Time.fixedDeltaTime;
                }

                if (wallJumpCounter <= 0)
                {
                    isWallJumping = false;
                    canWallJump = false;
                    //canMove = true;
                    wallJumpCounter = wallJumpTime;
                }

                if (!isClimbing)
                {
                    moveDirection.y += Physics.gravity.y * (gravityScale - 1) * Time.fixedDeltaTime;
                }

                if (isClimbing)
                {
                    moveDirection += (playerModel.transform.forward * climbPull);
                }

                if (bounceStart)
                {
                    gravityScale = 5f;
                    moveDirection.y = bounceForce;
                    coyoteCounter = 0;
                    isGroundPounding = false;
                    isLongJumping = false;
                    isBackflipping = false;
                    isDashing = false;
                    isSomersaulting = false;
                    isBouncing = true;
                    canDash = true;
                    canMove = true;
                    canTurn = true;
                    bounceStart = false;
                }

                if (isBouncing)
                {
                    maxAirAcceleration = 30f;
                    maxAirDeceleration = 30f;
                }

                if (enemyStomped)
                {
                    if (jumpPressed)
                    {
                        moveDirection.y += jumpForce;
                    }
                    else
                    {
                        moveDirection.y += 10f;
                    }
                    enemyStomped = false;
                }

                SetSlopeSlideVelocity();

                if (slopeSlideVelocity == Vector3.zero)
                {
                    isSliding = false;
                }

                if (isDashing)
                {
                    canTurn = false;
                    canMove = false;
                    maxAirAcceleration = 60f;
                    maxAirDeceleration = 60f;
                    moveDirection = lastJumpForward * dashSpeed;

                    moveDirection.y = 2f;
                    dashCounter -= Time.fixedDeltaTime;
                    if (dashReleased || crouchPressed)
                    {
                        dashCounter = 0;
                    }
                    if (dashCounter <= 0)
                    {
                        isDashing = false;
                        dashCounter = dashTime;
                    }
                }

                if (isLongJumping)
                {
                    canMove = true;
                    canTurn = false;
                    maxAirAcceleration = 60f;
                    maxAirAcceleration = 60f;
                    gravityScale = 3f;
                }

                if (dashPressed)
                {
                    if (isReading)
                    {
                        readSign = currentSign.GetComponent<ReadSign>();
                        readSign.EndRead();
                        isReading = false;
                    }
                    else if (controller.isGrounded)
                    {
                        if (isNearSign)
                        {
                            readSign = currentSign.GetComponent<ReadSign>();
                            readSign.Read();
                            canMove = false;
                            isReading = true;
                        }
                    }
                    else if (canDash)
                    {
                        //maxAirAcceleration = 1f;
                        gravityScale = 5f;
                        maxAirAcceleration = 50f;
                        maxAirDeceleration = 30f;
                        velocity = lastForward * dashSpeed;
                        lastJumpForward = lastForward;

                        dashSound.Play();
                        isDashing = true;
                        canDash = false;
                        isGroundPounding = false;
                        //canGroundPound = true;
                        isLongJumping = false;
                        isBackflipping = false;
                        isBouncing = false;
                        canTurn = true;
                    }
                }

                if (!isDashing && !canDash && controller.isGrounded)
                {
                    dashCooldownCount -= Time.fixedDeltaTime;
                    if (dashCooldownCount <= 0)
                    {
                        canDash = true;
                        dashCooldownCount = dashCooldown;
                    }
                }

                if (crouchPressed)
                {
                    if (coyoteCounter > 0)
                    {
                        isCrouching = true;
                        controller.height = 1;
                        controller.center = new Vector3(0f, -0.5f, 0f);
                        isStopped = false;
                        controller.slopeLimit = 90;
                    }
                    else
                    {
                        if (canGroundPound)
                        {
                            velocity = Vector3.zero;
                            groundPoundHangcount = groundPoundHangtime;
                            canMove = false;
                            isGroundPounding = true;
                            isLongJumping = false;
                        }
                    }
                }

                if (isCrouching)
                {
                    if (controller.isGrounded)
                    {
                        if (!isStopped)
                        {
                            canMove = false;
                            yStore = moveDirection.y;
                            moveDirection = Vector3.MoveTowards(moveDirection, new Vector3(0, moveDirection.y, 0), 0.5f);
                            moveDirection.y = yStore;
                            if (moveDirection.x == 0 && moveDirection.z == 0)
                            {
                                isStopped = true;
                            }
                        }
                        else
                        {
                            maxSpeed = 1f;
                        }
                        controller.slopeLimit = 50;
                    }
                    else
                    {
                        maxSpeed = defaultMaxSpeed;
                    }
                }

                if (crouchReleased)
                {
                    isCrouching = false;
                    controller.height = 2;
                    controller.center = Vector3.zero;
                    controller.slopeLimit = 90;
                    jumpFactor = 1f;
                }

                if (isSkidding)
                {
                    if (skidCounter < 0f)
                    {
                        isSkidding = false;
                    }
                    else
                    {
                        skidCounter -= Time.fixedDeltaTime;
                    }
                }

                if (somersaultCounter > 0.1f && somersaultCounter < somersaultTime)
                {
                    canSomersault = true;
                }
                else
                {
                    canSomersault = false;
                }

                somersaultCounter += Time.fixedDeltaTime;

                if (isBackflipping)
                {
                    canMove = true;
                    maxAirAcceleration = 3f;
                }

                if (isGroundPounding)
                {
                    canGroundPound = false;
                    if (groundPoundHangcount <= 0)
                    {
                        moveDirection = (Vector3.down * groundPoundPower);
                    }
                    else
                    {
                        moveDirection = Vector3.zero;
                        playerModel.transform.Rotate(new Vector3(0, 1440, 0) * Time.fixedDeltaTime);
                        groundPoundHangcount -= Time.fixedDeltaTime;
                    }
                }

                if (isClimbing)
                {
                    velocity = Vector3.MoveTowards(velocity, moveDirection, climbAcceleration);
                }
                else
                {
                    yStore = moveDirection.y;
                    moveDirection.y = 0;
                    velocity.y = 0;

                    moveDirection = AdjustSlopeVelocity(moveDirection);

                    if (Math.Abs(Vector3.Distance(Vector3.zero, moveDirection)) >= Math.Abs(Vector3.Distance(Vector3.zero, velocity)))
                    {
                        if (controller.isGrounded)
                        {
                            velocity = Vector3.MoveTowards(velocity, moveDirection, maxAcceleration * Time.fixedDeltaTime);
                            //velocity = Vector3.SmoothDamp(velocity, velocity + moveDirection, ref refVelocity, maxAcceleration, maxSpeed);
                        }
                        else
                        {
                            velocity = Vector3.MoveTowards(velocity, moveDirection, maxAirAcceleration * Time.fixedDeltaTime);
                        }
                    }
                    else
                    {
                        if (controller.isGrounded)
                        {
                            velocity = Vector3.MoveTowards(velocity, moveDirection, maxDeceleration * Time.fixedDeltaTime);
                            //velocity = Vector3.SmoothDamp(velocity, moveDirection, ref refVelocity, maxDeceleration, maxSpeed);
                        }
                        else
                        {
                            velocity = Vector3.MoveTowards(velocity, moveDirection, maxAirDeceleration * Time.fixedDeltaTime);
                        }
                    }

                    lastMoveDirection = moveDirection;

                    if (velocity.magnitude > maxSpeed)
                    {
                        velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
                    }
                    velocity.y = yStore;

                    if (controller.isGrounded)
                    {
                        velocity.y -= 2f;
                    }

                    if (isSliding)
                    {
                        velocity = slopeSlideVelocity;
                        velocity.y = moveDirection.y;
                        moveDirection = slopeSlideVelocity;
                    }

                    if (hitHead)
                    {
                        velocity = new Vector3(0, -5, 0);
                        canHover = false;
                        hitHead = false;
                    }
                }

                LedgeGrab();

                if (isHanging)
                {
                    canTurn = false;
                    canGroundPound = false;
                    isDashing = false;
                    isBackflipping = false;
                    isLongJumping = false;
                    maxAirAcceleration = defaultMaxAirAccel;
                    maxAirDeceleration = defaultMaxAirAccel;
                    velocity = Vector3.zero;
                }

                if (wasHanging)
                {
                    yStore = moveDirection.y;
                    moveDirection = playerModel.transform.forward * ledgeClimbFactor;
                    moveDirection.y = yStore;
                    velocity = Vector3.MoveTowards(velocity, moveDirection, maxAirAcceleration * Time.fixedDeltaTime);
                }

                if (radarPressed && canRadar)
                {
                    canRadar = false;
                    radarCounter = radarTime;
                    arrow.SetActive(true);
                    cheeseTimer = UIWait;
                    crackerTimer = UIWait;
                    brickTimer = UIWait;
                    uiMovement.UIToggle(true);
                }
                if (radarCounter > 0)
                {
                    Transform closesstCracker = levelManager.FindClosestCracker(playerModel.transform);
                    if (closesstCracker != null)
                    {
                        arrow.transform.position = new Vector3(playerModel.transform.position.x, playerModel.transform.position.y + 0.5f, playerModel.transform.position.z);
                        arrow.transform.LookAt(closesstCracker);
                        arrow.transform.position += arrow.transform.forward;
                    }
                    radarCounter -= Time.fixedDeltaTime;
                }
                else
                {
                    if (arrow.activeSelf)
                    {
                        arrow.SetActive(false);
                        uiMovement.UIToggle(false);
                    }
                    canRadar = true;
                }


                Physics.SyncTransforms();
                controller.Move(velocity * Time.fixedDeltaTime);
            }
            else
            {
                if (transform.position != pullUpEnd)
                {
                    transform.position = pullUpEnd;
                }
                else
                {
                    wasHanging = false;
                }
            }
        }
        else
        {
            canTurn = false;
            moveDirection.y += Physics.gravity.y * (gravityScale - 1) * Time.fixedDeltaTime;
            controller.Move(moveDirection * Time.fixedDeltaTime);
            knockbackCounter -= Time.fixedDeltaTime;

        }
        //Move player direction
        if (moveDirection.x != 0 || moveDirection.z != 0)
        {
            Quaternion newRotation;
            transform.rotation = Quaternion.Euler(0f, cameraTransform.rotation.eulerAngles.y, 0f);
            Vector3 newLookVector = new(moveDirection.x, 0f, moveDirection.z);
            if (newLookVector != Vector3.zero)
            {
                newRotation = Quaternion.LookRotation(newLookVector);
                dashModel.transform.rotation = newRotation;
                if (canTurn)
                {
                    playerModel.transform.rotation = dashModel.transform.rotation;
                }
                else
                {
                    playerModel.transform.forward = lastJumpForward;
                }

                if (isHanging)
                {
                    playerModel.transform.forward = -wallNormal;
                }
            }

            if (isClimbing)
            {
                if(lastNormal != null)
                {
                    playerModel.transform.forward = -lastNormal;
                }
                Quaternion g = playerModel.transform.rotation;
                g.x = 0;
                g.z = 0;
                playerModel.transform.rotation = g;
            }
            if (isWallJumping)
            {
                playerModel.transform.forward = wallNormal;
            }
        }

        lastForward = dashModel.transform.forward;

        animator.SetBool("isGrounded", controller.isGrounded || coyoteCounter > 0);
        animator.SetBool("isRunning", moveDirection.x != 0 || moveDirection.z != 0);
        animator.SetBool("isCrouching", isCrouching);
        animator.SetBool("isDashing", isDashing);
        animator.SetBool("isBackflipping", isBackflipping);
        animator.SetBool("isLongJumping", isLongJumping);
        animator.SetBool("isSomersaulting", isSomersaulting);
        animator.SetBool("isDoubleJumping", isDoubleJumping);
        animator.SetBool("isTripleJumping", isTripleJumping);
        animator.SetBool("isGroundPounding", isGroundPounding);
        animator.SetBool("isHanging", isHanging);
        animator.SetBool("isClimbing", isClimbing);
        animator.SetBool("isSkidding", isSkidding);
        animator.SetBool("isBouncing", isBouncing);
        animator.SetFloat("climbSpeed", move.ReadValue<Vector2>().magnitude);
        animator.SetFloat("runSpeed", move.ReadValue<Vector2>().magnitude);

        if(cheeseTimer < 0)
        {
            uiMovement.CheeseToggle(false);
        }
        else
        {
            cheeseTimer -= Time.fixedDeltaTime;
        }

        if (crackerTimer < 0)
        {
            uiMovement.CoinToggle(false);
        }
        else
        {
            crackerTimer -= Time.fixedDeltaTime;
        }

        if (brickTimer < 0)
        {
            uiMovement.BrickToggle(false);
        }
        else
        {
            brickTimer -= Time.fixedDeltaTime;
        }

        if (healthTimer < 0)
        {
            uiMovement.HealthToggle(false);
        }
        else
        {
            healthTimer -= Time.fixedDeltaTime;
        }

        jumpPressed = false;
        jumpReleased = false;
        dashPressed = false;
        dashReleased = false;
        crouchPressed = false;
        crouchReleased = false;
        radarPressed = false;
        radarReleased = false;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!controller.isGrounded && hit.collider.CompareTag("CanWallJump"))
        {
            wallNormal = hit.normal;
            canWallJump = true;
            //lastWallNormal = wallNormal;
        }
    }

    private Vector3 AdjustSlopeVelocity(Vector3 velocity)
    {
        var ray = new Ray(transform.position, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hitinfo, 2f))
        {
            Debug.Log(hitinfo.normal.y.ToString());
            var slopeRotation = Quaternion.FromToRotation(Vector3.up, hitinfo.normal);
            var adjustedVelocity = slopeRotation * velocity;

            if(adjustedVelocity.y != 0)
            {
                return adjustedVelocity;
            }
        }
        return velocity;
    }

    private void SetSlopeSlideVelocity()
    {
        int layerMask = LayerMask.GetMask("Default");
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, 5f, layerMask))
        {
            float angle = Vector3.Angle(hitInfo.normal, Vector3.up);

            if (angle >= controller.slopeLimit && !hitInfo.collider.CompareTag("Climbable"))
            {
                slopeSlideVelocity = Vector3.ProjectOnPlane(new Vector3(0, moveDirection.y, 0), hitInfo.normal);
                return;
            }
        }
        if (isSliding)
        {
            slopeSlideVelocity -= 3 * Time.fixedDeltaTime * slopeSlideVelocity;

            if (slopeSlideVelocity.magnitude > 1)
            {
                return;
            }
        }

        slopeSlideVelocity = Vector3.zero;
    }

    public void LedgeGrab()
    {
        if ((velocity.y < 0 || isClimbing) && canHang)
        {
            //if()
            {
                RaycastHit downHit;
                List<Vector3> directions = new() { transform.forward, -transform.forward, transform.right, -transform.right, (transform.forward + transform.right), (-transform.forward + transform.right), (transform.forward + -transform.right), (-transform.forward + -transform.right) };
                foreach (Vector3 direction in directions) {

                    Vector3 LineDownStart = (transform.position + Vector3.up * 0.6f) + (direction * 0.3f);
                    Vector3 LineDownEnd = (transform.position + Vector3.up * 0.4f) + (direction * 0.3f);
                    Physics.Linecast(LineDownStart, LineDownEnd, out downHit, LayerMask.GetMask("Default") | LayerMask.GetMask("Climbable"));

                    if (downHit.collider != null && !downHit.collider.isTrigger)
                    {
                        RaycastHit aboveHit;
                        Vector3 LineFwdStart = new Vector3(transform.position.x, downHit.point.y - 0.1f, transform.position.z);
                        Vector3 LineFwdEnd = new Vector3(transform.position.x, downHit.point.y - 0.1f, transform.position.z) + (direction * 0.3f);
                        Physics.Linecast(LineFwdStart, LineFwdEnd, out fwdHit, LayerMask.GetMask("Default") | LayerMask.GetMask("Climbable"));

                        Vector3 AboveFwdStart = new Vector3(transform.position.x, downHit.point.y + 0.1f, transform.position.z);
                        Vector3 AboveFwdEnd = new Vector3(transform.position.x, downHit.point.y + 0.1f, transform.position.z) + (direction * 0.3f);
                        Physics.Linecast(AboveFwdStart, AboveFwdEnd, out aboveHit, LayerMask.GetMask("Default") | LayerMask.GetMask("Climbable"));
                        //Debug.DrawLine(LineFwdStart, LineFwdEnd);

                        if (aboveHit.collider == null && fwdHit.collider != null && !fwdHit.collider.isTrigger)
                        {
                            velocity = Vector3.zero;
                            canHang = false;
                            isHanging = true;
                            isClimbing = false;
                            hangPos = new Vector3(fwdHit.point.x, downHit.point.y, fwdHit.point.z);
                            Vector3 offset = transform.forward * hangOffsetX + transform.up * hangOffsetY;
                            hangPos += offset;
                            transform.position = hangPos + offset;
                            wallNormal = fwdHit.normal;
                            playerModel.transform.forward = -wallNormal;
                            break;

                        }
                    }
                }
            }

        }
    }

    public void Knockback(Vector3 direction)
    {
        canMove = false;
        knockbackCounter = knockbackTime;

        //velocity = -playerModel.transform.forward * knockbackForce;
        //velocity.y = knockbackForce;

        moveDirection =  -playerModel.transform.forward * knockbackForce;
        moveDirection.y = knockbackForce;
    }
}