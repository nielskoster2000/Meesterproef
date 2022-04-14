using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

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

    //Weapon details
    [SerializeField] bool automatic;
    [SerializeField] public Type type;
    [SerializeField] public int ammo;
    [SerializeField] float cooldownTime;
    bool cooldown;
    [SerializeField] int damage;
    [SerializeField] public int ammoPickupAmount;
    public bool inHand = false;

    [HideInInspector] public UnityEvent onFire;

    //Components
    ParticleSystem attackEffect;
    AudioSource attackSound;
    Animation attackAnimation;
    public Camera playerCamera = null;

    int humanoidLayer;
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
                    CheckHit();
                    ammo--;
                    break;
                case Type.projectile:
                    //Shoot projectile
                    ammo--;
                    break;
                default:
                    break;
            }



            onFire.Invoke();
            ActivateWeaponEffects();
            StartCoroutine(Cooldown());
        }
    }

    private void Awake()
    {
        humanoidLayer = LayerMask.GetMask("Humanoid");

        //Try getting these components
        TryGetComponent<ParticleSystem>(out attackEffect);
        TryGetComponent<AudioSource>(out attackSound);
        TryGetComponent<Animation>(out attackAnimation);

        if (attackSound != null)
        {
            attackSound.volume = Settings.volume * 0.0005f;
        }
    }

    private void Update()
    {
        if (!Settings.gamePaused)
        {
            if (inHand)
            {
                if (automatic)
                {
                    if (Input.GetMouseButton(0))
                    {
                        Use();
                    }
                }
                else
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        Use();
                    }
                }
            }
        }


    }

    public override void OnEquip()
    {
        //playerCamera = transform.parent.parent.GetComponentInParent<Camera>();
        playerCamera = GameManager.FindComponentInParentRecursive(transform, typeof(Camera)) as Camera;
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
        if (attackEffect != null)
        {
            attackEffect.Play();
        }

        //Start audio
        if (attackSound != null)
        {
            attackSound.Play();
        }

        //Start animation
        if (attackAnimation != null)
        {
            attackAnimation.Play();
        }
    }

    private void CheckHit()
    {
        RaycastHit hit;
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, humanoidLayer))
        {
            if (hit.transform.TryGetComponent(out Humanoid humanoid) && humanoid.gameObject)
            {
                humanoid.ChangeHealth(-damage);
                print("Damaged " + humanoid.name);
            }
        }
    }
}
