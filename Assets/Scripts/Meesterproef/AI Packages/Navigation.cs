using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Navigation : MonoBehaviour
{
    NavPoint currentNavPoint = null;
    NavPoint targetNavPoint = null;
    NavMeshAgent navMeshAgent = null;
    [SerializeField] int travelQueueBuffer = 5; 
    Queue<NavPoint> traveledNavPoints = new Queue<NavPoint>();

    private void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        SetNewTarget();
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

    void SetNewTargetAccordingToPath(int tried = 0)
    {
        NavPoint destination = currentNavPoint.connections[Random.Range(0, currentNavPoint.connections.Count)];
        if (traveledNavPoints.Contains(destination) || tried < currentNavPoint.connections.Count)
        {
            SetNewTargetAccordingToPath(tried + 1);
        }
        else
        {
            targetNavPoint = destination;
        }

        navMeshAgent.destination = targetNavPoint.transform.position;
    }

    bool TargetReached()
    {
        return !navMeshAgent.pathPending && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance && (navMeshAgent.hasPath || navMeshAgent.velocity.sqrMagnitude < 1f);
    }
}
