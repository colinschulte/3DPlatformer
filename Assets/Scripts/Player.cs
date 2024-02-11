using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Animator animator;
    private PauseGame pause;
    [SerializeField] private LevelManager levelManager;
    private bool firstUpdate = true;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float climbSpeed;
    //[SerializeField] private float forwardInfluence;
    //[SerializeField] private float sidewaysInfluence;
    //[SerializeField] public float runMaxSpeed; //Target speed we want the player to reach.
    [SerializeField] private float maxAcceleration; //The speed at which our player accelerates to max speed, can be set to runMaxSpeed for instant acceleration down to 0 for none at all
    [SerializeField] private float maxAirAcceleration;
    //public float runAccelAmount; //The actual force (multiplied with speedDiff) applied to the player.
    //[SerializeField] public float runDecceleration; //The speed at which our player decelerates from their current speed, can be set to runMaxSpeed for instant deceleration down to 0 for none at all
    //public float runDeccelAmount;
    private Vector3 lastForward;

    [SerializeField] private float maxSpeedChange;
    [SerializeField] private Vector3 velocity;
    public bool canMove;
    public bool canTurn;

    public float jumpForce;
    public float gravityScale;

    [SerializeField] private float coyoteTime;
    private float coyoteCounter;

    [SerializeField] private float jumpTime;
    [SerializeField] private float jumpFactor = 1f;
    [SerializeField] private int jumpCounter = 1;
    private bool firstJumpActive;
    private bool secondJumpActive;
    private float secondJumpTimer;
    private float thirdJumpTimer;

    [SerializeField] private float bounceForce;
    public bool isBouncing = false;

    [SerializeField] private bool jumpPressed;
    [SerializeField] private bool jumpReleased;
    [SerializeField] private bool dashPressed;
    [SerializeField] private bool dashReleased;
    [SerializeField] private bool crouchPressed;
    [SerializeField] private bool crouchReleased;

    [SerializeField] private bool isCrouching;
    [SerializeField] private bool isStopped;
    [SerializeField] private bool isBackflipping;
    [SerializeField] private bool isLongJumping;
    public bool enemyStomped;
    public int groundPoundPower;
    public bool canGroundPound;
    public bool isGroundPounding;
    public float groundPoundHangtime;
    public float groundPoundHangcount;

    public bool isClimbing;
    public Climb climbObject;

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

    //private int coinCount;
    public Vector3 moveDirection;
    public CharacterController controller;

    [SerializeField] private Transform cameraTransform;

    public float rotateSpeed;

    public GameObject playerModel;

    public float knockbackForce;
    public float knockbackTime;
    private float knockbackCounter;

    public PlayerControls playerControls;

    private InputAction move;
    private InputAction jump;
    private InputAction crouch;
    private InputAction dash;

    private bool isSliding;
    [SerializeField] private Vector3 slopeSlideVelocity;

    [SerializeField] private AudioSource jumpSound;
    [SerializeField] private AudioSource dashSound;

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
    }

    private void OnDisable()
    {
        move.Disable();
        jump.Disable();
        crouch.Disable();
        dash.Disable();
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

            if (isClimbing && !controller.isGrounded)
            {
                moveDirection = move.ReadValue<Vector2>();
                moveDirection = (transform.up * moveDirection.y) + (playerModel.transform.right * moveDirection.x);
                //moveDirection = (transform.up * moveDirection.y);
                moveDirection += (playerModel.transform.forward * 0.2f);
                float magnitude = moveDirection.magnitude;
                magnitude = Mathf.Clamp01(magnitude);
                moveDirection = moveDirection.normalized;
                moveDirection = magnitude * climbSpeed * moveDirection;
                canWallJump = true;

                //toggle for moving sideways when climbing
                //moveDirection.x = 0;

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
                maxAcceleration = 1f;
                maxAirAcceleration = 0.85f;
                //forwardInfluence = 1f;
                //sidewaysInfluence = 1f;
                gravityScale = 5f;

                if (slopeSlideVelocity != Vector3.zero)
                {
                    isSliding = true;
                }
                if (isSliding == false)
                {
                    moveDirection.y = 0f;
                }

                coyoteCounter = coyoteTime;

            }
            else
            {

                coyoteCounter -= Time.deltaTime;
            }

            //check if Jump is pressed
            if (jumpPressed)
            {
                if (isReading)
                {
                    readSign = currentSign.GetComponent<ReadSign>();
                    readSign.EndRead();
                    isReading = false;
                }
                if (coyoteCounter > 0f && isSliding == false)
                {

                    if (isCrouching)
                    {
                        if (!isStopped)
                        {
                            //long jump
                            jumpFactor = 0.6f;
                            maxAirAcceleration = 1f;
                            isLongJumping = true;
                            canMove = true;
                        }
                        else
                        {
                            //backflip
                            jumpFactor = 1.4f;
                            isBackflipping = true;
                            canMove = true;
                        }
                    }
                    else
                    {
                        if (jumpCounter == 1)
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
                        wallNormal = -playerModel.transform.forward;
                    }
                    moveDirection = wallNormal * wallPushback;
                    moveDirection.y = jumpForce * jumpFactor;
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
                secondJumpTimer += Time.deltaTime;
            }

            if (secondJumpActive && coyoteCounter > 0f)
            {
                thirdJumpTimer += Time.deltaTime;
            }

            if (crouchReleased)
            {
                jumpFactor = 1f;
            }

            //if Jump is let go then start falling
            if (jumpReleased && moveDirection.y > 0 && !isBackflipping && !isLongJumping)
            {
                moveDirection.y = -0.2f;
            }

            if (isWallJumping)
            {
                wallJumpCounter -= Time.deltaTime;
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
                moveDirection.y += Physics.gravity.y * (gravityScale - 1) * Time.deltaTime;
            }

            if (isBouncing)
            {
                moveDirection.y = bounceForce;
                coyoteCounter = 0;
                isGroundPounding = false;
                canDash = true;
                canMove = true;
                isBouncing = false;
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
                    maxAirAcceleration = 1f;
                    velocity = lastForward * dashSpeed;
                    dashSound.Play();
                    isDashing = true;
                    canDash = false;
                }
                //dashPressed = false;
            }

            if (isDashing)
            {
                moveDirection = lastForward * dashSpeed;
                moveDirection.y = 2f;
                dashCounter -= Time.deltaTime;
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

            if (!isDashing && !canDash && controller.isGrounded)
            {
                dashCooldownCount -= Time.deltaTime;
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
                    this.gameObject.GetComponent<CharacterController>().height = 1;
                    this.gameObject.GetComponent<CharacterController>().center = new Vector3(0f, -0.5f, 0f);
                    isStopped = false;
                }
                else
                {
                    if (canGroundPound)
                    {
                        velocity = Vector3.zero;
                        groundPoundHangcount = groundPoundHangtime;
                        canMove = false;
                        isGroundPounding = true;
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
                        maxAcceleration = 0.15f;
                    }
                }
            }

            if (crouchReleased)
            {
                isCrouching = false;
                this.gameObject.GetComponent<CharacterController>().height = 2;
                this.gameObject.GetComponent<CharacterController>().center = Vector3.zero;
            }

            if (isBackflipping)
            {
                canMove = true;
                yStore = moveDirection.y;
                moveDirection = Vector3.MoveTowards(velocity, moveDirection, maxSpeedChange);
                moveDirection.y = yStore;
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
                    playerModel.transform.Rotate(new Vector3(0, 1440, 0) * Time.deltaTime);
                    groundPoundHangcount -= Time.deltaTime;
                }
            }

            if (isLongJumping)
            {
                canMove = true;
                canTurn = false;
                maxAirAcceleration = 1f;
                gravityScale = 4f;
                yStore = moveDirection.y;
                moveDirection += playerModel.transform.forward * 5;
                moveDirection.y = yStore;
            }

            if (controller.isGrounded)
            {
                velocity = Vector3.MoveTowards(velocity, moveDirection * maxAcceleration, maxSpeedChange);
            }
            else
            {
                velocity = Vector3.MoveTowards(velocity, moveDirection * maxAirAcceleration, maxSpeedChange);
            }

            velocity.y = moveDirection.y;



            if (isSliding)
            {
                velocity = slopeSlideVelocity;
                velocity.y = moveDirection.y;
            }

            controller.Move(velocity * Time.deltaTime);

        }
        else
        {
            canTurn = false;
            moveDirection.y += Physics.gravity.y * (gravityScale - 1) * Time.deltaTime;
            controller.Move(moveDirection * Time.deltaTime);
            knockbackCounter -= Time.deltaTime;

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
                if (canTurn)
                {
                    playerModel.transform.rotation = newRotation;
                }
            }
            lastForward = playerModel.transform.forward;

            if (isClimbing)
            {
                playerModel.transform.LookAt(climbObject.transform.position);
                Quaternion g = playerModel.transform.rotation;
                g.x = 0;
                g.z = 0;
                playerModel.transform.rotation = g;
            }
        }
        animator.SetBool("isGrounded", controller.isGrounded || coyoteCounter > 0);
        animator.SetBool("isRunning", moveDirection.x != 0 || moveDirection.z != 0);
        animator.SetBool("isCrouching", isCrouching);
        animator.SetBool("isDashing", isDashing);
        animator.SetBool("isLongJumping", isLongJumping);
        animator.SetBool("isGroundPounding", isGroundPounding);

        jumpPressed = false;
        jumpReleased = false;
        dashPressed = false;
        dashReleased = false;
        crouchPressed = false;
        crouchReleased = false;
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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Climbable"))
        {
            isClimbing = true;
            wallNormal = collision.transform.position;
            canWallJump = true;
            //lastWallNormal = wallNormal;
        }
        if (collision.gameObject.CompareTag("Sign"))
        {
            isNearSign = true;
            currentSign = collision.gameObject;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Climbable"))
        {
            isClimbing = false;
            canWallJump = false;
        }
        if (collision.gameObject.CompareTag("Sign"))
        {
            isNearSign = false;
            currentSign = null;
        }
    }

    private void SetSlopeSlideVelocity()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, 5f))
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
            slopeSlideVelocity -= 3 * Time.deltaTime * slopeSlideVelocity;

            if (slopeSlideVelocity.magnitude > 1)
            {
                return;
            }
        }

        slopeSlideVelocity = Vector3.zero;
    }

    public void Knockback(Vector3 direction)
    {
        knockbackCounter = knockbackTime;

        moveDirection = direction * knockbackForce;
        moveDirection.y = knockbackForce;
    }
}