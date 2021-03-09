using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    private ScoreboardManager scoreboardManager;
    private GameObject gameOverUI;
    private GameObject playerInput;
    private GameObject playerInputText;

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

    void Start()
    {
        asteroidPrefab = Resources.Load<GameObject>("Prefabs/Asteroid");
        playerPrefab = Resources.Load<GameObject>("Prefabs/Player");
        scoreManager = Resources.Load<ScoreManager>("ScriptableObjects/ScoreManager");
        healthManager = Resources.Load<HealthManager>("ScriptableObjects/HealthManager");
        destroyPrefab = Resources.Load<GameObject>("Prefabs/AsteroidDestroy");
        ufoPrefab = Resources.Load<GameObject>("Prefabs/Ufo");
        shotManager = Resources.Load<ShotManager>("ScriptableObjects/ShotManager");
        scoreboardManager = Resources.Load<ScoreboardManager>("ScriptableObjects/ScoreboardManager");

        gameOverUI = GameObject.Find("GameOver");
        gameOverUI.SetActive(false);

        player = Instantiate(playerPrefab);
        SpawnAsteroids();
        ufoSpawnTimer = 0;

        healthManager.Reset();
        scoreManager.Reset();
        shotManager.Reset();
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

        //Check is there more asteroids on a scene
        CheckAsteroids();

        //Detecting user input after gameover
        if (playerInputText != null && playerInputText.activeSelf == true)
        {
            string playerName = playerInputText.GetComponent<Text>().text;
            if (playerName.Length == 3 && Input.GetKeyDown(KeyCode.Return))
            {
                scoreboardManager.AddResult(playerName, scoreManager.GetScoreValue());
                SceneManager.LoadScene("Menu");
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu");
        }
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

    IEnumerator ShowGameOver()
    {
        yield return new WaitForSeconds(3);
        StopCoroutine("ShowGameOver");
        CheckScoreboard();
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

        CheckScoreLimit();

    }
    
    private void PlayerDied()
    {
        healthManager.ReduceHealth();
        Instantiate(destroyPrefab, player.transform.position, player.transform.rotation);
        Destroy(player);
        if (healthManager.GetPlayerHealth() == 0)
        {
            GameOver("outOfLives");
        }
        else
        {
            StartCoroutine("spawnPlayer");
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
        
        if(asteroidsArray.Length <= 1)
        {
            StartCoroutine("spawnAsteroids");
        }
    }

    private void CheckScoreLimit()
    {
        if(scoreManager.GetScoreValue() == 99000)
        {
            GameOver("maximumScores");
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

    private void GameOver(string condition)
    {

        denyUfoSpawn = true;

        gameOverUI.SetActive(true);
        playerInput = GameObject.Find("PlayerInput");
        playerInputText = GameObject.Find("PlayerInputText");
        playerInput.SetActive(false);

        if (condition == "maximumScores")
        {
            Destroy(player);
            Text dialog = GameObject.Find("GameOverText").GetComponent<Text>();
            dialog.text = "YOU HAVE REACHED\n" +
                "MAXIMUM SCORE AVAILABLE!";
        }

        StartCoroutine("ShowGameOver");
    }

    private void CheckScoreboard()
    {
        if (scoreManager.GetScoreValue() > scoreboardManager.LowestScore())
        {
            playerInput.SetActive(true);
            playerInput.GetComponent<InputField>().Select();
            playerInput.GetComponent<InputField>().ActivateInputField();

            Text dialog = GameObject.Find("GameOverText").GetComponent<Text>();
            dialog.text = "YOUR SCORE IS ONE OF THE FOUR BEST\n" +
                "PLEASE ENTER YOUR INITIALS\n" +
                "PUSH ENTER WHEN IS READY TO SAVE";
            dialog.fontSize = 30;
        }
        else
        {
            SceneManager.LoadScene("Menu");
        }
    }
}