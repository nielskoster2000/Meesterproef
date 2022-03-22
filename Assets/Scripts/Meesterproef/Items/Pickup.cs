using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pickup : Item
{
    [Space]
    public UnityEvent OnPickup;

    private void OnTriggerEnter(Collider other)
    {
        OnPickup.Invoke();
    }
}
