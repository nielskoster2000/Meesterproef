using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory 
{
    [SerializeField] public List<Item> items = new List<Item>();
    int selectedWeapon = 0;
    GameObject hand;

    public Inventory(GameObject hand)
    {
        this.hand = hand;
        SetListSize(6);
    }

    void SetListSize(int size)
    {
        for (int i = 1; i < size; i++)
        {
            items.Add(null);
        }
    }

    public void Pickup(Item item, bool isPlayer = false)
    {
        if (!items.Contains(item)) //If this inventory does not yet contain such a item...
        {
            if (item.GetType() == typeof(Weapon)) //and if item is a weapon
            {
                items.Insert(item.inventorySlotPosition, item); //We add it to the inventory
            }
            else //If its not a weapon it must be an effect, and we'll activate it when picked up
            {
                //ActivateEffect();
            }
         
            if (isPlayer && Settings.equipOnPickup) //Finally we'll have to check if the player prefers to automatically equip picked up weapons
            {
                Equip(item);
            }
        }
        else //If the item is already in this inventory...
        {
            foreach (Item i in items)
            {
                if (item == i)
                {
                    Weapon wComponent = i.GetComponent<Weapon>();
                    wComponent.AddAmmo(wComponent.ammoPickupAmount); //We add the weapon's ammo pickup amount to the ammo of the weapon that we're actually using
                }
            }
        }
    }

    public void Equip(Item item)
    {
        if (item.gameObject.activeSelf)
        {
            item.gameObject.SetActive(true);
        }

        GameObject instance = GameObject.Instantiate(item.transform.parent.gameObject);

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
