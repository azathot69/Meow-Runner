using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    bool isFalling = false;
    float downSpeed = 0f;
    void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Player")
        {
            isFalling = true;
            //Destroy(gameObject, 5);
            StartCoroutine(Respawn(5f, 10f));
        }

        //StartCoroutine(Respawn(5f, 10f));

    }

    private void Update()
    {
        if (isFalling)
        {
            downSpeed += Time.deltaTime / 20;
            transform.position = new Vector3(transform.position.x, transform.position.y - downSpeed, transform.position.z);
        }
    }

    IEnumerator Respawn(float timeToDespawn, float timeToRespawn)
    {
        yield return new WaitForSeconds(timeToDespawn);
        gameObject.SetActive(false);
        yield return new WaitForSeconds(timeToRespawn);
        gameObject.SetActive(true);
    }
}
