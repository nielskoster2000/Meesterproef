#if (UNITY_EDITOR) 

using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(NavPoint))]
public class SnapToFloorEditor : Editor
{
    //Draw in inspector
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(30);

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
    }
}

#endif