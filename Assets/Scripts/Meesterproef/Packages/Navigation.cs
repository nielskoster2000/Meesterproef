using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Navigation : MonoBehaviour
{
    NavPoint currentNavPoint = null;
    NavPoint targetNavPoint = null;
    NavMeshAgent navMeshAgent = null;
    [SerializeField] int travelQueueBuffer;
    Queue<NavPoint> traveledNavPoints = new Queue<NavPoint>();
    Combat combatPackage;
    Animator animator;

    private void Start()
    {
        navMeshAgent = gameObject.transform.parent.GetComponent<NavMeshAgent>();
        combatPackage = GetComponent<Combat>();
        animator = GetComponent<Animator>();
        SetClosestTarget();
    }

    private void Update()
    {
        if (!Settings.gamePaused)
        {
            if (!navMeshAgent.enabled)
            {
                navMeshAgent.enabled = true;
            }

            if (!animator.enabled)
            {
                animator.enabled = true;
            }

            bool outOfAmmo = combatPackage.inventory.weapons[combatPackage.inventory.selectedWeapon].ammo <= 0;
            if (combatPackage.combat == false || outOfAmmo) //If combat is not active
            {
                Pathfinding();
            }
            else
            {
                CombatMovement();
            }
        }
        else
        {
            navMeshAgent.enabled = false;
            animator.enabled = false;
        }
    }

    public void Pathfinding()
    {
        if (!animator.enabled)
        {
            animator.enabled = true;
        }

        if (TargetReached())
        {
            if (!traveledNavPoints.Contains(targetNavPoint))
            {
                traveledNavPoints.Enqueue(targetNavPoint);
            }

            if (traveledNavPoints.Count > travelQueueBuffer)
            {
                traveledNavPoints.Dequeue();
            }

            currentNavPoint = targetNavPoint;
            SetNewTargetAccordingToPath();
        }
    }

    public void CombatMovement()
    {
        if (animator.enabled)
        {
            animator.enabled = false;
        }

        //Combat navigation here
        navMeshAgent.destination = gameObject.transform.position;
    }

   void SetClosestTarget()
    {
        float closestDistance = 999999f;

        foreach (NavPoint navPoint in NavPointsManager.navPoints)
        {
            if (!traveledNavPoints.Contains(navPoint))
            {
                float distance = Vector3.Distance(gameObject.transform.position, navPoint.transform.position);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    targetNavPoint = navPoint;
                }
            }

            navMeshAgent.destination = targetNavPoint.transform.position;
            if (currentNavPoint == null)
            {
                currentNavPoint = targetNavPoint;
            }
        }
    }

    void SetNewTargetAccordingToPath(int tried = 0)
    {
        if (currentNavPoint.connections.Count > 1)
        {
            NavPoint destination = currentNavPoint.connections[Random.Range(0, currentNavPoint.connections.Count)];

            if (traveledNavPoints.Contains(destination) && tried < destination.connections.Count)
            {
                SetNewTargetAccordingToPath(tried + 1);
            }
            else
            {
                targetNavPoint = destination;
            }

            if (traveledNavPoints.Contains(destination) && currentNavPoint.connections.Count == 1)
            {
                traveledNavPoints.Clear();
            }

            navMeshAgent.destination = targetNavPoint.transform.position;
        }
    }

    bool TargetReached()
    {
        return !navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && (navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude < 1f);
    }
}
