using System.Collections.Generic;
using UnityEngine;

public class Humanoid : MonoBehaviour
{
    [HideInInspector] public string username;
    bool isHuman = false;
    int health;
    Vector2 stats;
    List<Vector3> scanPoints = new List<Vector3>();
    Inventory inventory;

    public int Healthd
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

    public Inventory GetInventory()
    {
        return inventory;
    }

    public bool IsHuman
    {
        get { return isHuman; }
        private set { isHuman = value; }
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
        GameObject hand = transform.GetComponentInChildren<Camera>().transform.Find("Hand").gameObject;
        IsHuman = gameObject.CompareTag("Player");
        inventory = new Inventory(hand);
        SetupScanPoints();
    }

    void SetupScanPoints()
    {
        Collider collider = gameObject.GetComponent<Collider>();
        //Do more stuff
    }
}
