using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Inventory : MonoBehaviour
{
    [SerializeField] public List<Weapon> weapons = new List<Weapon>();
    [SerializeField] public int selectedWeapon = 0;
    public GameObject hand;

    private void Awake()
    {
        SetListSize(6);
    }

    protected void SetListSize(int size)
    {
        for (int i = 0; i < size; i++)
        {
            weapons.Add(null);
        }
    }

    public virtual bool Pickup(Item item)
    {
        if (item.GetType() == typeof(Weapon)) //If item is a weapon
        { 
            Weapon weapon = item as Weapon;

            if (weapons[weapon.inventorySlotPosition] == null) //If this inventory does not yet contain such a weapon...
            {
                weapons[weapon.inventorySlotPosition] = weapon; //We add it to the inventory

                return true; //We return true since the weapon has been picked up and inserted into the inventory
            }
            else //If the item is already in this inventory...
            {
                weapons[weapon.inventorySlotPosition].AddAmmo(weapon.ammoPickupAmount);//Add ammo to the weapon which is already a duplicate
            }
        }
        else //It must be an consumable, in which case we'll use it
        {
            item.Use();
        }

        //If its not picked up return false
        return false;
    }


    public virtual void Equip(Weapon weapon)
    {
        if (weapon != null)
        {
            //Set active
            weapon.transform.parent.gameObject.SetActive(true);

            //Set Position and rotation
            weapon.transform.parent.position = hand.transform.position;
            weapon.transform.parent.rotation = hand.transform.rotation;

            //Set parent
            weapon.transform.parent.parent = hand.transform;

            //Set selected weapon
            selectedWeapon = weapon.inventorySlotPosition;
            weapon.OnEquip();
        }
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
