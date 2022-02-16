using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour
{
    public UnityEvent OnEnter;
    public UnityEvent OnStay;
    public UnityEvent OnExit;
    private GameObject triggerer = null;

    private void OnTriggerEnter(Collider other)
    {
        OnEnter.Invoke();
    }

    private void OnTriggerStay(Collider other)
    {
        OnStay.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        OnExit.Invoke();
    }
}
