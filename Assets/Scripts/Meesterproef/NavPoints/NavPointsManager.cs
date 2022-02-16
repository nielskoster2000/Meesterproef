using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class NavPointsManager : MonoBehaviour
{
    public static List<NavPoint> navPoints = new List<NavPoint>();

    private void Start()
    {
        UpdateNavPointsList();
    }

    void UpdateNavPointsList()
    {
        foreach (NavPoint navPoint in transform.GetComponentsInChildren<NavPoint>())
        {
            navPoints.Add(navPoint);
        }
    }

    public void GenerateGizmos()
    {
        foreach (NavPoint navPoint in transform.GetComponentsInChildren<NavPoint>())
        {
            navPoint.connections.Clear();

            foreach (NavPoint np in transform.GetComponentsInChildren<NavPoint>())
            {
                navPoint.connections.Add(np);
                navPoint.sphereColor = Color.green;
            }
        }
    }

    public void ClearGizmos()
    {
        foreach (NavPoint navPoint in transform.GetComponentsInChildren<NavPoint>())
        {
            navPoint.connections.Clear();
            navPoint.sphereColor = Color.red;
        }
    }
}

namespace Utils.EditorExtension
{
    [CustomEditor(typeof(NavPointsManager))]

    public class NavPointsEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            NavPointsManager navPointsManager = (NavPointsManager)target;

            if (GUILayout.Button("Generate NavMesh Gizmos"))
            {
                navPointsManager.GenerateGizmos();
            }

            if (GUILayout.Button("Clear NavMesh Gizmos"))
            {
                navPointsManager.ClearGizmos();
            }
        }
    }
}



