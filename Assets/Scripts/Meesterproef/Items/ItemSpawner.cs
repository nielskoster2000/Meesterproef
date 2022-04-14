using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] GameObject itemPrefab;
    Item item;

    private void Start()
    {
        GameObject instance = Instantiate(itemPrefab.transform.gameObject, gameObject.transform);

        instance.transform.position = gameObject.transform.position;
        instance.transform.rotation = gameObject.transform.rotation;

        item = instance.GetComponentInChildren<Item>();
        item.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Humanoid>(out Humanoid humanoid))
        {
            humanoid.Inventory.Pickup(item);

            if (item.GetType() == typeof(Weapon))
            {
                Weapon weapon = (Weapon)item;

                humanoid.Inventory.Equip(weapon);
                item.enabled = true;
            }
        }
    }
}
