using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : Inventory
{
    [SerializeField] List<InventorySlot> inventorySlots = new List<InventorySlot>();
    GameObject slotsParent;
    int scrollData = 0;
    bool IsInventoryEmpty = true;
    int newSelectedWeapon = 0;

    public override void Start()
    {
        base.Start();
        slotsParent = gameObject.GetComponentInChildren<LayoutGroup>().gameObject;

        foreach (Weapon weapon in weapons)
        {
            GameObject InventorySlotObject = Instantiate(Resources.Load<GameObject>("InventorySlot"), slotsParent.transform);
            InventorySlot inventorySlot = InventorySlotObject.GetComponent<InventorySlot>();
            inventorySlot.Selected = false;

            inventorySlots.Add(inventorySlot);
        }
    }

    public override void Update()
    {
        if (Input.mouseScrollDelta.y > 0)
        {
            scrollData = -1;
            FindNextWeapon();
        }
        else if (Input.mouseScrollDelta.y < 0)
        {
            scrollData = 1;
            FindNextWeapon();
        }
    }

    void FindNextWeapon()
    {
        if (!IsInventoryEmpty) //We don't need to use this function if the inventory is still empty
        {
            int tries = 0; //We'll need this for the while loop
            newSelectedWeapon = selectedWeapon; //Begin with the currently selected weapon

            while (tries < weapons.Count)
            {
                //Select next weapon in inventory
                newSelectedWeapon += scrollData;

                //Make sure that we don't go out of the weapons list
                if (newSelectedWeapon < 0)
                {
                    newSelectedWeapon = weapons.Count - 1;
                }
                else if (newSelectedWeapon > weapons.Count - 1)
                {
                    newSelectedWeapon = 0;
                }

                if (inventorySlots[newSelectedWeapon].Weapon != null)
                {
                    UnEquip(weapons[selectedWeapon]);
                    Equip(weapons[newSelectedWeapon]);
                    SelectSlot(newSelectedWeapon);
                    selectedWeapon = newSelectedWeapon;
                    break;
                }

                tries++;
            }
        }
    }

    private void SelectSlot(int slot)
    {
        foreach (InventorySlot inventorySlot in inventorySlots)
        {
            inventorySlot.Selected = false;
        }

        inventorySlots[slot].Selected = true;
    }

    public override bool Pickup(Item item)
    {
        if (base.Pickup(item) == true)
        {
            Weapon weapon = item as Weapon;

            if (weapon != null)
            {
                if (Settings.equipOnPickup) //Finally we'll have to check if the player prefers to automatically equip picked up weapons
                {
                    if (weapons[selectedWeapon] != null) //Unequip the previous weapon
                    {
                        UnEquip(weapons[selectedWeapon]);
                    }

                    selectedWeapon = item.inventorySlotPosition;

                    inventorySlots[selectedWeapon].Weapon = weapon;
                    Equip(weapons[selectedWeapon]); //Equip the newly selected weapon
                    SelectSlot(selectedWeapon); //Select the new inventory slot
                }

                IsInventoryEmpty = false; //Set this false once a weapon is picked up

                return true; //This will return true if a weapon has been picked up
            }
        }

        return false; //This will return false if something other than a weapon has been picked up
    }
}
