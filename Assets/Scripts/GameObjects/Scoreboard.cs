using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoreboard : MonoBehaviour
{

    [SerializeField]
    private GameObject mainMenu;

    public void BackToMenu()
    {
        gameObject.SetActive(false);
        mainMenu.SetActive(true);
    }
}
