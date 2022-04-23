using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    [HideInInspector] public string username;
    [HideInInspector] public Humanoid humanoid;
    Vector2 stats;

    [HideInInspector] public LeaderboardSlot leaderboardSlot;

    public bool IsHuman { get; private set; } = false;

    public Player(string name = "bot")
    {
        username = name;
        //humanoid.OnDeath.AddListener(AddDeath);
    }

    void SetUserName()
    {
        username = Settings.username;
    }

    public void AddKill()
    {
        stats.x++;

        if (stats.x >= Settings.MatchMaxKills)
        {
            humanoid.OnMaxKillsReached.Invoke();
        }
    }

    public void AddDeath(Player player)
    {
        stats.y++;
    }

    public Vector2 GetStats()
    {
        return stats;
    }
}
