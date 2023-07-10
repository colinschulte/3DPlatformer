using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public Animator animator;

    [SerializeField] private float moveSpeed;
    //[SerializeField] public float runMaxSpeed; //Target speed we want the player to reach.
    [SerializeField] public float maxAcceleration; //The speed at which our player accelerates to max speed, can be set to runMaxSpeed for instant acceleration down to 0 for none at all
    //public float runAccelAmount; //The actual force (multiplied with speedDiff) applied to the player.
    //[SerializeField] public float runDecceleration; //The speed at which our player decelerates from their current speed, can be set to runMaxSpeed for instant deceleration down to 0 for none at all
    //public float runDeccelAmount;
    [SerializeField] private Vector3 velocity;


    [SerializeField] private float JumpForce;
    [SerializeField] private float gravityScale;

    [SerializeField] private float coyoteTime;
    private float coyoteCounter;

    [SerializeField] private float jumpTime;
    [SerializeField] private float jumpFactor = 1f;
    [SerializeField] private int jumpCounter = 1;
    private bool firstJumpActive;
    private bool secondJumpActive;
    private float secondJumpTimer;
    private float thirdJumpTimer;

    [SerializeField] private float dashSpeed;
    private bool canDash;
    private bool isDashing;
    [SerializeField] private float dashTime;
    private float dashCounter;
    [SerializeField] private float dashCooldown;
    private float dashCooldownCount;

    private int coinCount;
    [SerializeField] private Vector3 moveDirection;
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
            float xStore = moveDirection.x;
            float yStore = moveDirection.y;
            float zStore = moveDirection.z;

            moveDirection = move.ReadValue<Vector2>();
            moveDirection = (transform.forward * moveDirection.y) + (transform.right * moveDirection.x);
            float magnitude = moveDirection.magnitude;
            magnitude = Mathf.Clamp01(magnitude);
            moveDirection = moveDirection.normalized;
            moveDirection = magnitude * moveSpeed * moveDirection;

            moveDirection.y = yStore;

            //if on the ground, reset y movement and coyote counter
            if (controller.isGrounded)
            {
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
            if (jump.triggered && coyoteCounter > 0f && isSliding == false)
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
                coyoteCounter = 0f;
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
        }
        else
        {
            knockbackCounter -= Time.deltaTime;
        }

        moveDirection.y += (Physics.gravity.y * (gravityScale - 1) * Time.deltaTime);

        setSlopeSlideVelocity();

        if(slopeSlideVelocity == Vector3.zero)
        {
            isSliding = false;
        }

        float maxSpeedChange = maxAcceleration * Time.deltaTime;
        velocity.x = Mathf.MoveTowards(velocity.x, moveDirection.x, maxSpeedChange);
        velocity.y = moveDirection.y;
        velocity.z = Mathf.MoveTowards(velocity.z, moveDirection.z, maxSpeedChange);

        if (isSliding)
        {
            velocity = slopeSlideVelocity;
            velocity.y = moveDirection.y;
        }


        if (dash.WasPressedThisFrame() && canDash)
        {
            velocity = playerModel.transform.forward * dashSpeed;
            isDashing = true;
            canDash = false;
        }

        if (isDashing)
        {
            velocity.y = 1f;
            dashCounter -= Time.deltaTime;
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

        controller.Move(velocity * Time.deltaTime);

        //Move player direction
        if (moveDirection.x != 0 || moveDirection.z != 0)
        {
            transform.rotation = Quaternion.Euler(0f, cameraTransform.rotation.eulerAngles.y, 0f);
            Quaternion newRotation = Quaternion.LookRotation(new Vector3(moveDirection.x, 0f, moveDirection.z));
            playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, newRotation, rotateSpeed * Time.deltaTime);
        }

    animator.SetBool("IsGrounded", controller.isGrounded);
        animator.SetBool("IsRunning", (Mathf.Abs(moveDirection.z) + Mathf.Abs(moveDirection.x)) > 0);
        //TODO: animator.SetBool("IsCrouched")
    }

    private void setSlopeSlideVelocity()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit hitInfo, 5))
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
