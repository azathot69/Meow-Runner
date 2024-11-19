using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Attack : MonoBehaviour
{
    #region
    [Header("References")]
    private bool calmdown; //TEMP VARIABBLE
    public KeyCode attackAction = KeyCode.Mouse0;
    public float atkCoolDown;
    public GameObject attackHitBox;

    private bool canAttack;
    #endregion

    private void Start()
    {
        attackHitBox.SetActive(false);
        canAttack = true;
    }


    private void Update()
    {
        AttackInput();

        //If Can Attack - Return
        if (canAttack) return;


    }

    private void AttackInput()
    {
        if (Input.GetKey(attackAction) && canAttack)
        {
            canAttack = false;

            PlayerAttack();

            Invoke(nameof(ResetAttack), atkCoolDown);
        }
    }

    /// <summary>
    /// Makes the player attack
    /// </summary>
    private void PlayerAttack()
    {
        //Attack
        attackHitBox.SetActive(true);

        //Set up Cooldown
    }

    private void ResetAttack()
    {
        canAttack = true;
        attackHitBox.SetActive(false);
    }
}
