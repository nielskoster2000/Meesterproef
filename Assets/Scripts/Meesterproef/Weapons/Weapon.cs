using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : Item
{
    public Sprite icon;

    public enum Type
    {
        melee,
        ranged,
        projectile
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
    [HideInInspector] public bool inHand = false;

    int humanoidLayer;

    void Start()
    {
        humanoidLayer = LayerMask.GetMask("Humanoid");
    }

    private void Update()
    {
        if (inHand)
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
                    ActivateWeaponEffects();
                    CheckHit();
                    ammo--;
                    break;
                case Type.projectile:
                    ActivateWeaponEffects();
                    //Shoot projectile
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

    private void ActivateWeaponEffects()
    {
        //Start particle system
        //Start audio
        //Start animation
    }

    private void CheckHit()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, humanoidLayer))
        {
            if (hit.transform.TryGetComponent<Humanoid>(out Humanoid humanoid))
            {
                humanoid.Health -= damage;
                print("Did damage!");
            }
            else
            {
                print("Couldn't get component!, hit layer " + LayerMask.LayerToName(hit.transform.gameObject.layer));
            }
        }
    }
}
