using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Humanoid : MonoBehaviour
{
    [HideInInspector] public string username;
    int health;
    Vector2 stats;
    List<Vector3> scanPoints = new List<Vector3>();
    Inventory inventory = new Inventory();

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
        //if (abilities != null)
        //{
        //    foreach (Ability ability in abilities)
        //    {
        //        Ability a = gameObject.AddComponent(ability.GetType()) as Ability;
        //    }
        //}
    }

    private void Awake()
    {
        SetupScanPoints();
    }

    void SetupScanPoints()
    {
        Collider collider = gameObject.GetComponent<Collider>();
        //Do more stuff
    }
}
