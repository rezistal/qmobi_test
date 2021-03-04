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

    public List<GameObject> asteroids;
    private List<GameObject> smallAsteroids;

    void SpawnAsteroids()
    {
        int asteroidsQuantity = 0; UnityEngine.Random.Range(4, 10);
        for (int i = 0; i <= asteroidsQuantity; i++)
        {
            GameObject asteroid = Instantiate(asteroidPrefab);
            asteroid.GetComponent<Asteroid>().Init();
            asteroids.Add(asteroid);
        }

    }
    void Start()
    {
        smallAsteroids = new List<GameObject>();

        asteroidPrefab = Resources.Load<GameObject>("Prefabs/Asteroid");
        SpawnAsteroids();

        playerPrefab = Resources.Load<GameObject>("Prefabs/Player");
        player = Instantiate(playerPrefab);

        scoreManager = Resources.Load<ScoreManager>("ScriptableObjects/ScoreManager");
        
        destroyPrefab = Resources.Load<GameObject>("Prefabs/AsteroidDestroy");
    }

    void Update()
    {
        if (!player)
        {
            StartCoroutine(WaitABit());
        }

        if(asteroids.Count == 0)
        {
            SpawnAsteroids();
        }
        else
        {
            smallAsteroids.Clear();
            for (int i = asteroids.Count - 1; i >= 0; i--)
            {
                if (asteroids[i].GetComponent<Asteroid>().IsDead())
                {
                    int health = asteroids[i].GetComponent<Asteroid>().Calculate();
                    scoreManager.SetScore(health);
                    if (health > 1)
                    {
                        smallAsteroids.Add(Instantiate(asteroids[i], asteroids[i].transform.position, asteroids[i].transform.rotation));
                        smallAsteroids.Add(Instantiate(asteroids[i], asteroids[i].transform.position, asteroids[i].transform.rotation));
                    }
                    else
                    {
                        Instantiate(destroyPrefab, asteroids[i].transform.position, asteroids[i].transform.rotation);
                    }
                        
                    asteroids[i].GetComponent<Asteroid>().Die();
                    asteroids.RemoveAt(i);
                }
            }
            asteroids.AddRange(smallAsteroids);
        }
    }

    IEnumerator WaitABit()
    {
        yield return new WaitForSeconds(2);
        player = Instantiate(playerPrefab);
        StopAllCoroutines();
    }

}



