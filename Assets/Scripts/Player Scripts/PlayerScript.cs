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

    [SerializeField]
    //Variables
    [Header("Movement")]
    private float playerSpeed = 2;
    public float sprintSpeed = 5;
    public float groundDrag;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    public Transform orientation;

    private float horizontalInput;
    private float verticalInput;

    Vector3 moveDirection;
    Rigidbody rb;
    

    

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");

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
    }

    /// <summary>
    /// Moves the player around
    /// </summary>
    void MovePlayer()
    {
        //Calculate Movement Direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        rb.AddForce(moveDirection.normalized * playerSpeed * 10f, ForceMode.Force);
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
}
