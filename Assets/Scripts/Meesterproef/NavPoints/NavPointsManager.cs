using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class NavPointsManager : MonoBehaviour
{
    public static List<NavPoint> navPoints = new List<NavPoint>();
    [SerializeField] List<NavPoint> visibleNavpoints = new List<NavPoint>();

    private void Awake()
    {
        UpdateNavPointsList();
    }

    void UpdateNavPointsList()
    {
        foreach (NavPoint navPoint in transform.GetComponentsInChildren<NavPoint>())
        {
            navPoints.Add(navPoint);
        }

        visibleNavpoints = navPoints;
    }

    private void OnDestroy()
    {
        navPoints.Clear();
    }
}

