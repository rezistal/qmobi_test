using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ScoreManager : ScriptableObject
{
    private int ScoreValue;

    public void Reset()
    {
        ScoreValue = 0;
    }
    private void OnEnable()
    {
        Reset();
    }

    public int GetScoreValue()
    {
        return ScoreValue;
    }

    private bool FreeShip(int increment)
    {
        if ((ScoreValue - increment) / 10000 < ScoreValue  / 10000)
        {
            return true;
        }
        return false;
    }

    public bool SetScore(int val, string type)
    {
        int increment = 0;
        switch (type)
        {
            case "asteroid":
                switch (val)
                {
                    case 4:
                        increment += 20;
                        break;
                    case 2:
                        increment += 50;
                        break;
                    default:
                        increment += 100;
                        break;
                }
                break;
            case "ufo":
                switch (val)
                {
                    case 1:
                        increment += 1000;
                        break;
                    case 2:
                        increment += 200;
                        break;
                    default:
                        increment += 0;
                        break;
                }
                break;
        }
        ScoreValue += increment;
        return FreeShip(increment);
    }
}
