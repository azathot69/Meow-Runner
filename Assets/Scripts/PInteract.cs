using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PInteract : MonoBehaviour
{
    public GameObject SilverDoor;
    public int silverKeysCollected = 0;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "SilverKey")
        {
            silverKeysCollected++;
            other.gameObject.SetActive(false);
        }

        /*
        if (other.gameObject.tag == "SilverDoor")
        {
            Door collidedDoor = other.gameObject.GetComponent<Door>();

            if (silverKeysCollected >= collidedDoor.silverKeysNeeded)
            {
                silverKeysCollected -= collidedDoor.silverKeysNeeded;
                other.gameObject.SetActive(false);
            }
        }
        */
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
    }

}
