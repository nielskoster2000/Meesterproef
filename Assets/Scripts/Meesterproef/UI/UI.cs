using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class UI : Inventory
{
    Text ammocount;
    Text health;

    [SerializeField] List<InventorySlot> inventorySlots = new List<InventorySlot>();
    GameObject slotsParent;
    int scrollData = 0;
    bool IsInventoryEmpty = true;
    int newSelectedWeapon = 0;

    GameObject pausemenu = null;
    Image crosshair = null;
    Humanoid humanoid;

    [HideInInspector] public UnityEvent onPauseMenuSelected = new UnityEvent();

    public void Awake()
    {
        base.SetListSize(6);

        slotsParent = GameManager.FindChildRecursive(transform, "Inventory");
        humanoid = gameObject.GetComponent<Humanoid>();

        if (humanoid.gameObject.CompareTag("Player"))
        {
            humanoid.OnRecievedDamage.AddListener(UpdateHealth);
        }

        ammocount = GameObject.Find("AmmoCount").GetComponent<Text>();
        health = GameObject.Find("Health").GetComponent<Text>();

        for (int i = 0; i < weapons.Count; i++)
        {
            GameObject InventorySlotObject = Instantiate(Resources.Load<GameObject>("InventorySlot"), slotsParent.transform);
            InventorySlot inventorySlot = InventorySlotObject.GetComponent<InventorySlot>();
            inventorySlot.Selected = false;

            inventorySlots.Add(inventorySlot);
        }

        crosshair = GameObject.Find("Crosshair").GetComponent<Image>();
        pausemenu = GameObject.Find("PauseMenu");
        pausemenu.SetActive(false);
        onPauseMenuSelected.AddListener(TogglePauseMenu);

        UpdateHealth();
    }

    public void Update()
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

    public void UpdateAmmoCount()
    { 
        if (ammocount == null)
        {
            ammocount = GameObject.Find("AmmoCount").GetComponent<Text>();
        }

        if (weapons[selectedWeapon].type != Weapon.Type.melee)
        {
            ammocount.text = weapons[selectedWeapon].ammo.ToString();
        }
        else
        {
            ammocount.text = "";
        }
    }

    public void TogglePauseMenu()
    {
        pausemenu.SetActive(!pausemenu.activeSelf);
        crosshair.gameObject.SetActive(!crosshair.gameObject.activeSelf);
    }

    public void UpdateHealth()
    {
        health.text = humanoid.GetHealth().ToString();
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
                    weapons[selectedWeapon].onFire.Invoke(); //Update ammo count
                    break;
                }

                tries++;
            }
        }
    }

    public void SelectSlot(int slot)
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
            weapons[item.inventorySlotPosition].onFire.AddListener(UpdateAmmoCount);
            inventorySlots[item.inventorySlotPosition].Weapon = weapon;

            if (Settings.equipOnPickup) //Finally we'll have to check if the player prefers to automatically equip picked up weapons
            {
                if (weapons[selectedWeapon] != weapons[item.inventorySlotPosition]) //If weapon is not already selected. This could happen when spawning
                {
                    Equip(weapons[item.inventorySlotPosition]); //Equip the newly selected weapon
                    SelectSlot(item.inventorySlotPosition); //Select the new inventory slot
                }
            }

            inventorySlots[weapon.inventorySlotPosition].Icon = weapon.icon;
            IsInventoryEmpty = false; //Set this false once a weapon is picked up

            return true; //This will return true if a weapon has been picked up
     
        }

        return false; //This will return false if something other than a weapon has been picked up
    }

    public override void Equip(Weapon weapon)
    {
        //Unequip previous weapon if there is one
        if (selectedWeapon != -1) //Check if any weapon has been selected before
        {
            if (weapons[selectedWeapon] != null) //Unequip the previous weapon if there is one
            {
                UnEquip(weapons[selectedWeapon]);
            }
        }

        base.Equip(weapon);

        if (weapon != null)
        {
            weapon.onFire.Invoke(); //Use the event to update the ammo count without any ammo being actually wasted
            weapon.inHand = true; //Set inHand to true so that you can shoot it
            SelectSlot(weapon.inventorySlotPosition); //Select inventoryslot
        }
    }
}
