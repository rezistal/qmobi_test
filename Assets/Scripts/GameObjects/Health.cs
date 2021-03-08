using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Health : MonoBehaviour
{
    private HealthManager healthManager;
    private GameObject[] goArray;

    private void OnEnable()
    {
        HealthManager.Changed += PrintHearts;
    }

    private void OnDisable()
    {
        HealthManager.Changed -= PrintHearts;
    }

    private void PrintHearts()
    {
        int health = healthManager.GetPlayerHealth();
        for (int i = 0; i  <= goArray.Length - 1; i++)
        {
            if(i < health)
            {
                goArray[i].GetComponent<Image>().enabled = true;

            } else
            {
                goArray[i].GetComponent<Image>().enabled = false;
            }
        }
    }

    private void Start()
    {
        healthManager = Resources.Load<HealthManager>("ScriptableObjects/HealthManager");
        goArray = new GameObject[5];
        goArray[0] = GameObject.Find("Health1");
        goArray[1] = GameObject.Find("Health2");
        goArray[2] = GameObject.Find("Health3");
        goArray[3] = GameObject.Find("Health4");
        goArray[4] = GameObject.Find("Health5");
        
        PrintHearts();
    }
}
