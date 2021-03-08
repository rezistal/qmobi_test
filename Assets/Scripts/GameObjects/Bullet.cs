using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;
    private int bulletSpeed = 500;

    public static event EventManager.BulletDestroyed Destroyed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.up * bulletSpeed, ForceMode.VelocityChange);
    }

    private void FixedUpdate()
    {
        if (Warparound.Reposition(rb, transform.position.x, transform.position.y, 0))
        {
            StartCoroutine(Countdown());
        }
    }

    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(0.2f);
        StopAllCoroutines();
        Die();
    }

    public void Die()
    {
        Destroyed(gameObject.layer);
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        Die();
    }
}
