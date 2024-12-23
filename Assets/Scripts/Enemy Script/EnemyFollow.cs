using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    #region Variables
    [Header("References")]
    public Transform target;
    public float enemySpeed;
    public float detectionRadius;

    Rigidbody rb;

    SphereCollider myCollider;

    [SerializeField]
    private behaveState state;
    private Transform startingPos;

    public enum behaveState
    {
        STAY,
        CHASE
    }

    //Range of Notice
    #endregion

    private void Start()
    {
        //Get Rigidbody
        rb = GetComponentInParent<Rigidbody>();

        //Sphere Collider
        myCollider = GetComponent<SphereCollider>();

        if (myCollider != null)
        {
            myCollider.radius = detectionRadius;
        }
    }

    private void Update()
    {
        StateMachine();
    }

    /// <summary>
    /// Handles the Chase State
    /// </summary>
    private void StateMachine()
    {
        switch (state)
        {
            default:
                state = behaveState.STAY;
                break;

            case behaveState.STAY:
                //Don't Do anything. Default state
                break;

            case behaveState.CHASE:
                rb.transform.position = Vector3.MoveTowards(this.transform.position,target.position, enemySpeed * Time.deltaTime);
                break;  
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            state = behaveState.CHASE;
        }
    }

}
