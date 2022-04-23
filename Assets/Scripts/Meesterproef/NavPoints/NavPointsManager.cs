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


    public void GenerateGizmos()
    {
        foreach (NavPoint navPoint in transform.GetComponentsInChildren<NavPoint>())
        {
            navPoint.connections.Clear();
            foreach (NavPoint np in transform.GetComponentsInChildren<NavPoint>())
            {
                navPoint.connections.Add(np);
            }
        }
    }

    public void ClearGizmos()
    {
        foreach (NavPoint navPoint in transform.GetComponentsInChildren<NavPoint>())
        {
            navPoint.connections.Clear();
        }
    }
}

#if UNITY_EDITOR
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
#endif


