using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PInteract : MonoBehaviour
{
    public GameObject SilverDoor;
    public GameObject SilverDoor2;
    public int silverKeysCollected = 0;
    public int silverKeys2Collected = 0;


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "SilverKey")
        {
            silverKeysCollected++;
            other.gameObject.SetActive(false);
        }

        if (other.gameObject.tag == "SilverKey2")
        {
            silverKeys2Collected++;
            other.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        /*
        if (gameObject.tag == "SilverDoor")
        {
            if (silverKeysCollected >= 1)
            {
                gameObject.SetActive(false);
            }
        }
        */
        if (silverKeysCollected >= 1)
        {
            //Destroy(SilverDoor);
            SilverDoor.SetActive(false);
        }
        if (silverKeys2Collected >= 2)
        {
            //Destroy(SilverDoor);
            SilverDoor2.SetActive(false);
        }
    }

}
