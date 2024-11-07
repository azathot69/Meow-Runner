using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject bulletPrefab;
    public float spawnRate = 1f;
    public bool shootRight = false;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("ShootBullet", 0, spawnRate);

    }


    public void ShootBullet()
    {
        GameObject bulletInstance = Instantiate(bulletPrefab, transform.position, transform.rotation);
        bulletInstance.GetComponent<Bullet>().goingRight = shootRight;
    }
}
