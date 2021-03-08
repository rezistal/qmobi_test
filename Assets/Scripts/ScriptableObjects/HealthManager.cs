using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class HealthManager : ScriptableObject
{
    private int PlayerHealth;
    [SerializeField]
    private int StartHealth = 3;

    public static event EventManager.HealthChanged Changed;

    public void Reset()
    {
        PlayerHealth = StartHealth;
    }

    private void OnEnable()
    {
        Reset();
    }

    public void RestoreHealth()
    {
        if (PlayerHealth + 1 <= 5)
        {
            PlayerHealth += 1;
            Changed();
        }
    }

    public void ReduceHealth()
    {
        if (PlayerHealth - 1 >= 0)
        {
            PlayerHealth -= 1;
            Changed();
        }
    }

    public int GetPlayerHealth()
    {
        return PlayerHealth;
    }
}
