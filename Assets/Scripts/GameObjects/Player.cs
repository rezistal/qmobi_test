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
        acceleration_ratio = 300;
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
            float x = UnityEngine.Random.Range(20, Screen.width - 20);
            float y = UnityEngine.Random.Range(20, Screen.height - 20);
            rb.MovePosition(new Vector3(x, y, 0));
        }

        RePosition();
    }

    // Update is called once per frame
    void Update()
    {

        Debug.Log(Screen.height);
    }

    void RePosition()
    {
        float x = transform.position.x;
        float y = transform.position.y;
        float new_x = x, new_y = y;
        bool trigger = false;
        if(x > Screen.width + 20)
        {
            new_x = 0;
            trigger = true;
        }
        if (x < -20)
        {
            new_x = Screen.width + 20;
            trigger = true;
        }
        if (y > Screen.height + 20)
        {
            new_y = 0;
            trigger = true;
        }
        if (y < -20)
        {
            new_y = Screen.height + 20;
            trigger = true;
        }
        if (trigger)
        {
            rb.MovePosition(new Vector3(new_x, new_y, 0));
        }
    }

}
