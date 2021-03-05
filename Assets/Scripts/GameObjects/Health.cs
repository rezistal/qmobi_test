using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Health : MonoBehaviour
{
    private HealthManager healthManager;
    private Image[] goArray;

    private void PrintHearts()
    {
        int health = healthManager.GetPlayerHealth();
        for (int i = 0; i  <= goArray.Length - 1; i++)
        {
            if(i < health)
            {
                goArray[i].enabled = true;

            } else
            {
                goArray[i].enabled = false;
            }
        }
    }

    private void Start()
    {
        healthManager = Resources.Load<HealthManager>("ScriptableObjects/HealthManager");
        goArray = GetComponentsInChildren<Image>();
    }

    private void Update()
    {
        PrintHearts();
    }
}
