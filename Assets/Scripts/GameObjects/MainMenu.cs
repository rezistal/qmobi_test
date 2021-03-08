using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenu;

    [SerializeField]
    private GameObject scoreboard;
    [SerializeField]
    private GameObject scoreRowPrefab;
    [SerializeField]
    private GameObject scoreTable;
    
    private ScoreboardManager scoreboardManager;
    private List<GameObject> scoreRows;


    private void Start()
    {
        scoreboardManager = Resources.Load<ScoreboardManager>("ScriptableObjects/ScoreboardManager");
        scoreRows = new List<GameObject>();
        FillScoreBoard();
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("GameMode");
    }

    public void Scoreboard()
    {
        mainMenu.SetActive(false);
        scoreboard.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void BackToMenu()
    {
        scoreboard.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void ResetScores()
    {
        scoreboardManager.ClearData();
        FillScoreBoard();
    }

    public void FillScoreBoard()
    {
        if (scoreRows != null)
        {
            foreach (GameObject srow in scoreRows)
            {
                Destroy(srow);
            }
        }

        float rowHeight = scoreRowPrefab.GetComponent<RectTransform>().sizeDelta.y;
        foreach ((string name, int score, int index) in scoreboardManager.GreatestResults())
        {
            GameObject row = Instantiate(scoreRowPrefab, scoreTable.transform);
            row.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, -137 - rowHeight * index);
            row.transform.GetChild(0).GetComponent<Text>().text = name;
            row.transform.GetChild(1).GetComponent<Text>().text = score.ToString();
            scoreRows.Add(row);

        }
    }
}
