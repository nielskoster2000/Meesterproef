using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string _name;
    public int inventorySlotPosition;

    public virtual void Use()
    {
        
    }

    public virtual void OnPickup()
    { 

    }

    public virtual void OnEquip()
    {

    }
}
