using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    [SerializeField] float mouseSens;
    [SerializeField] float volume;

    public static float _mouseSens;
    public static float _volume;

    private void Awake()
    {
        _mouseSens = mouseSens;
        _volume = volume;
    }
}
