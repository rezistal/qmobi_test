using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Core : MonoBehaviour
{
    public GameObject asteroidPrefab;

    public GameObject player;
    public GameObject playerPrefab;

    private GameObject destroyPrefab;

    private ScoreManager scoreManager;
    private HealthManager healthManager;

    void SpawnAsteroids()
    {
        int asteroidsQuantity = 0; UnityEngine.Random.Range(4, 10);
        for (int i = 0; i <= asteroidsQuantity; i++)
        {
            Instantiate(asteroidPrefab);
        }

    }

    private void OnEnable()
    {
        Player.Died += PlayerDied;
        Asteroid.Collided += ASteroidCollided;
    }

    private void OnDisable()
    {
        Player.Died -= PlayerDied;
        Asteroid.Collided -= ASteroidCollided;
    }

    private void ASteroidCollided(GameObject asteroid, Collider other)
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
            Collider(asteroid);
        }
    }

    private void Collider(GameObject asteroid)
    {
        int health = asteroid.GetComponent<Asteroid>().CalculateHealth();
        scoreManager.SetScore(health);
        if (health > 1)
        {
            Instantiate(asteroid, asteroid.transform.position, asteroid.transform.rotation);
            Instantiate(asteroid, asteroid.transform.position, asteroid.transform.rotation);
        }
        else
        {
            Instantiate(destroyPrefab, asteroid.transform.position, asteroid.transform.rotation);
        }

        Destroy(asteroid);
    }

    private void PlayerDied()
    {
        //healthManager.ReduceHealth();
        Instantiate(destroyPrefab, player.transform.position, player.transform.rotation);
        Destroy(player);
        StartCoroutine(WaitABit());
    }

    IEnumerator WaitABit()
    {
        yield return new WaitForSeconds(2);
        player = Instantiate(playerPrefab);
        StopAllCoroutines();
    }

    void Start()
    {
        //smallAsteroids = new List<GameObject>();

        asteroidPrefab = Resources.Load<GameObject>("Prefabs/Asteroid");
        SpawnAsteroids();

        playerPrefab = Resources.Load<GameObject>("Prefabs/Player");
        player = Instantiate(playerPrefab);

        scoreManager = Resources.Load<ScoreManager>("ScriptableObjects/ScoreManager");
        healthManager = Resources.Load<HealthManager>("ScriptableObjects/HealthManager");

        destroyPrefab = Resources.Load<GameObject>("Prefabs/AsteroidDestroy");
    }

    void Update()
    {

    }
}



