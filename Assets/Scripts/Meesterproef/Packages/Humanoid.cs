using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Humanoid : MonoBehaviour
{
    int health;
    Vector2 stats = new Vector2(0, 0);
    Bounds bounds;
    [HideInInspector] public UnityEvent<Humanoid> OnPlayerDeath;
    [HideInInspector] public UnityEvent OnRecievedDamage;
    [HideInInspector] public UnityEvent OnMaxKillsReached;

    public void ChangeKills(int amount)
    {
        stats.x += amount; 

        if (stats.x >= Settings.MatchMaxKills)
        {
            OnMaxKillsReached.Invoke();
        }
    }

    public void ChangeDeaths(int amount)
    {
        stats.y += amount;
    }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            OnRecievedDamage.Invoke();
        }

        health += amount;

        if (health <= 0)
        {
            Die();
        }
    }

    public int GetHealth()
    {
        return health;
    }


    public void Die()
    {
        //Animations or something
        ChangeDeaths(1);
        OnPlayerDeath.Invoke(this);
    }


    public Vector2 GetStats()
    {
        return stats;
    }

    public Bounds Bounds { get { return bounds; }  private set { bounds = value; } }

    public Camera cam
    {
        get; private set; 
    }

    public Inventory Inventory { get; set; } 


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
        GameObject hand = transform.GetComponentInChildren<Camera>().transform.Find("Hand").gameObject;
        cam = GetComponentInChildren<Camera>();
        health = 100;

        if (gameObject.CompareTag("Player")) 
        { 
            Bounds = GetComponent<CharacterController>().bounds;
            Inventory = gameObject.AddComponent<UI>();
        }
        else 
        { 
            Bounds = GetComponentInChildren<Collider>().bounds;
            Inventory = gameObject.AddComponent<Inventory>();
        }

        Inventory.hand = hand;
    }
}
