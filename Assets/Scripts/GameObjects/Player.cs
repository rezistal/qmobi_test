using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Sprite engineOn;
    private Sprite engineOff;
    private GameObject destroyPrefab;
    private Rigidbody rb;
    private SpriteRenderer sr;

    private float deceleration_ratio;
    private float acceleration_ratio;
    private float rotation_ratio;
    private Vector3 velocity;
    private float acceleration;
    private bool invincible = false;

    void Start()
    {
        engineOn = Resources.Load<Sprite>("Sprites/PlayerMove");
        engineOff = Resources.Load<Sprite>("Sprites/Player");
        destroyPrefab = Resources.Load<GameObject>("Prefabs/AsteroidDestroy");
        acceleration_ratio = 700;
        rotation_ratio = 15;
        rb = GetComponent<Rigidbody>();
        sr = GetComponent<SpriteRenderer>();
        rb.mass = 100;
        invincible = true;
        StartCoroutine(Invincible());
    }

    IEnumerator Invincible()
    {
        yield return new WaitForSeconds(0.25f);
        sr.enabled = !sr.enabled;
        StartCoroutine(Invincible());
        yield return new WaitForSeconds(3);
        sr.enabled = true;
        invincible = false;
        StopAllCoroutines();
    }

    private void FixedUpdate()
    {
        acceleration = 0;
        if (Input.GetKey(KeyCode.W))
        {
            acceleration = 1;
        }
        rb.AddForce(transform.up * acceleration * acceleration_ratio, ForceMode.Impulse);
        transform.Rotate(0, 0, -Input.GetAxis("Horizontal") * rotation_ratio);

        if (Input.GetKeyDown(KeyCode.F))
        {
            float x = UnityEngine.Random.Range(20, Screen.width - 20);
            float y = UnityEngine.Random.Range(20, Screen.height - 20);
            rb.MovePosition(new Vector3(x, y, 0));
        }

        RePosition();
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            sr.sprite = engineOn;
        }
        else
        {
            sr.sprite = engineOff;
        }
    }

    void RePosition()
    {
        float range = 20;
        float x = transform.position.x;
        float y = transform.position.y;
        float new_x = x, new_y = y;
        bool trigger = false;
        if(x > Screen.width + range)
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
        }
    }

    public void Die()
    {
        if (!invincible)
        {
            Instantiate(destroyPrefab, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

}
