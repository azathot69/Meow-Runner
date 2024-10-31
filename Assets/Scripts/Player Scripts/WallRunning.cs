using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallRunning : MonoBehaviour
{
    #region
    [Header("Wallrunning")]
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public float wallRunForce;
    public float maxWallRunTime;
    private float wallRunTimer;

    [Header("Input")]
    private float horizontalInput;
    private float verticalInput;

    [Header("Detection")]
    public float wallCheckDistance;
    public float minJumpHeight;
    private RaycastHit leftWallhit;
    private RaycastHit rightWallhit;
    private bool wallLeft;
    private bool wallRight;

    [Header("Reference")]
    public Transform orientation;
    [SerializeField]
    private PlayerScript pm;
    private Rigidbody rb;


    #endregion

    private void start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerScript>();
    }



    void Update()
    {
        CheckForWall();
        StateMachine();
    }

    void fixedUpdate()
    {
        if (pm.wallRun)
        {
            WallRunningMovement();
        }
    }

    private void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, Vector3.right, out rightWallhit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -Vector3.right, out rightWallhit, wallCheckDistance, whatIsWall);
        Debug.DrawRay(transform.position, Vector3.right, Color.blue);
        Debug.DrawRay(transform.position, -Vector3.right, Color.blue);
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }

    private void StateMachine()
    {
        //Get Inputs
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        //State 1 - WallRunning
        if ((wallLeft || wallRight) && verticalInput > 0 && AboveGround())
        {
            //Start Wallrunning
            if (!pm.wallRun) 
                StartWallRun();
        }

        //Stage 3 - None
        else
        {
            if (pm.wallRun) 
                StopWallRun();
        }

    }

    private void StartWallRun()
    {
        pm.wallRun = true;
    }

    private void WallRunningMovement()
    {
        Debug.Log("Now Your wall Running!");

        rb.useGravity = false;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        Vector3 wallNormal = wallRight ? rightWallhit.normal : leftWallhit.normal;

        Vector3 wallForward = Vector3.Cross(wallNormal, transform.up);

        //forward force
        rb.AddForce(wallForward * wallRunForce, ForceMode.Force);
    }

    private void StopWallRun()
    {
        pm.wallRun = false;
    }
}
