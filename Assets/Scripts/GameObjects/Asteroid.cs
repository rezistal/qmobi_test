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
    private int health;
    [SerializeField]
    private bool childAsteroid;

    public static event EventManager.Collided Collided;

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

    private void FixedUpdate()
    {
        Warparound.Reposition(rb, transform.position.x, transform.position.y, 80);
    }


    private void OnTriggerEnter(Collider other)
    {
        Collided(gameObject, other);
    }

    public int CalculateHealth()
    {
        health /= 2;
        transform.localScale *= 0.6f;
        return health;
    }

}
