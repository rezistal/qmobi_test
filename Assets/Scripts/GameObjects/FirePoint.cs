using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirePoint : MonoBehaviour
{
    [SerializeField]
    private GameObject bullet;
    private ShotManager shotManager;

    void Start()
    {
        shotManager = Resources.Load<ShotManager>("ScriptableObjects/ShotManager");
        bullet = Resources.Load<GameObject>("Prefabs/Bullet");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shoot();
        }
    }
    
    private void Shoot()
    {
        if(shotManager.Available())
        {
            shotManager.PlayerShoot();
            Instantiate(bullet, transform.position, transform.rotation).layer = gameObject.layer;
        }
    }
}
