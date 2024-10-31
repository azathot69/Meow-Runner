using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbing : MonoBehaviour
{
    #region Variables
    [Header("References")]
    public Transform orientation;
    public Rigidbody rb;
    public PlayerScript pm;
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;

    [Header("Climbing")]
    public float climbSpeed;
    public float maxClimbTime;
    private float climbTimer;

    [Header("Climb Jumping")]
    public float climbJumpUpForce;
    public float climbJumpBackForce;

    public KeyCode jumpKey = KeyCode.Space;
    public int climbJumps;
    private int climbJumpsLeft;

    public bool climbing;

    [Header("Detection")]
    public float detectionLength;
    public float sphereCastRadius;

    private RaycastHit frontWallHit;
    private bool wallFront;

    private Transform lastWall;
    private Vector3 lastWallNormal;
    public float miniWallNormalAngleChange;

    public float wallCheckDistance;
    public float minJumpHeight;
    private RaycastHit leftWallhit;
    private RaycastHit rightWallhit;
    [SerializeField]
    bool wallRight;
    [SerializeField]
    bool wallLeft;
    private RaycastHit rightWallHit;
    private RaycastHit leftWallHit;

    [Header("Exiting Wall")]
    public bool exitingWall;
    public float exitWallTime;
    private float exitWallTimer;

    #endregion

    private void Update()
    {
        WallCheck();

        if (wallRight && Input.GetKeyDown(jumpKey)) ClimbJumpR();
        if (wallLeft && Input.GetKeyDown(jumpKey)) ClimbJumpL();

        if (exitWallTimer > 0) exitWallTimer -= Time.deltaTime;
        if (exitWallTimer < 0) exitingWall = false;

        //StateMachine();

        //if (climbing && !exitingWall) Debug.Log("Climbing!");

        //if (climbing && !exitingWall) ClimbingMovement();
    }

    private void StateMachine()
    {
        // State 1 - Climbing
        if (wallFront && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) && !exitingWall)
        {
            if (!climbing && climbTimer > 0) StartClimbing();

            //Timer
            if (climbTimer > 0) climbTimer -= Time.deltaTime;
            if (climbTimer < 0) StopClimbing();
        } 

        //State 2 - Exiting
        else if (exitingWall)
        {
            if(climbing) StopClimbing();

            if (exitWallTimer > 0) exitWallTimer -= Time.deltaTime;
            if (exitWallTimer < 0) exitingWall = false;
        }

        //State 3 - None
        else
        {
            if (climbing) StopClimbing();
        }

        if (wallRight && Input.GetKeyDown(jumpKey) && climbJumpsLeft > 0) ClimbJumpR();
        if (wallLeft && Input.GetKeyDown(jumpKey) && climbJumpsLeft > 0) ClimbJumpL();
    }

    private bool AboveGround()
    {
        return !Physics.Raycast(transform.position, Vector3.down, minJumpHeight, whatIsGround);
    }

    //Check if facing wall
    private void WallCheck()
    {
        wallRight = Physics.Raycast(transform.position, Vector3.right, out rightWallhit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -Vector3.right, out leftWallHit, wallCheckDistance, whatIsWall);

    }

    /*
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            wallFront = true;
        }
        else
        {
            wallFront = false;
        }

        wallFront = Physics.SphereCast(transform.position, sphereCastRadius, transform.forward, out frontWallHit, detectionLength, whatIsWall);

        //bool newWall = collision.gameObject.transform != lastWall || Mathf.Abs(Vector3.Angle(lastWallNormal, frontWallHit.normal)) > miniWallNormalAngleChange;

        if ((wallFront) || pm.grounded)
        {
            climbTimer = maxClimbTime;
            climbJumpsLeft = climbJumps;
        }
    }
    */
    #region Climb Up Walls

    private void StartClimbing()
    {
        Debug.Log("Now Climbing!");
        climbing = true;
        pm.climbing = true;

        lastWall = frontWallHit.transform;
        lastWallNormal = frontWallHit.normal;
    }

    private void ClimbingMovement()
    {
        rb.velocity = new Vector3((rb.velocity.y * climbSpeed), rb.velocity.y, rb.velocity.z);
    }

    private void StopClimbing()
    {
        Debug.Log("No Longer Climbing!");
        climbing = false;
        pm.climbing = false;
    }
    #endregion

    private void ClimbJumpR()
    {
        Debug.Log("Kick to the right");
        exitingWall = true;
        exitWallTimer = exitWallTime;

        Vector3 forceToApply = transform.up * climbJumpUpForce + rightWallHit.normal * climbJumpBackForce;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);
    }

    private void ClimbJumpL()
    {
        Debug.Log("Left to the kick");
        exitingWall = true;
        exitWallTimer = exitWallTime;

        Vector3 forceToApply = transform.up * climbJumpUpForce + leftWallhit.normal * climbJumpBackForce;

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(forceToApply, ForceMode.Impulse);
    }
}
