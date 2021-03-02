using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float deceleration_ratio;
    private float acceleration_ratio;
    private float rotation_ratio;

    private Rigidbody rb;
    private Vector3 velocity;
    private Vector3 deceleration;
    private float acceleration;

    // Start is called before the first frame update
    void Start()
    {
        deceleration_ratio = -1;
        acceleration_ratio = 5;
        rotation_ratio = 3;
        rb = GetComponent<Rigidbody>();
        rb.mass = 100;
    }

    private void FixedUpdate()
    {
        acceleration = 0;
        if (Input.GetKey(KeyCode.W))
        {
            acceleration = Math.Abs(Input.GetAxis("Vertical"));
        }
        deceleration = rb.velocity * deceleration_ratio;
        rb.AddForce(transform.up * acceleration * acceleration_ratio + deceleration, ForceMode.Impulse);
        this.transform.Rotate(0, 0, -Input.GetAxis("Horizontal") * rotation_ratio);

        if (Input.GetKeyDown(KeyCode.F))
        {
            float x = UnityEngine.Random.Range(-6, 6);
            float y = UnityEngine.Random.Range(-4, 4);
            rb.MovePosition(new Vector3(x, y, 0));
        }

        RePosition();
    }

    // Update is called once per frame
    void Update()
    {


    }

    void RePosition()
    {
        float x = transform.position.x;
        float y = transform.position.y;
        float new_x = x, new_y = y;
        bool trigger = false;
        if(x > 7)
        {
            new_x = -7;
            trigger = true;
        }
        if (x < -7)
        {
            new_x = 7;
            trigger = true;
        }
        if (y > 5.3f)
        {
            new_y = -5.3f;
            trigger = true;
        }
        if (y < -5.3f)
        {
            new_y = 5.3f;
            trigger = true;
        }
        if (trigger)
        {
            rb.MovePosition(new Vector3(new_x, new_y, 0));
        }
    }

}
