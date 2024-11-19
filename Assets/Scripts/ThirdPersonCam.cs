using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;

    public float rotationSpeed;

    public GameObject thirdPersonCam;


    public CameraStyle thirdPerson;
    public enum CameraStyle
    {
        Basic
    }

    private void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;

    }

    private void Update()
    {

        //rotate orientation
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        
        //roate player object
        if (thirdPerson == CameraStyle.Basic)
        {
            float horizontalInput = Input.GetAxis("Mouse X");
            //float verticalInput = Input.GetAxis("Vertical");
            Vector3 inputDir = orientation.forward * horizontalInput;

            if (inputDir != Vector3.zero)
                playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }
        

    }

}
