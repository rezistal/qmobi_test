using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class HealthManager : ScriptableObject
{
    private int PlayerHealth;

    private void OnEnable()
    {
        PlayerHealth = 0;
    }

    public int GetPlayerHealth()
    {
        return PlayerHealth;
    }
}
