using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Combat : MonoBehaviour
{
    //Objects
    Camera cam;
    Humanoid humanoid;
    public Inventory inventory;
    public List<Humanoid> players = new List<Humanoid>();
    public bool combat = false;
    float aimAccuracy = 5f; //The lower the value, the more accurate the bot
    Humanoid currentEnemy = null;

    public List<Transform> raycastpoints = new List<Transform>();

    private void Start()
    {
        GetPlayers();
        cam = GetComponentInChildren<Camera>();
        humanoid = GetComponent<Humanoid>();
        inventory = humanoid.Inventory;
    }

    public void GetPlayers()
    {
        players.Clear();
        players = new List<Humanoid>(gameObject.transform.parent.parent.GetComponentsInChildren<Humanoid>());
    }

    private void Update()
    {
        if (currentEnemy != null)
        {
            combat = true;
            Attack();
        }
        else
        {
            combat = false;
            currentEnemy = Detect();
        }
    }

    Humanoid Detect()
    {
        if (inventory.selectedWeapon >= 0)
        {
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);

            foreach (Humanoid player in players)
            {
                if (player.gameObject != gameObject)
                {
                    if (GeometryUtility.TestPlanesAABB(planes, player.Bounds)) //Is a player in the camera's view?
                    {
                        if (!IsObstructed(player.gameObject)) //Can the bot see the player wihout obstruction?
                        {
                            if (currentEnemy == null)
                            {
                                return player;
                            }
                        }
                    }
                    else
                    {
                        currentEnemy = null; //Enemy is no longer visible
                    }
                }
            }
        }

        return null;
    }

    public void Attack()
    {
        //Aim
        Vector3 aimOffset = new Vector3(Random.Range(-aimAccuracy, aimAccuracy), Random.Range(-aimAccuracy, aimAccuracy), Random.Range(-aimAccuracy, aimAccuracy));

        Quaternion newRotation = Quaternion.LookRotation(currentEnemy.transform.position - transform.position, Vector3.forward);
        newRotation.x = 0f;
        newRotation.z = 0f;

        transform.rotation = newRotation;

        if (inventory.weapons[inventory.selectedWeapon] != null)
        {
            inventory.weapons[inventory.selectedWeapon].Use();
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
}
