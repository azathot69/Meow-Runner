using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliding : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform playerObj;
    private Rigidbody rb;
    private PlayerScript playerMovement;

    [Header("Sliding")]
    public float maxSlideTime;
    public float slideForce;
    private float slideTimer;

    public float slideYScale;
    private float startYScale;

    [Header("Input")]
    public KeyCode slideKey = KeyCode.LeftControl;
    private float horizontalInput;
    private float verticalInput;

    private bool sliding;

    private void start()
    {
        rb = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerScript>();

        startYScale = playerObj.localScale.y;
    }

    private void FixedUpdate()
    {
        if (sliding)
        {
            SlidingMovement();
        }
    }

    private void Update()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(slideKey) && (horizontalInput != 0 || verticalInput != 0))
        {
            StartSlide();
        }

        if (Input.GetKeyUp(slideKey) && sliding)
        {
            StopSlide();
        }
    }

    private void StartSlide()
    {
        sliding = true;
    }

    private void SlidingMovement()
    {
        Vector3 inputDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //Normal Slide
        if (!playerMovement.OnSlope() || rb.velocity.y > -0.1f)
        {
            rb.AddForce(inputDirection.normalized * slideForce, ForceMode.Force);
            slideTimer -= Time.deltaTime;
        }

        //Slope Slide
        else
        {
            rb.AddForce(playerMovement.GetSlopeMoveDirection(inputDirection) * slideForce, ForceMode.Force);
        }

        if (slideTimer <= 0){
            StopSlide();
        }
    }

    private void StopSlide()
    {
        sliding = false;

    }

}
