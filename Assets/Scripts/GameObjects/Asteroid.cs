using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private SpriteRenderer sr;

    [SerializeField]
    private bool dead = false;
    [SerializeField]
    private int health;
    [SerializeField]
    private bool childAsteroid;

    public bool IsDead()
    {
        return dead;
    }

    private void InitPosition()
    {
        int w = Screen.width;
        int h = Screen.height;
        int x = UnityEngine.Random.Range(0, w);
        int y = UnityEngine.Random.Range(0, h);
        //Безопасная зона появления для игрока
        x += (x > w / 4) && (x < w * 3 / 4) ? w / 2 : 0;
        y += (y > h / 4) && (y < h * 3 / 4) ? h / 2 : 0;
        rb.MovePosition(new Vector3(x, y, 0));
    }

    private void InitMovement()
    {
        int rotate = UnityEngine.Random.Range(0, 360);
        rotate += (rotate == 0) || (rotate == 90) || (rotate == 180) || (rotate == 360) ? 20 : 0;
        transform.Rotate(0, 0, rotate);

        int velocity = UnityEngine.Random.Range(320, 400);
        rb.velocity = Vector3.zero;
        rb.AddForce(transform.up * velocity / health, ForceMode.VelocityChange);

        float torque = UnityEngine.Random.Range(0.4f, 1.2f);
        rb.angularVelocity = Vector3.zero;
        rb.AddTorque(transform.forward * torque, ForceMode.VelocityChange);
    }

    void Start()
    {
        if (!childAsteroid)
        {
            childAsteroid = true;
            int start_health = 3;
            health = (int)Mathf.Pow(2, start_health);
            
            sr = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody>();
            
            int spriteNumber = UnityEngine.Random.Range(1, 3);
            var sprite = Resources.Load<Sprite>("Sprites/Asteroid" + spriteNumber);
            sr.sprite = sprite;

            InitPosition();
        }
        InitMovement();
    }

    void Update()
    {
        RePosition();
    }

    void RePosition()
    {
        float range = 80;
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
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();
        Bullet bullet = other.GetComponent<Bullet>();
        if(player != null)
        {
            player.Die();
        }
        if(bullet != null)
        {
            bullet.Die();
            dead = true;
        }
    }

    public int Calculate()
    {
        dead = false;
        health /= 2;
        transform.localScale *= 0.6f;
        return health;
    }

}
