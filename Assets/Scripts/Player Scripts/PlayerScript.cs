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
        //jumpAction = playerInput.actions.FindAction("Jump");

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
        //Calculate Movement Direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;


        //On Ground
        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * playerSpeed * 10f, ForceMode.Force);
        }else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * playerSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //Limit Velocity if needed
        if (flatVel.magnitude > playerSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * playerSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);

        }

    }

    private void Jump()
    {
        Debug.Log("Jumping");
        //Reset Y Velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        readyToJump = true;

    }
}
