using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
//using static UnityEditor.Timeline.TimelinePlaybackControls;
using TMPro;
using Unity.VisualScripting;

/// <summary>
/// Dictates Player Behavior
/// </summary>
public class PlayerScript : MonoBehaviour
{
    #region Varibles
    [Header("References")]
    PlayerInput playerInput;
    InputAction moveAction;
    InputAction sprintAction;

    public bool wallRun;

    [Header("UI")]
    [SerializeField] private TMP_Text livesText;
    

    [Header("Jumping")]
    InputAction jumpAction;
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    public float jumpDelayTimer;
    private float jumpDelay;
    private bool lingerJump;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    [SerializeField]
    private bool exitingSlope;
    public bool ImOnSlope;

    [Header("Respawning")]
    public int lives = 0;
    public float deathYLevel = -10f;
    [SerializeField]
    private Vector3 startPos;
    //private bool hasDied = false;

    [Header("Movement State")]
    public movementState state;

    public enum movementState
    {
        WALK,
        SPRINT,
        AIR,
        CLIMB,
        WALLRUN
    }

    //Variables

    
    [Header("Movement")]
    public float playerSpeed = 2;
    public float sprintSpeed = 5;
    public float climbSpeed = 2;
    public float wallRunSpeed = 2;
    public float moveSpeed;
    public float groundDrag;



    float horizontalInput;
    float verticalInput;

    
    public bool readyToJump;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKeyL = KeyCode.LeftShift;
    public KeyCode sprintKeyR = KeyCode.RightShift;


    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    public bool grounded;

    public Transform orientation;


    Vector3 moveDirection;
    Rigidbody rb;
    public GameObject catBody;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        //catBody.transform.
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");
        jumpAction = playerInput.actions.FindAction("Jump");
        sprintAction = playerInput.actions.FindAction("Sprint");

        jumpDelay = jumpDelayTimer;

        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        startPos = rb.position;
    }



    // Update is called once per frame
    void Update()
    {
        livesText.text = "Deaths: " + lives;

        //Ground Check
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();
        StateHandler();

        //Handle Drag
        if (grounded)
        {
            rb.drag = groundDrag;

            //Reset jump delay when grounded
            if (jumpDelay <= jumpDelayTimer) jumpDelay = jumpDelayTimer;
        }
        else
        {
            rb.drag = 0;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();

        if (transform.position.y <= deathYLevel)
        {
            //hasDied = true;
            Respawn();
        }

        if (!grounded)
        {
            jumpDelay--;
        }
        if (jumpDelay > 0)
        {
            lingerJump = true;
        }
        else
        {
            lingerJump = false;
        }
    }

    /// <summary>
    /// Kill Player when colliding with enemy
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter(Collider collision)
    {
        switch (collision.gameObject.tag)
        {
            case "bullet":
                Respawn();
                break;
        }


    }

    private void OnCollisionEnter(Collision collision)
    {
        switch (collision.gameObject.tag)
        {
            case "enemy":
                Respawn();
                
                break;
        }
    }

    private void MyInput()
    {

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //When to jump
        if (Input.GetKey(jumpKey) && readyToJump && lingerJump)
        {
            
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);
            
        }

        
    }

    //Handle the different states of the player
    public void StateHandler()
    {
        // Mode - Wall Run
        if (wallRun)
        {
            state = movementState.WALLRUN;
            moveSpeed = wallRunSpeed;
        }

        // Sprinting
        else if (grounded && (Input.GetKey(sprintKeyL) || Input.GetKey(sprintKeyR)))
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
    public void MovePlayer()
    {

        //Calculate Movement Direction
        /*
        var input = moveAction.ReadValue<Vector2>();
        
        moveDirection.x = input.x;
        moveDirection.z = input.y;

        moveDirection.Normalize();
        */

        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //if On Slope
        if (OnSlope() && !exitingSlope)
        {
            //Move player in direction of slope
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 10f, ForceMode.Force);

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

    }

    /// <summary>
    /// Fixes the player's maximum speed
    /// </summary>
    public void SpeedControl()
    {
        
        //Limit Speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
            
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
        /*
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //Limit Velocity if needed
        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);

        }
        */
    }

    /// <summary>
    /// Makes the player jump
    /// </summary>
    public void Jump()
    {
        //if (wallRun) return;

        exitingSlope = true;

        //Reset Y Velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

        //rb.AddForce(Vector3.down * 5, ForceMode.Force);
    }

    /// <summary>
    /// Allows the player to jump again when touching the ground
    /// </summary>
    public void ResetJump()
    {
        exitingSlope = false;
        readyToJump = true;
    }

    private void Respawn()
    {
        //teleport the player to the starting position
        //cause the player to lose a life
        lives++;
        transform.position = startPos;
    }

    /// <summary>
    /// Check if the player is on the slope
    /// </summary>
    /// <returns></returns>
    public bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            if (angle != 0)
            {
                ImOnSlope = true;
            }
            else
            {
                ImOnSlope = false;
            }
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
        return Vector3.ProjectOnPlane(direction, slopeHit.normal).normalized;
    }
}
