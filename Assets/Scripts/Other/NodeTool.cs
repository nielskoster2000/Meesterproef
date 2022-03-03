#if (UNITY_EDITOR) 

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NavPoint))]
[CanEditMultipleObjects]
public class NodeTool : Editor
{
    bool linked = false;
    //Draw in inspector
    public override void OnInspectorGUI()
    {
        //OnClick button
        if (GUILayout.Button("Snap"))
        {
            //Loop through selected objects
            GameObject[] selectedObjects = Selection.gameObjects;
            for (int i = 0; i < selectedObjects.Length; i++)
            {
                Vector3 pos = selectedObjects[i].transform.position;

                RaycastHit[] hits;
                hits = Physics.RaycastAll(pos, Vector3.down, Mathf.Infinity);

                for (int j = 0; j < hits.Length; j++)
                {
                    if (hits[j].transform.gameObject != selectedObjects[i])
                    {
                        selectedObjects[i].transform.position = new Vector3(pos.x, hits[j].point.y, pos.z);
                        break;
                    }
                }
            }
        }

        if (!linked)
        {
            if (GUILayout.Button("Link"))
            {
                linked = true;

                //Loop through selected objects
                GameObject[] selectedObjects = Selection.gameObjects;
                for (int i = 0; i < selectedObjects.Length; i++)
                {
                    if (selectedObjects[i].TryGetComponent(out NavPoint navPoint1))
                    {
                        for (int j = 0; j < selectedObjects.Length; j++)
                        {
                            if (selectedObjects[j].TryGetComponent(out NavPoint navPoint2))
                            {
                                if (navPoint1 != navPoint2)
                                {
                                    navPoint2.Link(navPoint1);
                                }
                            }
                        }
                    }
                }
            }
        }
        else
        {
            if (GUILayout.Button("UnLink"))
            {
                linked = false;

                //Loop through selected objects
                GameObject[] selectedObjects = Selection.gameObjects;
                for (int i = 0; i < selectedObjects.Length; i++)
                {
                    if (selectedObjects[i].TryGetComponent(out NavPoint navPoint1))
                    {
                        for (int j = 0; j < selectedObjects.Length; j++)
                        {
                            if (selectedObjects[j].TryGetComponent(out NavPoint navPoint2))
                            {
                                navPoint2.connections.Clear();
                            }
                        }
                    }
                }
            }
        }

        GUILayout.Space(30);
        base.OnInspectorGUI();
    }
}

#endif