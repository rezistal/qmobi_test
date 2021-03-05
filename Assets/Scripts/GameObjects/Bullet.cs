using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;
    private int bulletSpeed = 500;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.up * bulletSpeed, ForceMode.VelocityChange);
    }

    private void FixedUpdate()
    {
        RePosition();
    }

    private void RePosition()
    {
        float range = 0;
        float x = transform.position.x;
        float y = transform.position.y;
        float new_x = x, new_y = y;
        bool trigger = false;
        if (x > Screen.width + range)
        {
            new_x = 0;
            trigger = true;
        }
        if (x < -range)
        {
            new_x = Screen.width + range;
            trigger = true;
        }
        if (y > Screen.height + range)
        {
            new_y = 0;
            trigger = true;
        }
        if (y < -range)
        {
            new_y = Screen.height + range;
            trigger = true;
        }
        if (trigger)
        {
            rb.MovePosition(new Vector3(new_x, new_y, 0));
            StartCoroutine(Countdown());
        }

    }

    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(0.4f);
        StopAllCoroutines();
        Die();
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        Die();
    }
}
