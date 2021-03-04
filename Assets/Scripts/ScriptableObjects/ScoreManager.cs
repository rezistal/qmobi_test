using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ScoreManager : ScriptableObject
{
    [SerializeField]
    public int ScoreValue { get; private set; }

    private void OnEnable()
    {
        ScoreValue = 0;
    }

    public void SetScore(int val)
    {
        switch (val)
        {
            case 4:
                ScoreValue += 20;
                break;
            case 2:
                ScoreValue += 50;
                break;
            default:
                ScoreValue += 100;
                break;
        }
    }
}
