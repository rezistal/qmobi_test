using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ShotManager : ScriptableObject
{
    private int shotLimit = 4;
    private int shots;

    public void Reset()
    {
        shots = 0;
    }

    private void OnEnable()
    {
        Reset();
        Bullet.Destroyed += CountPlayerBullets;
    }

    private void OnDisable()
    {
        Bullet.Destroyed -= CountPlayerBullets;
    }

    private void CountPlayerBullets(int layer)
    {
        if (LayerMask.LayerToName(layer) == "PlayerBullets")
        {
            if(shots - 1 >= 0)
            {
                shots--;
            }
        }
    }

    public void PlayerShoot()
    {
        shots++;
    }

    public bool Available()
    {
        if (shots < shotLimit)
        {
            return true;
        }
        return false;
    }
}
