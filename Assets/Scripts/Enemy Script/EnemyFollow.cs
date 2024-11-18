using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    #region Variables
    [Header("References")]
    public Transform target;
    public float enemySpeed;

    [SerializeField]
    private behaveState state;

    public enum behaveState
    {
        STAY,
        CHASE,
        RETURN
    }

    //Range of Notice
    #endregion

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
                transform.position = Vector3.MoveTowards(this.transform.position,target.position, enemySpeed * Time.deltaTime);
                break;  

            case behaveState.RETURN:

                break;
        }
    }
}
