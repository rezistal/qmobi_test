using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{

    private ScoreManager scoreManager;
    private Text scoreText;

    // Start is called before the first frame update
    void Start()
    {
        scoreManager = Resources.Load<ScoreManager>("ScriptableObjects/ScoreManager");
        scoreText = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = scoreManager.ScoreValue.ToString();
    }
}
