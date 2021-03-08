using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private Sprite engineOn;
    [SerializeField]
    private Sprite engineOff;
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private SpriteRenderer sr;

    private float deceleration_ratio;
    private float acceleration_ratio;
    private float rotation_ratio;
    private Vector3 velocity;
    private float acceleration;
    private bool invincible = false;

    public static event EventManager.PlayerDeath Died;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        sr = GetComponent<SpriteRenderer>();
        engineOn = Resources.Load<Sprite>("Sprites/PlayerMove");
        engineOff = Resources.Load<Sprite>("Sprites/Player");

        acceleration_ratio = 700;
        rotation_ratio = 15;
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
        //invincible = true;
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

            int selfDestroyChance = UnityEngine.Random.Range(0, 101);
            if(selfDestroyChance >= 100)
            {
                Died();
            }
            else
            {
                rb.MovePosition(new Vector3(x, y, 0));
            }
        }
        
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
        
        Warparound.Reposition(rb, transform.position.x, transform.position.y, 20);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!invincible)
        {
            Died();
        }
    }

}
