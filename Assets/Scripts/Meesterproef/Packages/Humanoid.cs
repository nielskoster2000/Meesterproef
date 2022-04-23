using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Humanoid : MonoBehaviour
{
    int health;
    Bounds bounds;
    [HideInInspector] public UnityEvent<Player, Player> OnDeath;
    [HideInInspector] public UnityEvent OnRecievedDamage;
    [HideInInspector] public UnityEvent OnMaxKillsReached;

    [HideInInspector] public Player player;

    public void ChangeHealth(int amount, Player damager)
    {
        if (amount < 0)
        {
            OnRecievedDamage.Invoke();
        }

        health += amount;

        if (health <= 0)
        {
            Die(player, damager);
        }
    }

    public int GetHealth()
    {
        return health;
    }

    public void Die(Player player, Player killer)
    {
        player.AddDeath(player);
        killer.AddKill();
        OnDeath.Invoke(player, killer);
    }

    public Bounds Bounds { get { return bounds; }  private set { bounds = value; } }

    public Camera cam
    {
        get; private set; 
    }

    public Inventory Inventory { get; set; } 


    private void Awake()
    {
        GameObject hand;

        if (transform.GetComponentInChildren<Camera>().transform.Find("Hand"))
        {
            hand = transform.GetComponentInChildren<Camera>().transform.Find("Hand").gameObject;
        }
        else
        {
            hand = GameManager.FindChildRecursive(transform, "Hand");
        }

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
