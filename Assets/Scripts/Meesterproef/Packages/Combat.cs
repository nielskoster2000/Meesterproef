using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Combat : Ability
{
    //Objects
    Camera cam;
    Humanoid humanoid;
    Inventory inventory;
    public List<Humanoid> players = new List<Humanoid>();
    public bool combat = false;
    float aimAccuracy = 5f; //The lower the value, the more accurate the bot
    GameObject currentEnemy = null;

    //Variables
    private float aimTime = 0.0f;

    public List<Transform> raycastpoints = new List<Transform>();
    GameObject spine;

    private void Start()
    {
        GetPlayers();
        cam = GetComponentInChildren<Camera>();
        humanoid = GetComponent<Humanoid>();
        inventory = humanoid.Inventory;

        spine = GameManager.FindChildRecursive(transform, "Spine"); 
    }

    public void GetPlayers()
    {
        players.Clear();
        players = new List<Humanoid>(gameObject.transform.parent.parent.GetComponentsInChildren<Humanoid>());
    }

    private void Update()
    {
        if (inventory.selectedWeapon >= 0)
        {
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);

            foreach (Humanoid player in players)
            {
                if (player.gameObject != gameObject)
                {
                    combat = false;

                    if (GeometryUtility.TestPlanesAABB(planes, player.Bounds)) //Is a player in the camera's view?
                    {
                        if (!IsObstructed(player.gameObject))
                        {
                            combat = true;

                            if (currentEnemy == null || player.gameObject == currentEnemy)
                            {
                                FightOpponent(player);
                            }

                            break;
                        }
                    }
                    else
                    {
                        currentEnemy = null; //Enemy is no longer visible
                    }
                }
            }

            if (!combat)
            {
                //cam.transform.rotation = gameObject.transform.rotation;
            }
        }
    }

    bool IsObstructed(GameObject target, float eyeOffset = 0f)
    {
        RaycastHit hit;
        Vector3 pos = cam.transform.position + (target.transform.right * eyeOffset);
        Vector3 dir = (target.transform.position - pos).normalized;

        if (Physics.Raycast(pos, dir, out hit, Mathf.Infinity)) //Send raycast to check if the player in the camera's view isn't behind a wall or other obstruction
        {
            if (hit.collider.gameObject == target.gameObject) //Check that we're hitting an actual player
            {
                Debug.DrawRay(pos, dir * hit.distance);
                return false;
            }
        }

        return true;
    }

    public void FightOpponent(Humanoid opponent)
    {
        currentEnemy = opponent.gameObject;

        //Aim
        Vector3 aimOffset = new Vector3(Random.Range(-aimAccuracy, aimAccuracy), Random.Range(-aimAccuracy, aimAccuracy), Random.Range(-aimAccuracy, aimAccuracy));

        Quaternion newRotation = Quaternion.LookRotation(opponent.transform.position - transform.position, Vector3.forward);
        newRotation.x = 0f;
        //newRotation.y = 0f;
        newRotation.z = 0f;

        //spine.transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, aimTime);
        //spine.transform.rotation = newRotation;

        //aimTime += Time.deltaTime;

        print("selected weapon: "+inventory.selectedWeapon);
        if (inventory.weapons[inventory.selectedWeapon] != null)
        {
            inventory.weapons[inventory.selectedWeapon].Use();
        }
    }
}
