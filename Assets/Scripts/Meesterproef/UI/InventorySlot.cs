using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    Weapon weapon = null;
    Sprite icon = null;
    Image selectionBox = null;

    public bool Selected
    {
        set { selectionBox.enabled = value; }
    }

    public Weapon Weapon
    {
        get { return weapon; }
        set { weapon = value; icon = value.icon; }
    }

    private void Awake()
    {
        selectionBox = GetComponentInChildren<Image>();
    }


}
