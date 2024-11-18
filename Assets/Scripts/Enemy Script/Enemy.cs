using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Variables
    [Header("References")]
    private int HP;

    #endregion

    /// <summary>
    /// Check if hit by attack
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "playerAttack")
        {
            HitByAttack();
        }
    }

    /// <summary>
    /// Lose HP when hit
    /// </summary>
    private void HitByAttack()
    {
        if (HP > 0){
            HP--;
        }
        else {
            //If no HP left, deactivate
            Die();
        }
    }

    /// <summary>
    /// Deactivate self
    /// </summary>
    private void Die()
    {
        gameObject.SetActive(false);
    }
}
