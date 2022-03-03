using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavPoint : MonoBehaviour
{
    public Color sphereColor = new Color();

    [Space]

    public float sphereSize = 0.2f;

    //[HideInInspector] public List<NavPoint> connections = new List<NavPoint>();
    public List<NavPoint> connections = new List<NavPoint>();

    bool selected = false;


    void OnDrawGizmos()
    {
        //SPHERE
        Gizmos.color = sphereColor;
        if (selected) Gizmos.color = Color.magenta;
        Gizmos.DrawSphere(gameObject.transform.position, sphereSize);
        foreach (NavPoint navPoint in connections)
        {
            Gizmos.DrawLine(gameObject.transform.position, navPoint.transform.position);
        }

        selected = false;
    }

    void OnDrawGizmosSelected()
    {
        selected = true;
    }

    public void Link(NavPoint navPoint)
    {
        //if (navPoint.gameObject == this.gameObject) return;
        if (connections.Contains(navPoint))
        {
            return;
        }
        connections.Add(navPoint);
    }

    public void RemoveLink(NavPoint navpoint)
    {
        connections.Remove(navpoint);
    }
}
