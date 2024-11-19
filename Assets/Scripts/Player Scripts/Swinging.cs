using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the Swinging feature works
/// </summary>
public class Swinging : MonoBehaviour
{
    #region Variables
    [Header("References")]
    public Transform orientation;
    public Rigidbody rb;
    public PlayerScript pm;
    public LayerMask whatIsSwing;


    [Header("Swing")]
    public float swingSpeed;
    public float swingForce;

    public float jumpForce;

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

    private void Start()
    {

        
    }

    private void FixedUpdate()
    {
        

        if (pm.grounded)
        {
            state = swingState.NOTSWINGING;
        }
    }

    private void Update()
    {
        
        

        StateMachine();


       if (pm.grounded)
        {

        }
    }

    private void StateMachine()
    {
        switch (state)
        {
            case swingState.SWING:
                StartSwinging();
                break;

            case swingState.SWINGJUMP:
                SwingMovement();
                break;

            case swingState.NOTSWINGING:
                break;

            default:
                break;
        }

    }


    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log("You collided with... ");
        if (collision.gameObject.tag == "Swing")
        {
            Debug.Log(" A Swing!");

            state = swingState.SWING;
        }
    }

    /// <summary>
    /// Start Swinging
    /// </summary>
    private void StartSwinging()
    {
        Debug.Log("Init Swing");
        swinging = true;

        //Stop Movement
        rb.constraints = RigidbodyConstraints.FreezeAll;

        //Check for input
        if (Input.GetKeyDown(pm.jumpKey))
        {
            //Allow Jump
            pm.readyToJump = true;

            //Unfreeze Pos and Freeze Rot
            rb.constraints = RigidbodyConstraints.None;
            rb.constraints = RigidbodyConstraints.FreezeRotation;

            //Launch
            state = swingState.SWINGJUMP;
        }
    }

    private void SwingMovement()
    {
        Debug.Log("Hey im swingin here!");

        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);


        //rb.AddForce(transform.forward * swingForce, ForceMode.Impulse);
        state = swingState.NOTSWINGING;
        
    }

    /// <summary>
    /// Stop Swinging
    /// </summary>
    private void StopSwinging()
    {

        swinging = false;
    }
}
