using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory 
{
    [SerializeField] public List<GameObject> weapons = new List<GameObject>();
    int selectedWeapon;
    GameObject hand;

    public Inventory(GameObject hand)
    {
        this.hand = hand;
    }

    public void Pickup(GameObject item, bool isPlayer = false)
    {
        if (!weapons.Contains(item)) //If this inventory does not yet contain such a weapon...
        {
            weapons.Add(item); //We add it to the inventory...
            if (isPlayer && Settings.equipOnPickup) //And we check if the player prefers to automatically equip picked up weapons
            {
                Equip(item);
            }
        }
        else //If the weapon is already in this inventory...
        {
            foreach (GameObject w in weapons)
            {
                if (item == w)
                {
                    Weapon wComponent = w.GetComponent<Weapon>();
                    wComponent.AddAmmo(wComponent.ammoPickupAmount); //We add the weapon's ammo pickup amount to the ammo of the weapon that we're actually using
                }
            }
        }
    }

    public void Equip(GameObject item)
    {
        if (item.activeSelf)
        {
            item.SetActive(true);
        }

        GameObject instance = GameObject.Instantiate(item);

        instance.transform.position = hand.transform.position;
        instance.transform.rotation = hand.transform.rotation;
        instance.transform.parent = hand.transform;
    }

    public void UnEquip(GameObject instance)
    {
        if (instance.activeSelf)
        {
            Object.Destroy(instance);
        }
    }
}
