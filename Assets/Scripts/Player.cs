using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Animator animator;

    [SerializeField] private float moveSpeed;
    //[SerializeField] public float runMaxSpeed; //Target speed we want the player to reach.
    [SerializeField] private float maxAcceleration; //The speed at which our player accelerates to max speed, can be set to runMaxSpeed for instant acceleration down to 0 for none at all
    [SerializeField] private float maxAirAcceleration;
    //public float runAccelAmount; //The actual force (multiplied with speedDiff) applied to the player.
    //[SerializeField] public float runDecceleration; //The speed at which our player decelerates from their current speed, can be set to runMaxSpeed for instant deceleration down to 0 for none at all
    //public float runDeccelAmount;
    private Vector3 lastForward;

    [SerializeField] private float maxSpeedChange;
    [SerializeField] private Vector3 velocity;
    private bool canMove;

    [SerializeField] public float JumpForce;
    [SerializeField] public float gravityScale;

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

    public bool enemyStomped;

    private Vector3 wallNormal;
    [SerializeField] private float wallPushback;
    private bool canWallJump;
    private bool isWallJumping;
    [SerializeField] private float wallJumpTime;
    private float wallJumpCounter;
    private Vector3 lastWallNormal;

    [SerializeField] private float dashSpeed;
    private bool canDash = true;
    private bool isDashing = false;
    [SerializeField] private float dashTime;
    private float dashCounter;
    [SerializeField] private float dashCooldown;
    private float dashCooldownCount;

    private int coinCount;
    [SerializeField] public Vector3 moveDirection;
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
        if (knockbackCounter <= 0)
        {
            float xStore = velocity.x;
            float yStore = velocity.y;
            float zStore = velocity.z;

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

            //if on the ground, reset y movement and coyote counter
            if (controller.isGrounded)
            {
                canWallJump = false;

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
            if (jump.triggered)
            {
                if (coyoteCounter > 0f && isSliding == false)
                {
                    //Backflip
                    if (crouch.IsPressed())
                    {
                        jumpFactor = 1.4f;
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
                    moveDirection.y = JumpForce * jumpFactor;
                    jumpSound.Play();
                    coyoteCounter = 0f;
                }
                else if (canWallJump)
                {
                    velocity = Vector3.zero;
                    moveDirection = wallNormal * wallPushback;
                    moveDirection.y = JumpForce * jumpFactor;
                    jumpSound.Play();
                    wallJumpCounter = wallJumpTime;
                    canWallJump = false;
                    canMove = false;
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

            if(crouch.WasReleasedThisFrame())
            {
                jumpFactor = 1f;
            }

            //if Jump is let go then start falling
            if (jump.WasReleasedThisFrame() && moveDirection.y > 0)
            {
                moveDirection.y = -0.2f;
            }

            if (isWallJumping)
            {
                wallJumpCounter -= Time.deltaTime;
            }

            if(wallJumpCounter <= 0)
            {
                isWallJumping = false;
                canWallJump = false;
                canMove = true;
            }
        }
        else
        {
            knockbackCounter -= Time.deltaTime;
        }

        moveDirection.y += Physics.gravity.y * (gravityScale - 1) * Time.deltaTime;

        if (isBouncing)
        {
            moveDirection.y = bounceForce;
            isBouncing = false;
        }

        if (enemyStomped)
        {
            moveDirection.y = 1f;
            enemyStomped = false;
        }

        setSlopeSlideVelocity();

        if(slopeSlideVelocity == Vector3.zero)
        {
            isSliding = false;
        }

        if (dash.WasPressedThisFrame() && canDash)
        {
            velocity = lastForward * dashSpeed;
            dashSound.Play();
            isDashing = true;
            canDash = false;
        }

        if (isDashing)
        {
            moveDirection = lastForward * dashSpeed;
            moveDirection.y = 2f;
            dashCounter -= Time.deltaTime;
            if (dash.WasReleasedThisFrame())
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
        //if (controller.isGrounded)
        //{
        //    maxSpeedChange = maxAcceleration * Time.deltaTime;
        //}
        //else
        //{
        //    maxSpeedChange = maxAirAcceleration * Time.deltaTime;
        //}

        velocity = Vector3.MoveTowards(velocity, moveDirection, maxSpeedChange);
        velocity.y = moveDirection.y;
        //velocity.x = Mathf.MoveTowards(velocity.x, moveDirection.x, maxAcceleration);
        //velocity.z = Mathf.MoveTowards(velocity.z, moveDirection.z, maxAcceleration);

        if (isSliding)
        {
            velocity = slopeSlideVelocity;
            velocity.y = moveDirection.y;
        }


        controller.Move(velocity * Time.deltaTime);

        //Move player direction
        if (moveDirection.x != 0 || moveDirection.z != 0)
        {
            transform.rotation = Quaternion.Euler(0f, cameraTransform.rotation.eulerAngles.y, 0f);
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(velocity.x, 0f, velocity.z));
            //playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
            playerModel.transform.rotation = newRotation;
            lastForward = playerModel.transform.forward;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(!controller.isGrounded && hit.collider.CompareTag("CanWallJump"))
        {
            wallNormal = hit.normal;
            canWallJump = true;
            lastWallNormal = wallNormal;
        }
    }

    private void setSlopeSlideVelocity()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, 5f))
        {
            float angle = Vector3.Angle(hitInfo.normal, Vector3.up);

            if(angle >= controller.slopeLimit)
            {
                slopeSlideVelocity = Vector3.ProjectOnPlane(new Vector3(0, moveDirection.y, 0), hitInfo.normal);
                return;
            }
        }
        if (isSliding)
        {
            slopeSlideVelocity -= slopeSlideVelocity * Time.deltaTime * 3;

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
        moveDirection.y = knockbackForce * 0.5f;
    }
}
