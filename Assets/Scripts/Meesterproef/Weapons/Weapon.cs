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

    [Header("A higher rank means that bots prefer that weapon more")]
    [SerializeField] int rank;

    [Space]

    [SerializeField] bool automatic;
    [SerializeField] Type type;
    [SerializeField] int ammo;
    [SerializeField] float cooldownTime;
    bool cooldown;
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
        if (ammo > 0 && !cooldown)
        {
            switch (type)
            {
                case Type.melee:
                    //Do melee stuff
                    break;
                case Type.ranged:
                    ammo--;
                    break;
                default:
                    break;
            }

            StartCoroutine(Cooldown());
        }
    }

    public virtual IEnumerator Cooldown()
    {
        cooldown = true;
        yield return new WaitForSeconds(cooldownTime);
        cooldown = false;
    }

    public void AddAmmo(int amount)
    {
        ammo += amount;
    }
}
