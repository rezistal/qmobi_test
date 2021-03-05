using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class HealthManager : ScriptableObject
{
    private int PlayerHealth;

    private void OnEnable()
    {
        PlayerHealth = 5;
        Player.Died += ReduceHealth;
    }

    private void OnDisable()
    {
        Player.Died -= ReduceHealth;
    }

    private void ReduceHealth()
    {
        PlayerHealth -= 1;
    }

    public int GetPlayerHealth()
    {
        return PlayerHealth;
    }
}
