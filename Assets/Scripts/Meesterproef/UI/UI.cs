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

        slotsParent = gameObject.GetComponentInChildren<LayoutGroup>().gameObject;
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

            if (weapon != null)
            {
                if (selectedWeapon != -1 && weapons[selectedWeapon] != null) //Unequip the previous weapon
                {
                    UnEquip(weapons[selectedWeapon]);
                }

                selectedWeapon = item.inventorySlotPosition;
                weapons[selectedWeapon].onFire.AddListener(UpdateAmmoCount);
                weapons[selectedWeapon].onFire.Invoke();

                if (Settings.equipOnPickup) //Finally we'll have to check if the player prefers to automatically equip picked up weapons
                {
                    if (weapon != weapons[selectedWeapon]) //If weapon is not already selected. This could happen when spawning
                    {
                        inventorySlots[selectedWeapon].Weapon = weapon;
                        Equip(weapons[selectedWeapon]); //Equip the newly selected weapon
                        SelectSlot(selectedWeapon); //Select the new inventory slot
                    }
                }

                IsInventoryEmpty = false; //Set this false once a weapon is picked up

                return true; //This will return true if a weapon has been picked up
            }
        }

        return false; //This will return false if something other than a weapon has been picked up
    }

    public override void Equip(Weapon weapon)
    {
        base.Equip(weapon);

        if (weapon != null)
        {
            weapon.inHand = true;
        }
    }
}
