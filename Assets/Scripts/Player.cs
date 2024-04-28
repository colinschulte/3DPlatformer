using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Animator animator;
    private PauseGame pause;
    [SerializeField] private LevelManager levelManager;
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
    public bool canMove;
    public bool canTurn;

    public float jumpForce;
    public float gravityScale;

    [SerializeField] private float coyoteTime;
    private float coyoteCounter;

    public bool canJump = true;
    [SerializeField] private float jumpTime;
    [SerializeField] private float jumpFactor = 1f;
    [SerializeField] private int jumpCounter = 1;
    private bool firstJumpActive;
    private bool secondJumpActive;
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
    [SerializeField] private bool isStopped;
    [SerializeField] private bool isBackflipping;
    [SerializeField] private bool isLongJumping;
    public bool enemyStomped;
    [SerializeField] private int groundPoundPower;
    [SerializeField] private bool canGroundPound;
    public bool isGroundPounding;
    [SerializeField] private float groundPoundHangtime;
    [SerializeField] private float groundPoundHangcount;

    public bool isHanging;
    public bool canHang;
    public float hangTime;
    private float hangCounter;
    [SerializeField] private float hangOffsetX;
    [SerializeField] private float hangOffsetY;
    [SerializeField] private Vector3 hangPos;


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
    private bool isDashing = false;
    [SerializeField] private float dashTime;
    private float dashCounter;
    [SerializeField] private float dashCooldown;
    private float dashCooldownCount;

    [SerializeField] private bool canRadar;
    [SerializeField] private float radarTime;
    private float radarCounter;

    [SerializeField] private Transform cameraTransform;

    public float rotateSpeed;

    public GameObject playerModel;
    [SerializeField] private GameObject dashModel;
    [SerializeField] private GameObject arrow;

    public float knockbackForce;
    public float knockbackTime;
    private float knockbackCounter;

    public PlayerControls playerControls;

    private InputAction move;
    private InputAction jump;
    private InputAction crouch;
    private InputAction dash;
    private InputAction climb;
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
    }

    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
        crouch.Disable();
        dash.Disable();
        climb.Disable();
        radar.Disable();
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

            if(Physics.Linecast(LineFwdStart, LineFwdEnd, out fwdHit, LayerMask.GetMask("Climbable")))
            {
                Debug.Log("HIT");
            }

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

            if (isClimbing && !controller.isGrounded)
            {
                canDash = false;
                isDashing = false;
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
            }

            //if on the ground, reset y movement and coyote counter
            if (controller.isGrounded)
            {
                if (!pause.gamePaused)
                {
                    canMove = true;
                    canTurn = true;
                }
                canWallJump = false;
                isGroundPounding = false;
                canGroundPound = true;
                isBackflipping = false;
                isLongJumping = false;
                isBouncing = false;
                canHover = false;
                isHovering = false;
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

            //check if Jump is pressed
            if (jumpPressed)
            {

                //remove text
                if (isReading)
                {
                    readSign = currentSign.GetComponent<ReadSign>();
                    readSign.EndRead();
                    isReading = false;
                }

                //if (coyoteCounter > 0f && isSliding == false)
                if (coyoteCounter > 0f)
                {
                    canJump = true;
                }
                else
                {
                    canJump = false;
                }

                if (isHanging)
                {
                    hangCounter = hangTime;
                    canJump = true;
                    isHanging = false;
                }

                if (canJump) {
                    canTurn = false;
                    canHover = true;
                    maxSpeed = defaultMaxSpeed;

                    if (isCrouching)
                    {
                        if (!isStopped)
                        {
                            //long jump
                            jumpFactor = 0.6f;
                            maxAirAcceleration = 50f;
                            isLongJumping = true;
                            canMove = true;
                        }
                        else
                        {
                            //high jump
                            jumpFactor = 1.4f;
                            isBackflipping = true;
                            canMove = true;
                        }
                    }
                    else
                    {
                        if (Vector3.Dot(new Vector3(velocity.x, 0, velocity.z), new Vector3(moveDirection.x, 0, moveDirection.z)) < -0.5)
                        {
                            //backflip
                            jumpFactor = 1.3f;
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
                        }
                        else if (jumpCounter == 3 && secondJumpActive && thirdJumpTimer > 0f)
                        {
                            jumpFactor = 1.3f;
                            jumpCounter++;
                            thirdJumpTimer = 0f;
                            firstJumpActive = false;
                            secondJumpActive = false;
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
            if (jumpReleased && moveDirection.y > 0 && !isBackflipping && !isLongJumping)
            {
                moveDirection.y = -5f;
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
                canMove = true;
                wallJumpCounter = wallJumpTime;
            }

            if (!isClimbing)
            {
                moveDirection.y += Physics.gravity.y * (gravityScale - 1) * Time.fixedDeltaTime;
            }

            if (bounceStart)
            {
                moveDirection.y = bounceForce;
                coyoteCounter = 0;
                isGroundPounding = false;
                canDash = true;
                canMove = true;
                canTurn = true;
                bounceStart = false;
                isBouncing = true;
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
                maxAirAcceleration = 30f;
                maxAirDeceleration = 30f;
                moveDirection = lastForward * dashSpeed;
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
                    if(isNearSign)
                    {
                        readSign = currentSign.GetComponent<ReadSign>();
                        readSign.Read();
                        isReading = true;
                    }
                }
                else if(canDash)
                {
                    //maxAirAcceleration = 1f;
                    velocity = lastForward * dashSpeed;
                    dashSound.Play();
                    isDashing = true;
                    canDash = false;
                    isGroundPounding = false;
                    canGroundPound = true;
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
                        moveDirection = Vector3.MoveTowards(velocity, new Vector3(0, moveDirection.y, 0), 0.5f);
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

            if (isLongJumping)
            {
                canMove = true;
                canTurn = false;
                //maxAirAcceleration = 1f;
                gravityScale = 4f;
                yStore = moveDirection.y;
                moveDirection += playerModel.transform.forward * 20;
                moveDirection.y = yStore;
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

                if(velocity.magnitude > maxSpeed)
                {
                    velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
                }
                velocity.y = yStore;


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

            if(isHanging)
            {
                canTurn = false;
                velocity = Vector3.zero;
            }

            if (radarPressed && canRadar)
            {
                canRadar = false;
                radarCounter = radarTime;
                arrow.SetActive(true);
            }
            if(radarCounter > 0)
            {
                Transform closesstCracker = levelManager.FindClosestCracker(playerModel.transform);
                if(closesstCracker != null )
                {
                    arrow.transform.position = new Vector3(playerModel.transform.position.x, playerModel.transform.position.y + 0.5f, playerModel.transform.position.z);
                    arrow.transform.LookAt(closesstCracker);
                    arrow.transform.position += arrow.transform.forward;
                }
                radarCounter -= Time.fixedDeltaTime;
            }
            else
            {
                arrow.SetActive(false);
                canRadar = true;
            }


            Physics.SyncTransforms();
            controller.Move(velocity * Time.fixedDeltaTime);

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
                if (isHanging)
                {
                    playerModel.transform.forward = -wallNormal;
                }
            }
            lastForward = dashModel.transform.forward;

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
        animator.SetBool("isGrounded", controller.isGrounded || coyoteCounter > 0);
        animator.SetBool("isRunning", moveDirection.x != 0 || moveDirection.z != 0);
        animator.SetBool("isCrouching", isCrouching);
        animator.SetBool("isDashing", isDashing);
        animator.SetBool("isLongJumping", isLongJumping);
        animator.SetBool("isGroundPounding", isGroundPounding);
        animator.SetBool("isHanging", isHanging);
        animator.SetBool("isClimbing", isClimbing);
        animator.SetFloat("climbSpeed", moveDirection.normalized.x + moveDirection.normalized.y);

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
            {
                RaycastHit downHit;
                Vector3 LineDownStart = (transform.position + Vector3.up * 0.6f) + (playerModel.transform.forward * 0.3f);
                Vector3 LineDownEnd = (transform.position + Vector3.up * 0.4f) + (playerModel.transform.forward * 0.3f);
                Physics.Linecast(LineDownStart, LineDownEnd, out downHit, LayerMask.GetMask("Default") | LayerMask.GetMask("Climbable")); 
                Debug.DrawLine(LineDownStart, LineDownEnd);

                if (downHit.collider != null && !downHit.collider.isTrigger)
                {
                    
                    Vector3 LineFwdStart = new Vector3(transform.position.x, downHit.point.y - 0.1f, transform.position.z);
                    Vector3 LineFwdEnd = new Vector3(transform.position.x, downHit.point.y - 0.1f, transform.position.z) + (playerModel.transform.forward * 0.3f);
                    Physics.Linecast(LineFwdStart, LineFwdEnd, out fwdHit, LayerMask.GetMask("Default") | LayerMask.GetMask("Climbable")); 
                    Debug.DrawLine(LineFwdStart, LineFwdEnd);

                    if (fwdHit.collider != null && !fwdHit.collider.isTrigger)
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

                    }
                }
            }

        }
    }

    public void Knockback(Vector3 direction)
    {
        knockbackCounter = knockbackTime;

        moveDirection = direction * knockbackForce;
        moveDirection.y = knockbackForce;
    }
}