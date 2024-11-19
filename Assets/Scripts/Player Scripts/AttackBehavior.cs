using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBehavior : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        GameObject other = collision.gameObject;

        if (other.CompareTag("enemy"))
        {
            other.GetComponent<Enemy>().HitByAttack();
        }
    }
}
