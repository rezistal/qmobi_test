using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        Bullet bullet = other.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.Die();
        }
        /*
        // Версия со смертью игрока
        if(other.name == "Player")
        {
            Destroy(other.gameObject);
        }
        */
    }
}
