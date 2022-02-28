using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Navigation : MonoBehaviour
{
    NavPoint currentNavPoint = null;
    NavPoint targetNavPoint = null;
    NavMeshAgent navMeshAgent = null;
    [SerializeField] int travelQueueBuffer = 1; 
    Queue<NavPoint> traveledNavPoints = new Queue<NavPoint>();

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        if (!Settings.gamePaused)
        {
            if (!navMeshAgent.enabled)
            {
                navMeshAgent.enabled = true;
            }

            if (TargetReached())
            {
                traveledNavPoints.Enqueue(targetNavPoint);

                if (traveledNavPoints.Count > travelQueueBuffer)
                {
                    traveledNavPoints.Dequeue();
                }

                currentNavPoint = targetNavPoint;
                SetNewTarget();
            }
        }
        else
        {
            navMeshAgent.enabled = false;
        }
    }

   void SetNewTarget()
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

    bool TargetReached()
    {
        return !navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && (navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude < 1f);
    }
}
