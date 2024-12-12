using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterClimb : MonoBehaviour
{
    [Header("References")]
    private PlayerScript pm;
    private Rigidbody rb;
    public Transform orientation;

    //[Header("Detection")]

    //private Raycas

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        pm = GetComponent<PlayerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void WallCheck()
    {
        //FrontWall = Physics.Raycast(transform.position, Vector3.right, out rightWallhit, wallCheckDistance, whatIsWall);
    }
}
