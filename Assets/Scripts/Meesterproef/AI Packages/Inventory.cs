using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : Ability
{
    [SerializeField] List<GameObject> weapons = new List<GameObject>();
    int selectedWeapon;
    GameObject hand;

    private void Start()
    {
        hand = transform.Find("Hand").gameObject;
    }

    public void Pickup(GameObject weapon)
    {
        if (!weapons.Contains(weapon))
        {
            weapons.Add(weapon);
            if (gameObject.CompareTag("Player") && Settings.equipOnPickup)
            {
                Equip(weapon);
            }
        }
        else
        {
            foreach (GameObject w in weapons)
            {
                if (weapon == w)
                {
                    Weapon wComponent = w.GetComponent<Weapon>();
                    wComponent.AddAmmo(wComponent.ammoPickupAmount);
                }
            }
        }
    }

    public void Equip(GameObject inventoryweapon)
    {
        if (inventoryweapon.activeSelf)
        {
            inventoryweapon.SetActive(true);
        }

        GameObject weapon = GameObject.Instantiate(inventoryweapon);

        weapon.transform.position = hand.transform.position;
        weapon.transform.rotation = hand.transform.rotation;
        weapon.transform.parent = hand.transform;
    }

    public void UnEquip(GameObject weapon)
    {
        if (weapon.activeSelf)
        {
            Destroy(weapon);
        }
    }
}
