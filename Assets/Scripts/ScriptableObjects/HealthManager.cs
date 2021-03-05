using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class HealthManager : ScriptableObject
{
    private int PlayerHealth;
    [SerializeField]
    private int StartHealth = 3;

    private void OnEnable()
    {
        PlayerHealth = StartHealth;
    }

    public void RestoreHealth()
    {
        PlayerHealth += 1;
    }

    public void ReduceHealth()
    {
        PlayerHealth -= 1;
    }

    public int GetPlayerHealth()
    {
        return PlayerHealth;
    }
}
