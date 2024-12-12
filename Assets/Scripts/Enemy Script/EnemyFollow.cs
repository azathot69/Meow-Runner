using System;
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

    [Header("Attack")]
    public float distFromPlayer;
    public bool canAttack = false;
    public GameObject attackHitBox;

    //Desired Attacking Distance
    public float attackRange;

    //Attack Buffer - How long until Object can attack when Player is in range
    private float attackBuffer;
    public float attackBufferTimer;

    //Attack Hitbox - Make one


    Rigidbody rb;

    SphereCollider myCollider;

    public enum behaveState
    {
        STAY,
        ATTACK,
        CHASE
    }

    public behaveState state;
    private Transform startingPos;

    

    //Range of Notice
    #endregion

    private void Start()
    {
        //Get Rigidbody
        rb = GetComponentInParent<Rigidbody>();

        //Sphere Collider
        myCollider = GetComponent<SphereCollider>();

        attackHitBox.SetActive(false);


        if (myCollider != null)
        {
            myCollider.radius = detectionRadius;
        }

        //Set up Timers
        attackBuffer = attackBufferTimer;
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
                myCollider.radius = detectionRadius;
                break;

            case behaveState.CHASE:
                //Check if Player is in attacking range
                if (distFromPlayer <= attackRange) state = behaveState.ATTACK;

                

                //attackHitBox.SetActive(false);

                rb.transform.position = Vector3.MoveTowards(this.transform.position,target.position, enemySpeed * Time.deltaTime);
                myCollider.radius = detectionRadius * 2;

                //Get Distance from Player
                distFromPlayer = Vector3.Distance(target.position, transform.position);


                
                
                break;  

            case behaveState.ATTACK:
                //If Player's Distance Farther, Chase
                if (canAttack)
                {
                    EnemyAttack();
                }
                else
                {
                    if (attackBuffer <= 0)
                    {
                        canAttack = true;
                    }
                    attackBuffer -= Time.deltaTime;
                    

                }

                
                break;
        }
    }

    /// <summary>
    /// Perform an attack
    /// </summary>
    private void EnemyAttack()
    {
        //Summon Attack
        attackHitBox.SetActive(true);

        //Set Attack Cooldown
        canAttack = false;

        //Reset Attack Buffer
        attackBuffer = attackBufferTimer;

        state = behaveState.CHASE;

        Invoke(nameof(BufferAttackReset), attackBuffer);

    }


    private void BufferAttackReset()
    {
        attackHitBox.SetActive(false);
    }

    /// <summary>
    /// Chase Player
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            state = behaveState.CHASE;
        }


    }

    /// <summary>
    /// De-Aggro if Player is out of range
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" && state == behaveState.CHASE)
        {
            state = behaveState.STAY;
        }
    }
}
