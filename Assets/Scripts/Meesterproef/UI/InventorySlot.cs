using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    Weapon weapon = null;
    Sprite icon = null;
    Image selectionBox = null;
    Image weaponIcon = null;

    public bool Selected
    {
        set { selectionBox.enabled = value; }
    }

    public Weapon Weapon
    {
        get { return weapon; }
        set { weapon = value; icon = value.icon; }
    }

    public Sprite Icon
    {
        set { weaponIcon.sprite = value; }
    }

    private void Awake()
    {
        selectionBox = transform.GetChild(0).GetComponent<Image>();
        weaponIcon = transform.GetChild(1).GetComponent<Image>();
    }
}
