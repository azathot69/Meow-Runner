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
    public bool sprinting = false;
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

    //Variables

    #region Movement
    [Header("Movement")]
    public float playerSpeed = 2;
    public float sprintSpeed = 5;
    public float groundDrag;

    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    [SerializeField]
    bool readyToJump;
    #endregion

    #region Keybinds
    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKeyL = KeyCode.LeftShift;
    public KeyCode sprintKeyR = KeyCode.RightShift;

    #endregion

    #region Ground Check
    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    [SerializeField]
    bool grounded;

    public Transform orientation;

    private float horizontalInput;
    private float verticalInput;

    Vector3 moveDirection;
    Rigidbody rb;


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
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();



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
        
        if (Input.GetKey(sprintKeyL) || Input.GetKey(sprintKeyR))
        {
            sprinting = true;
        }
        else
        {
            sprinting = false;
        }

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

    /// <summary>
    /// Moves the player around
    /// </summary>
    void MovePlayer()
    {
        //Local Varibale
        var mySpeed = 0f;   //Stores the speed used
        

        //Check if Sprinting
        if (!sprinting){
            mySpeed = playerSpeed;
        }
        else
        {
            mySpeed = sprintSpeed;
        }

        //Calculate Movement Direction
        var input = moveAction.ReadValue<Vector2>();
        moveDirection.x = input.x;
        moveDirection.z = input.y;

        //if On Slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * mySpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0f)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }

        //On Ground
        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * mySpeed * 10f, ForceMode.Force);

        }else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * mySpeed * 10f * airMultiplier, ForceMode.Force);
        }

        //Turn off fravity while on slope
        rb.useGravity = !OnSlope();
    }

    /// <summary>
    /// Fixes the player's maximum speed
    /// </summary>
    private void SpeedControl()
    {
        //Variable
        var myMaxSpeed = 0f;

        //Check if Sprinting
        if (!sprinting)
        {
            myMaxSpeed = playerSpeed;
        }
        else
        {
            myMaxSpeed = sprintSpeed;
        }

        //Limit Speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > myMaxSpeed)
            {
                rb.velocity = rb.velocity.normalized * myMaxSpeed;
            }
        }
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            //Limit Velocity if needed
            if (flatVel.magnitude > myMaxSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * myMaxSpeed;
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

        Debug.Log("Jumping");
        //Reset Y Velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
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
    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            ImOnSlope = true;
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
            
        }
        ImOnSlope = false;
        return false;
    }

    /// <summary>
    /// Move in direction of slope
    /// </summary>
    /// <returns></returns>
    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
}
