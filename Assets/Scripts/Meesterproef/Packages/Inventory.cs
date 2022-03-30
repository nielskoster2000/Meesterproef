using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory : MonoBehaviour
{
    [SerializeField] public List<Weapon> weapons = new List<Weapon>();
    [SerializeField] protected int selectedWeapon = 0;
    public GameObject hand;

    public virtual void Start()
    {
        SetListSize(5);
    }

    public virtual void Update()
    {

    }

    void SetListSize(int size)
    {
        for (int i = 0; i < size; i++)
        {
            weapons.Add(null);
        }
    }

    public virtual bool Pickup(Item item)
    {
        if (item.GetType() == typeof(Weapon)) //and if item is a weapon
        { 
            Weapon weapon = item as Weapon;

            if (!weapons.Contains(weapon)) //If this inventory does not yet contain such a item...
            {
                weapons[weapon.inventorySlotPosition] = weapon; //We add it to the inventory
                return true; //We return true since the weapon has been picked up and inserted into the inventory
            }
            else //If the item is already in this inventory...
            {
                foreach (Weapon i in weapons)
                {
                    if (weapon == i)
                    {
                        weapon.AddAmmo(weapon.ammoPickupAmount);
                    }
                }
            }
        }

        //If its not picked up return false
        return false;
    }


    public void Equip(Weapon weapon)
    {
        weapon.transform.parent.gameObject.SetActive(true);

        weapon.transform.parent.position = hand.transform.position;
        weapon.transform.parent.rotation = hand.transform.rotation;
        weapon.transform.parent.parent = hand.transform;

        weapon.inHand = true;
    }

    public void UnEquip(Weapon weapon)
    {
        weapon.transform.parent.gameObject.SetActive(false);

        weapon.inHand = false;
    }

    public bool IsSlotEmpty(InventorySlot inventorySlot)
    {
        if (inventorySlot)
        {
            return false;
        }

        return true;
    }

    
}
