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

    [Header("Climbing")]
    public float climbSpeed;
    public float maxClimbTime;
    private float climbTimer;


    public bool climbing;

    [Header("Detection")]
    public float detectionLength;
    public float sphereCastRadius;

    private RaycastHit frontWallHit;
    private bool wallFront;
    #endregion

    private void Update()
    {
        WallCheck();
        StateMachine();

        if (climbing) Debug.Log("Climbing!");

        if (climbing) ClimbingMovement();
    }

    private void StateMachine()
    {
        // State 1 - Climbing
        if (wallFront && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)))
        {
            if (!climbing && climbTimer > 0) StartClimbing();

            //Timer
            if (climbTimer > 0) climbTimer -= Time.deltaTime;
            if (climbTimer < 0) StopClimbing();
        } 

        //State 3 - None
        else
        {
            if (climbing) StopClimbing();
        }
    }

    //Check if facing wall
    private void WallCheck()
    {
        wallFront = Physics.SphereCast(transform.position, sphereCastRadius, transform.forward, out frontWallHit, detectionLength, whatIsWall);

        //wallFront = Physics.Raycast(transform.position, transform.forward, out frontWallHit, detectionLength, whatIsWall);
        //Debug.DrawRay(transform.position, transform.forward, Color.blue);

        if (pm.grounded)
        {
            climbTimer = maxClimbTime;
        }
    }

    private void StartClimbing()
    {
        Debug.Log("Start Climbing!");
        climbing = true;
    }

    private void ClimbingMovement()
    {
        rb.velocity = new Vector3(rb.velocity.x, climbSpeed, rb.velocity.z);
    }

    private void StopClimbing()
    {
        Debug.Log("Climbing Stopped!");
        climbing = false;
    }
}
