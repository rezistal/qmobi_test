using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UfoFirePoint : MonoBehaviour
{
    [SerializeField]
    private GameObject bullet;
    private Player player;
    private float timer = 0;

    void Start()
    {
        bullet = Resources.Load<GameObject>("Prefabs/Bullet");
    }

    private void Shoot()
    {
        int size = 20 * transform.parent.GetComponent<Ufo>().UfoType;
        if(size <= transform.parent.position.x
            && transform.parent.position.x <= Screen.width - size
            && size <= transform.parent.position.y 
            && transform.parent.position.y <= Screen.height - size)
        {
            Instantiate(bullet, transform.position, transform.rotation).layer = gameObject.layer;
        }
    }

    void Update()
    {
        player = FindObjectOfType<Player>();
        if (player)
        {
            timer += Time.deltaTime;
            if (timer >= 2.0f)
            {
                timer = 0;
                Shoot();
            }
        }
    }
}
