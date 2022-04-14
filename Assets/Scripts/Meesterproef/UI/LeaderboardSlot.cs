using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardSlot : MonoBehaviour
{
    Humanoid playerHumanoid;

    Text lbPlayername;
    Text lbkills;
    Text lbdeaths;

    private void Awake()
    {
        transform.GetChild(0).TryGetComponent<Text>(out lbPlayername);
        transform.GetChild(1).TryGetComponent<Text>(out lbkills) ;
        transform.GetChild(2).TryGetComponent<Text>(out lbdeaths);
    }

    public void GetPlayerStats(Player player)
    {
        if (TryGetComponent<Humanoid>(out Humanoid humanoid))
        {
            lbPlayername.text = player.username;
            playerHumanoid = player.humanoid;

            if (TryGetComponent<User>(out _))
            {
                lbPlayername.text = Settings.username;
            }
        }
    }

    public void UpdatePlayerStats()
    {
        lbkills.text = playerHumanoid.GetStats().x.ToString();
        lbdeaths.text = playerHumanoid.GetStats().y.ToString();
    }
}
