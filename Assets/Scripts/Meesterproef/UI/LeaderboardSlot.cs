using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class LeaderboardSlot : MonoBehaviour
{
    public Player player;

    Text lbPlayername;
    Text lbkills;
    Text lbdeaths;

    Leaderboard leaderboard;

    public void SetComponents()
    {
        transform.GetChild(0).TryGetComponent<Text>(out lbPlayername);
        transform.GetChild(1).TryGetComponent<Text>(out lbkills) ;
        transform.GetChild(2).TryGetComponent<Text>(out lbdeaths);

        leaderboard = GameManager.FindComponentInParentRecursive(transform, typeof(Leaderboard)) as Leaderboard;
    }

    public void SetPlayerStats(Player p)
    {
        player = p;

        if (lbdeaths == null)
        {
            SetComponents();
        }

        lbPlayername.text = player.username;
        lbkills.text = player.GetStats().x.ToString();
        lbdeaths.text = player.GetStats().y.ToString();
    }
}
