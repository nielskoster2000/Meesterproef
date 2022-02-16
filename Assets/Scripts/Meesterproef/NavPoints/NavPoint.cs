using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavPoint : MonoBehaviour
{
    [HideInInspector] public Color sphereColor = new Color();

    [Space]

    public float sphereSize = 0.2f;

    /*[HideInInspector] */public List<NavPoint> connections = new List<NavPoint>();


    private void OnDrawGizmos()
    {
        //SPHERE
        Gizmos.color = sphereColor;
        Gizmos.DrawSphere(gameObject.transform.position, sphereSize);
    }
}
