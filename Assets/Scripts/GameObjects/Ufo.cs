using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ufo : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer sr;
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private UfoFirePoint firePoint;
    private Player player;

    public int UfoType { get; private set; } = 1;
    private int precision = 0;
    private float angle_x, angle_y;
    private float timer;

    public static event EventManager.Collided Collided;

    public void Init(int ufoType, int precision)
    {
        UfoType = ufoType;
        this.precision = precision;
    }

    private void InitPosition()
    {
        int w = Screen.width;
        int h = Screen.height;
        int x = UnityEngine.Random.Range(50, w-50);
        int y = UnityEngine.Random.Range(50, h-50);
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

        int velocity = UnityEngine.Random.Range(320, 440);
        rb.velocity = Vector3.zero;
        rb.AddForce(transform.up * velocity / (UfoType * 1.9f), ForceMode.VelocityChange);
        transform.Rotate(0, 0, -rotate);
    }

    void Start()
    {
        firePoint = GetComponentInChildren<UfoFirePoint>();

        sr = GetComponent<SpriteRenderer>();
        var sprite = Resources.Load<Sprite>("Sprites/Ufo");
        sr.sprite = sprite;


        rb = GetComponent<Rigidbody>();

        transform.localScale *= UfoType;

        InitPosition();
        InitMovement();
    }

    void Update()
    {
        if (UfoType == 1)
        {
            player = FindObjectOfType<Player>();
            if (player)
            {
                Vector3 direction = player.gameObject.transform.position - transform.position;
                angle_x = Vector3.Angle(Vector3.up, direction);
                angle_y = Vector3.Angle(Vector3.right, direction);
            }
        }
        else
        {
            angle_x = UnityEngine.Random.Range(0f, 360f);
            angle_y = UnityEngine.Random.Range(0f, 360f);
        }
        float p = UnityEngine.Random.Range(-precision, precision);
        firePoint.gameObject.transform.rotation = Quaternion.Euler(0, 0, angle_x * (angle_y < 90 ? -1 : 1) + p);

        timer += Time.deltaTime;
        if (timer >= 5)
        {
            timer = 0;
            rb.velocity = new Vector3(rb.velocity.x * Mathf.Cos(70) - rb.velocity.y * Mathf.Sin(70), rb.velocity.x * Mathf.Sin(70) + rb.velocity.y * Mathf.Cos(70), 0);
        }

    }

    private void FixedUpdate()
    { 
        Warparound.Reposition(rb, transform.position.x, transform.position.y, 20 * UfoType);

    }

    private void OnTriggerEnter(Collider other)
    {
        Collided(gameObject, other);
    }
}
