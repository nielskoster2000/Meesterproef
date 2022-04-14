using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [HideInInspector] public string username;
    [HideInInspector] public Humanoid humanoid;
    Vector2 stats;

    public bool IsHuman { get; private set; } = false;

    public Player(string name = "bot")
    {
        username = name;
    }

    void SetUserName()
    {
        username = Settings.username;
    }
}
