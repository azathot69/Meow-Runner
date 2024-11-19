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
    /// Lose HP when hit
    /// </summary>
    public void HitByAttack()
    {
        Debug.Log("I'm Hit!");
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
    public void Die()
    {
        gameObject.SetActive(false);
    }
}
