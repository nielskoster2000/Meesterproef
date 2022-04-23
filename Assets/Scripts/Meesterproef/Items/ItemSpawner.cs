using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] GameObject itemPrefab;
    [SerializeField] float RespawnTime = 5f;
    Item item;
    GameObject instance;
    [SerializeField] AudioSource pickupSound;

    private void Start()
    {
        SpawnInstance();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Humanoid humanoid))
        {
            if (item.GetType() == typeof(Weapon))
            {
                Weapon weapon = (Weapon)item;

                if (humanoid.Inventory.Pickup(item))
                {
                    item.enabled = true;
                }
                else
                {
                    humanoid.Inventory.weapons[weapon.inventorySlotPosition].onFire.Invoke();
                }
            }

            //pickupSound.Play();
        }

        //StartCoroutine(StartRespawn(RespawnTime));
    }
    private void SpawnInstance()
    {
        instance = Instantiate(itemPrefab.transform.gameObject, gameObject.transform);

        instance.transform.position = gameObject.transform.position;
        instance.transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));

        item = instance.GetComponentInChildren<Item>();
        item.enabled = false;
    }

    private IEnumerator StartRespawn(float time)
    {
        Destroy(instance);

        yield return new WaitForSeconds(time);

        SpawnInstance();
    }
}
