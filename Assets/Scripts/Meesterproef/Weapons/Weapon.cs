using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type
    {
        melee,
        ranged
    }

    [SerializeField] public bool automatic;
    [SerializeField] public Type type;
    [SerializeField] public int ammo;
    [SerializeField] public float cooldownTime;
    [SerializeField] public bool cooldown;
    [SerializeField] public int damage;

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

    public virtual void Use()
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


}
