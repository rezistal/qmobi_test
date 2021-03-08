using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScoreData
{
    [SerializeField]
    private List<ScoreRow> highscores;
    
    public ScoreData()
    {
        highscores = new List<ScoreRow>();
    }

    public void AddResult(string name, int score)
    {
        highscores.Add(new ScoreRow(name, score));
    }
    
    public (string name, int score)[] GreatestResults(int quantity)
    {
        int cap = highscores.Count < quantity ? highscores.Count : quantity;
        if(cap > 0)
        {
            highscores.Sort(CompareRowsDesc);
            (string name, int score)[] array = new(string name, int score)[cap];

            for (int i = 0; i < cap; i++)
            {
                array[i] = highscores[i].GetRow();
            }
            return array;
        }
        return new (string name, int score)[0];
    }

    public int ScoresQuantity()
    {
        return highscores.Count;
    }

    public int LowestResult(int quantity)
    {
        int cap = highscores.Count < quantity ? highscores.Count : quantity;
        highscores.Sort(CompareRowsDesc);
        return highscores[cap-1].GetScore();
    }
 
    private static int CompareRowsAsc(ScoreRow x, ScoreRow y)
    {
        return x.GetScore().CompareTo(y.GetScore());
    }

    private static int CompareRowsDesc(ScoreRow x, ScoreRow y)
    {
        return -x.GetScore().CompareTo(y.GetScore());
    }
}

[System.Serializable]
public class ScoreRow
{
    private string name;
    private int score;

    public ScoreRow(string name, int score)
    {
        this.name = name;
        this.score = score;
    }
    public (string name, int score) GetRow()
    {
        return (name, score);
    }

    public int GetScore()
    {
        return score;
    }

    public string GetName()
    {
        return name;
    }

}