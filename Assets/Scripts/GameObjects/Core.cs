using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Core : MonoBehaviour
{
    private GameObject asteroidPrefab;
    private GameObject player;
    private GameObject playerPrefab;
    private GameObject destroyPrefab;
    private GameObject ufoPrefab;
    private GameObject ufo;
    private ScoreManager scoreManager;
    private HealthManager healthManager;
    private ShotManager shotManager;

    private float ufoSpawnTimer;
    private float incChance;
    private bool denyUfoSpawn = false;

    private int complexity = 0; // max = 10 (100.000 scores)

    private void OnEnable()
    {
        Player.Died += PlayerDied;
        Asteroid.Collided += Collided;
        Ufo.Collided += Collided;
    }

    private void OnDisable()
    {
        Player.Died -= PlayerDied;
        Asteroid.Collided -= Collided;
        Ufo.Collided -= Collided;
    }

    private void Collided(GameObject obj, Collider other)
    {
        string what = LayerMask.LayerToName(obj.layer);
        string with = LayerMask.LayerToName(other.gameObject.layer);

        //Debug.Log(what + " " + with);

        switch (what)
        {
            case "Asteroids":
                int health = obj.GetComponent<Asteroid>().CalculateHealth();
                if (health > 1)
                {
                    Instantiate(obj, obj.transform.position, obj.transform.rotation);
                    Instantiate(obj, obj.transform.position, obj.transform.rotation);
                }
                else
                {
                    Instantiate(destroyPrefab, obj.transform.position, obj.transform.rotation);
                }
                switch (with)
                {
                    case "Ufo":
                        Instantiate(destroyPrefab, other.transform.position, other.transform.rotation);
                        Destroy(other.gameObject);
                        break;
                    case "Player":
                    case "PlayerBullets":
                        if (scoreManager.SetScore(health, "asteroid"))
                        {
                            healthManager.RestoreHealth();
                        }
                        break;
                    case "UfoBullets":
                        break;
                }



                break;

            case "Ufo":
                int ufoType = obj.GetComponent<Ufo>().UfoType;
                switch (with)
                {
                    case "Player":
                    case "PlayerBullets":
                        if (scoreManager.SetScore(ufoType, "ufo"))
                        {
                            healthManager.RestoreHealth();
                        }
                        break;
                }
                Instantiate(destroyPrefab, obj.transform.position, obj.transform.rotation);
                break;
        }
        Destroy(obj);

        //Check is there more asteroids on a scene
        CheckAsteroids();
    }
    
    private void PlayerDied()
    {
        healthManager.ReduceHealth();
        Instantiate(destroyPrefab, player.transform.position, player.transform.rotation);
        Destroy(player);
        if (healthManager.GetPlayerHealth() == 0)
        {
            GameOver();
        }
        StartCoroutine("spawnPlayer");
    }

    IEnumerator spawnPlayer()
    {
        yield return new WaitForSeconds(2);
        player = Instantiate(playerPrefab);
        StopCoroutine("spawnPlayer");
    }

    IEnumerator spawnAsteroids()
    {
        yield return new WaitForSeconds(3);

            SpawnAsteroids();
            if (ufo != null)
            {
                Destroy(ufo);
            }
        StopCoroutine("spawnAsteroids");
    }

    void Start()
    {
        asteroidPrefab = Resources.Load<GameObject>("Prefabs/Asteroid");
        playerPrefab = Resources.Load<GameObject>("Prefabs/Player");
        scoreManager = Resources.Load<ScoreManager>("ScriptableObjects/ScoreManager");
        healthManager = Resources.Load<HealthManager>("ScriptableObjects/HealthManager");
        destroyPrefab = Resources.Load<GameObject>("Prefabs/AsteroidDestroy");
        ufoPrefab = Resources.Load<GameObject>("Prefabs/Ufo");
        shotManager = Resources.Load<ShotManager>("ScriptableObjects/ShotManager");

        player = Instantiate(playerPrefab);
        SpawnAsteroids();
        ufoSpawnTimer = 0;
    }

    void Update()
    {
        //If there is no other ufo and ufo spawn allowed
        if (ufo == null && !denyUfoSpawn)
        {
            ufoSpawnTimer += Time.deltaTime;
            //Every 10 seconds
            if (ufoSpawnTimer > 10f)
            {

                //Appears a chance to create Ufo
                //float createChance = UnityEngine.Random.Range(0.0f, 1.0f);
                if (UnityEngine.Random.value + incChance >= 0.99999)
                {
                    //Nullify timer and chance increaser
                    ufoSpawnTimer = 0;
                    incChance = 0;

                    SpawnUfo();
                }
                //If ufo didnt created - we increase a chance every frame
                incChance += 0.00001f;
            }
        }
    }

    private void CheckAsteroids()
    {
        GameObject[] asteroidsArray = GameObject.FindGameObjectsWithTag("Asteroid");

        //Its bad idea infinitely spawn ufo, that would change it
        if(asteroidsArray.Length <= 2)
        {
            denyUfoSpawn = true;
        }
        else
        {
            denyUfoSpawn = false;
        }
        
        if(asteroidsArray.Length == 1)
        {
            StartCoroutine("spawnAsteroids");
        }
    }

    private void SpawnAsteroids()
    {
        int asteroidsQuantity = 4 + 8 / 10 * complexity; //UnityEngine.Random.Range(3, 6);
        for (int i = 0; i <= asteroidsQuantity; i++)
        {
            Instantiate(asteroidPrefab);
        }
        complexity = scoreManager.GetScoreValue() / 10000;
        complexity = complexity > 10 ? 10 : complexity;
    }

    private void SpawnUfo()
    {
        ufo = Instantiate(ufoPrefab);
        //Random ufo type, plus the higher complexity implies an increase the precision
        //small fast ufo appears only after 10k points
        //only small fast ufo appears after 40k points
        int ufoType = (scoreManager.GetScoreValue() <= 10000) ?  2 : (scoreManager.GetScoreValue() >= 40000 ? 1 : UnityEngine.Random.Range(1, 3));

        ufo.GetComponent<Ufo>().Init(ufoType, 33 - 3 * complexity);

    }

    private void GameOver()
    {
        healthManager.Reset();
        scoreManager.Reset();
        shotManager.Reset();
        SceneManager.LoadScene("Menu");
    }
}



