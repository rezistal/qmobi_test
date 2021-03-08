using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


[CreateAssetMenu]
public class ScoreboardManager : ScriptableObject
{
 
    private ScoreData scoreData;
    public int scoreLimit;

    private void OnEnable()
    {
        scoreLimit = 4;
        LoadData();
    }

    private void OnDisable()
    {
        SaveData();
    }

    public int LowestScore()
    {
        if (scoreData.ScoresQuantity() >= scoreLimit)
        {
            return scoreData.LowestResult(scoreLimit);
        }
        else
        {
            return 0;
        }
    }

    public IEnumerable<(string name, int score, int index)> GreatestResults()
    {
        (string name, int score)[] arr = scoreData.GreatestResults(scoreLimit);

        if (arr.Length > 0)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                yield return (arr[i].name, arr[i].score, i);
            }
        }
        else
        {
            yield break;
        }
    }

    public void AddResult(string name, int score)
    {
        scoreData.AddResult(name, score);
    }

    public void ClearData()
    {
        scoreData = new ScoreData();
    }

    public void SaveData()
    {
        string path = Application.dataPath + Path.DirectorySeparatorChar + "Scoreboard" + Path.DirectorySeparatorChar + "scoreboard.data";
        BinaryFormatter formatter = new BinaryFormatter();

        FileStream stream = new FileStream(path, FileMode.OpenOrCreate);
        formatter.Serialize(stream, scoreData);
        stream.Close();
    }
    

    public void LoadData()
    {
        string path = Application.dataPath + Path.DirectorySeparatorChar + "Scoreboard" + Path.DirectorySeparatorChar + "scoreboard.data";
        BinaryFormatter formatter = new BinaryFormatter();

        if (File.Exists(path))
        {
            FileStream stream = new FileStream(path, FileMode.Open);
            scoreData = formatter.Deserialize(stream) as ScoreData;
            stream.Close();
            
        }
        else
        {
            scoreData = new ScoreData();
        }

    }
}
