using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinFood : MonoBehaviour
{
    public float rotationSpeed = 10f;

    void Update()

    {

        //float rotationSpeed = 10f;

        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime, Space.World);

    }
}
