using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject scoreboard;


    public void PlayGame()
    {
        SceneManager.LoadScene("GameMode");
    }

    public void Scoreboard()
    {
        gameObject.SetActive(false);
        scoreboard.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
