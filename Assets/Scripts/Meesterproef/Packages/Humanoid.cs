using System.Collections.Generic;
using UnityEngine;

public class Humanoid : MonoBehaviour
{
    [HideInInspector] public string username;
    int health;
    Vector2 stats;
    Bounds bounds;

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

    public bool IsHuman { get; private set; } = false;
    public Bounds Bounds { get { return bounds; }  private set { bounds = value; } }

    public Camera cam
    {
        get; private set; 
    }

    public Inventory Inventory { get; private set; } 


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
        IsHuman = gameObject.CompareTag("Player");
        Inventory = new Inventory(hand);
        cam = GetComponentInChildren<Camera>();

        if (IsHuman) Bounds = GetComponent<CharacterController>().bounds;
       else Bounds = GetComponent<CapsuleCollider>().bounds;
    }
}
