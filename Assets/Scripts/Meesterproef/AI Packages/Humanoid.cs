using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Humanoid : MonoBehaviour
{
    string username;
    int health;
    Vector2 stats;
    Collider collider;
    List<Transform> scanPoints = new List<Transform>();

    public int Health
    {
        get { return health; }
        set { health = Mathf.Clamp(value, 0, 100); }
    }

    public void ChangeKills(int amount)
    {
        stats.x += amount; 
    }

    public void ChangeDeaths(int amount)
    {
        stats.y += amount;
    }



    public Humanoid(string name, List<Ability> abilities)
    {
        if (abilities != null)
        {
            foreach (Ability ability in abilities)
            {
                Ability a = gameObject.AddComponent(ability.GetType()) as Ability;
            }
        }
    }
}
