using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterWallRun : MonoBehaviour
{
    #region
    [Header("References")]
    public LayerMask whatIsWall;
    public LayerMask whatIsGround;
    public float maxWallRunTime;
    private float wallRunTimer;

    [Header("Detection")]
    public float wallCheckDistance;
    public float minJumpHeight;
    private RaycastHit leftWallhit;
    private RaycastHit rightWallhit;
    [SerializeField]
    private bool wallLeft;
    [SerializeField]
    private bool wallRight;

    [Header("Reference")]
    public Transform orientation;
    [SerializeField]
    private PlayerScript pm;
    private Rigidbody rb;

    [Header("State Machine")]
    public wallRunState state;

    public enum wallRunState
    {
        WALLRUN,
        WALLJUMP,
        WALLEND
    }
    #endregion

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerScript>();
    }


    /// <summary>
    /// Handles Wallrunning Behavior
    /// </summary>
    private void StateMachine()
    {
        switch (state)
        {
            case wallRunState.WALLRUN:
                pm.wallRun = true;
                rb.constraints = RigidbodyConstraints.FreezePositionY;
                break;

            case wallRunState.WALLJUMP:

                break;

            case wallRunState.WALLEND:
                FreeMe();
                break;
        }
    }

    /// <summary>
    /// Check if player is adjacent to wall
    /// </summary>
    private void CheckForWall()
    {
        wallRight = Physics.Raycast(transform.position, Vector3.right, out rightWallhit, wallCheckDistance, whatIsWall);
        wallLeft = Physics.Raycast(transform.position, -Vector3.right, out rightWallhit, wallCheckDistance, whatIsWall);
    }

    /// <summary>
    /// Removes constraints from player RB.
    /// </summary>
    private void FreeMe()
    {
        //rb.constraints = RigidbodyConstrainty.None;
        //rb.constraints = RigidbodyConstrainty.FreezeRotation;
        pm.wallRun = false;
    }
}
