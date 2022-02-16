using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] List<Weapon> weapons = new List<Weapon>();
    int selectedWeapon;
    GameObject hand;

    private void Start()
    {
        hand = transform.Find("Hand").gameObject;
    }

    void Pickup(Weapon weapon)
    {
        weapons.Add(weapon);
    }

    void Equip(Weapon weapon)
    {
        weapon.gameObject.transform.position = hand.transform.position;
        weapon.gameObject.transform.rotation = hand.transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("weapon"))
        {
            Pickup(other.GetComponent<Weapon>());
        }
    }
}
