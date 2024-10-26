using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEditor.Timeline.TimelinePlaybackControls;

/// <summary>
/// Dictates Player Behavior
/// </summary>
public class PlayerScript : MonoBehaviour
{
    
    PlayerInput playerInput;
    InputAction moveAction;
    InputAction sprintAction;

    [Header("Jumping")]
    InputAction jumpAction;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    [SerializeField]
    private bool exitingSlope;
    public bool ImOnSlope;


    [Header("Climbing")]

    [Header("Respawning")]
    public int lives = 1;
    public float deathYLevel = -10f;
    private Vector3 startPos;

    [Header("Movement State")]
    public movementState state;

    public enum movementState
    {
        WALK,
        SPRINT,
        AIR
    }

    //Variables

    #region Movement
    [Header("Movement")]
    public float playerSpeed = 2;
    public float sprintSpeed = 5;
    public float moveSpeed;
    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;

    float horizontalInput;
    float verticalInput;

    [SerializeField]
    bool readyToJump;
    #endregion

    #region Keybinds
    //[Header("Keybinds")]
    private KeyCode jumpKey = KeyCode.Space;
    private KeyCode sprintKeyL = KeyCode.LeftShift;
    private KeyCode sprintKeyR = KeyCode.RightShift;

    #endregion

    #region Ground Check
    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    [SerializeField]
    bool grounded;

    public Transform orientation;


    Vector3 moveDirection;
    Rigidbody rb;
    public GameObject catBody;

    #endregion

    


    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");
        jumpAction = playerInput.actions.FindAction("Jump");
        sprintAction = playerInput.actions.FindAction("Sprint");


        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }



    // Update is called once per frame
    void Update()
    {
        //Ground Check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.3f, whatIsGround);

        MyInput();
        SpeedControl();
        StateHandler();


        //Handle Drag
        if (grounded)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        //horizontalInput = Input.GetAxisRaw("Horizontal");
        //verticalInput = Input.GetAxisRaw("Vertical");

        //When to jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);

        }

        if (transform.position.y <= deathYLevel)
        {
            Respawn();
        }
    }

    //Handle the different states of the player
    public void StateHandler()
    {
        // Sprinting
        if (grounded && (Input.GetKey(sprintKeyL) || Input.GetKey(sprintKeyR)))
        {
            state = movementState.SPRINT;
            moveSpeed = sprintSpeed;
        }

        //Walking
        else if (grounded)
        {
            state = movementState.WALK;
            moveSpeed = playerSpeed;
        }

        //In the Air
        else
        {
            state = movementState.AIR;
        }

    }

    /// <summary>
    /// Moves the player around
    /// </summary>
    void MovePlayer()
    {
        //Calculate Movement Direction
        var input = moveAction.ReadValue<Vector2>();
        moveDirection.x = input.x;
        moveDirection.z = input.y;

        //Debug.Log("X: " + input.x);
        //Debug.Log("Y: " + input.y);

        /*
        switch (moveDirection.x)
        {
            case 0:

                break;

            case 1:
                catBody.transform.rotation = Quaternion.Euler(new Vector3(0, 45, 0));
                break;

            case -1:
                catBody.transform.rotation = Quaternion.Euler(new Vector3(0, -45, 0));
                break;
        }

        switch (moveDirection.y)
        {
            case 0:

                break;

            case 1:
                catBody.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                break;

            case -1:
                catBody.transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
                break;
        }
        */

        //moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //if On Slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }

        //On Ground
        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        }else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }

        //Turn off fravity while on slope
        rb.useGravity = !OnSlope();
    }

    /// <summary>
    /// Fixes the player's maximum speed
    /// </summary>
    private void SpeedControl()
    {
        //Limit Speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            //Limit Velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);

            }
        }
    }

    /// <summary>
    /// Makes the player jump
    /// </summary>
    private void Jump()
    {
        exitingSlope = true;

        //Debug.Log("Jumping");
        //Reset Y Velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        //rb.AddForce(Vector3.down * 200f, ForceMode.Force);
    }

    /// <summary>
    /// Allows the player to jump again when touching the ground
    /// </summary>
    private void ResetJump()
    {
        exitingSlope = false;

        readyToJump = true;
    }

    private void Respawn()
    {
        //teleport the player to the starting position
        //cause the player to lose a life
        lives--;
        transform.position = startPos;

    }

    /// <summary>
    /// Check if the player is on the slope
    /// </summary>
    /// <returns></returns>
    public bool OnSlope()
    {
        //Debug.DrawRay(rb.transform.position, Vector3.down, Color.green);
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            
            ImOnSlope = true;
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);

            //Debug.Log("Angle: " + angle);



            return angle < maxSlopeAngle && angle != 0;
            
        }
        ImOnSlope = false;
        return false;
    }

    /// <summary>
    /// Move in direction of slope
    /// </summary>
    /// <returns></returns>
    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        //Debug.DrawRay(transform.position, Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized, Color.green);
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }
}
