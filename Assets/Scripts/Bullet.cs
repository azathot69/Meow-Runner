using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed;
    public bool goingRight;

    private void Start()
    {
        //starts the coroutine when the object 
        StartCoroutine(DespawnDelay());
    }

    // Update is called once per frame
    void Update()
    {
        //if the laser should move right, move it right, else move it left
        if (goingRight)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
        else
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }


    }

    IEnumerator DespawnDelay()
    {
        yield return new WaitForSeconds(3);
        Destroy(this.gameObject);
    }
}
