using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item
{
    public enum Type
    {
        melee,
        ranged
    }

    [SerializeField] bool automatic;
    [SerializeField] Type type;
    [SerializeField] int ammo;
    [SerializeField] float cooldownTime;
    [SerializeField] bool cooldown;
    [SerializeField] int damage;
    [SerializeField] public int ammoPickupAmount;

    private void Update()
    {
        if (automatic)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                Use();
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                Use();
            }
        }

    }

    public override void Use()
    {
        if (ammo > 0 && cooldown)
        {
            switch (type)
            {
                case Type.melee:
                    break;
                case Type.ranged:
                    break;
                default:
                    break;
            }

            ammo--;
            StartCoroutine(Cooldown());
        }
    }

    public virtual IEnumerator Cooldown()
    {
        cooldown = false;
        yield return new WaitForSeconds(cooldownTime);
        cooldown = true;
    }

    public void AddAmmo(int amount)
    {
        ammo += amount;
    }
}
