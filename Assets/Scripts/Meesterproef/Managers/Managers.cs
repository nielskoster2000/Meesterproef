using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    public static Managers instance;
    public static GameManager gameManagerInstance;
    public static Settings settingsInstance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }    
        else
        {
            instance = this;
            gameManagerInstance = gameObject.GetComponentInChildren<GameManager>();
            settingsInstance = gameObject.GetComponentInChildren<Settings>();
            DontDestroyOnLoad(this);
        }
    }
}
