using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLooks : MonoBehaviour
{
    public Transform target;


    // Update is called once per frame
    void Update()
    {
        //Face Target
        transform.LookAt(target);
    }
}
