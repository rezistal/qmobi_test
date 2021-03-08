using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{
    public delegate void Collided(GameObject o, Collider c);
    public delegate void PlayerDeath();
    public delegate void HealthChanged();
    public delegate void BulletDestroyed(int layer);
}
