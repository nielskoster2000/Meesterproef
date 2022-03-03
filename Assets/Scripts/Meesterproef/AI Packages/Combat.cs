using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : Ability
{
    //Objects
    Camera cam;
    List<CapsuleCollider> players = new List<CapsuleCollider>();
    public bool combat = false;

    //Variables
    //[SerializeField] int health = 100;
    //[SerializeField] float eyeDistance = 0.1f;
    public List<Transform> raycastpoints = new List<Transform>();

    private void Start()
    {
        GetPlayers();
        cam = GetComponentInChildren<Camera>();
    }

    void GetPlayers()
    {
        players.Clear();
        players = new List<CapsuleCollider>(gameObject.transform.parent.GetComponentsInChildren<CapsuleCollider>());
    }

    private void Update()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);

        foreach (CapsuleCollider player in players)
        {
            if (player.gameObject != gameObject)
            {
                player.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
                combat = false;

                if (GeometryUtility.TestPlanesAABB(planes, player.bounds)) //Is a player in the camera's view?
                {
                    if (!IsObstructed(player.gameObject))
                    {
                        //Do stuff here
                        player.GetComponent<MeshRenderer>().material.color = Color.red;
                        combat = true;
                        break;
                    }
                }
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
}
