using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the Swinging feature works
/// </summary>
public class Swinging : MonoBehaviour
{
    #region
    [Header("References")]
    public Transform orientation;
    public Rigidbody rb;
    public PlayerScript pm;
    public LayerMask whatIsSwing;

    [Header("Swing")]
    public float swingSpeed;

    public bool swinging;

    [Header("Detection")]
    public float detectionLength;
    public float sphereCastRadius;

    private RaycastHit swingObject;
    public bool swingCheck;

    [Header("State Machine")]
    public swingState state;

    public enum swingState{
        SWING,
        SWINGJUMP,
        NOTSWINGING,
    }
    #endregion

    private void StateMachine()
    {
        // S1 - Swinging Stationary ~ Swing Model B.A.F.
        if (swingCheck)
        {
            state = swingState.SWING;
        }

        // S2 - Swing ~ Move Player in Direction
        else
        {
            state = swingState.NOTSWINGING;
        }

    }

    /// <summary>
    /// Check if player touches Swinging Object
    /// </summary>
    private void SwingCheck()
    {
        swingCheck = Physics.SphereCast(transform.position, sphereCastRadius, transform.forward, out swingObject, detectionLength, whatIsSwing);
    }

    /// <summary>
    /// Start Swinging
    /// </summary>
    private void StartSwinging()
    {
        swinging = true;

        //Stop Movement
        rb.velocity = new Vector3(0, 0, 0);
    }



    /// <summary>
    /// Stop Swinging
    /// </summary>
    private void StopSwinging()
    {
        swinging = false;
    }
}
