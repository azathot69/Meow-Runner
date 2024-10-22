using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Dictates Player Behavior
/// </summary>
public class PlayerScript : MonoBehaviour
{
    PlayerInput playerInput;
    InputAction moveAction;
    InputAction jumpAction;
    InputAction sprintAction;

    public bool sprinting = false;

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

        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //When to jump
        if (Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);

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
        //moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
        var input = moveAction.ReadValue<Vector2>();
        moveDirection.x = input.x;
        moveDirection.z = input.y;

        //On Ground
        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * mySpeed * 10f, ForceMode.Force);

        }else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * mySpeed * 10f * airMultiplier, ForceMode.Force);
        }
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


        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //Limit Velocity if needed
        if (flatVel.magnitude > myMaxSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * myMaxSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);

        }

    }

    /// <summary>
    /// Makes the player jump
    /// </summary>
    private void Jump()
    {
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
        readyToJump = true;

    }
}
