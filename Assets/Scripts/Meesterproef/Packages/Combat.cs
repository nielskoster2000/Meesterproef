using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : Ability
{
    //Objects
    Camera cam;
    Humanoid humanoid;
    public List<Humanoid> players = new List<Humanoid>();
    public bool combat = false;
    float aimAccuracy = 5f; //The lower the value, the more accurate the bot

    //Variables
    [SerializeField] int health = 100;

    public List<Transform> raycastpoints = new List<Transform>();

    private void Start()
    {
        GetPlayers();
        cam = GetComponentInChildren<Camera>();
        humanoid = GetComponent<Humanoid>();
    }

    void GetPlayers()
    {
        players.Clear();
        players = new List<Humanoid>(gameObject.transform.parent.parent.GetComponentsInChildren<Humanoid>());
    }

    private void Update()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(cam);

        foreach (Humanoid player in players)
        {
            if (player.gameObject != gameObject)
            {
                player.GetComponent<MeshRenderer>().material.color = Color.green;
                combat = false;
                cam.transform.rotation = gameObject.transform.rotation;

                if (GeometryUtility.TestPlanesAABB(planes, player.Bounds)) //Is a player in the camera's view?
                {
                    if (!IsObstructed(player.gameObject))
                    {
                        combat = true;
                        player.GetComponent<MeshRenderer>().material.color = Color.red;
                        FightOpponent(player);
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

    public void FightOpponent(Humanoid opponent)
    {
        //Aim
        Vector3 aimOffset = new Vector3(Random.Range(-aimAccuracy, aimAccuracy), Random.Range(-aimAccuracy, aimAccuracy), Random.Range(-aimAccuracy, aimAccuracy));

     /*   transform.parent.LookAt(opponent.transform.position);
        transform.parent.rotation = Quaternion.Euler(transform.parent.rotation.x, transform.parent.rotation.y, *//*transform.parent.rotation.z*//* 0 );*/

        //pew pew
        
    }
}
