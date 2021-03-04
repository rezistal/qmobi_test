using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePoint : MonoBehaviour
{
    [SerializeField]
    private GameObject bullet;

    void Start()
    {
        bullet = Resources.Load<GameObject>("Prefabs/Bullet");

    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        Instantiate(bullet, transform.position, transform.rotation);
    }
}
