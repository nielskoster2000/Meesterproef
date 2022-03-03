using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] GameObject item;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Humanoid>(out Humanoid humanoid))
        {
            humanoid.GetInventory().Pickup(item, humanoid.IsHuman);
        }
    }
}
