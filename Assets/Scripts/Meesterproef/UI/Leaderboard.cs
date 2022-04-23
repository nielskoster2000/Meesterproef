using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leaderboard : MonoBehaviour
{
    public enum gamemode
    {
        FFA,
        TDM
    }

    public gamemode currentGameMode = gamemode.FFA;

    //Objects
    GameObject lbFFA;
    GameObject lbTDM;

    GameObject FFASlots;
    //GameObject TDMSlots;

    [SerializeField] GameObject leaderboardSlot;
    public List<LeaderboardSlot> leaderboardSlots = new List<LeaderboardSlot>();

    private void SetObjects()
    {
        lbFFA = transform.GetChild(0).gameObject;
        lbTDM = transform.GetChild(1).gameObject;

        FFASlots = lbFFA.transform.Find("Slots").gameObject;
    }

    public void SetLeaderboardSlots()
    {
        foreach (Player player in GameManager.players)
        {
            AddSlot(player);
        }
    }

    public void Clear()
    {
        foreach (LeaderboardSlot lbslot in leaderboardSlots)
        {
            Destroy(lbslot);
        }

        leaderboardSlots.Clear();
    }

    public void ShowGamemodeLeaderboard()
    {
        lbFFA.SetActive(System.Convert.ToBoolean(currentGameMode));
        lbTDM.SetActive(!System.Convert.ToBoolean(currentGameMode));
    }

    public void AddSlot(Player player)
    {
        if (FFASlots == null)
        {
            SetObjects();
        }

        GameObject newLeaderboardSlot = Instantiate(leaderboardSlot, FFASlots.transform);
        LeaderboardSlot leaderboardSlotComponent = newLeaderboardSlot.GetComponent<LeaderboardSlot>();
        leaderboardSlotComponent.SetPlayerStats(player);
        leaderboardSlots.Add(leaderboardSlotComponent);

        //Sort it correctly
    }

    public void RemoveSlot(int index)
    {
        Destroy(leaderboardSlots[index]);
        leaderboardSlots.RemoveAt(index);
    }

    public void SortSlot(Player player)
    {
        for (int i = leaderboardSlots.Count - 1; i > 0; i--)
        {
            if (leaderboardSlots[i].player.GetStats().x < player.GetStats().x)
            {
                player.leaderboardSlot.transform.SetSiblingIndex(player.leaderboardSlot.transform.GetSiblingIndex() - 1);

                leaderboardSlots.RemoveAt(i);
                leaderboardSlots.Insert(i-1, player.leaderboardSlot);

                SortSlot(player);
            }
        }
    }

    public void SortSlots(Player player = null, Player killer = null)
    {
        int mostkills = -1;

        foreach (LeaderboardSlot lbslot in leaderboardSlots)
        {
            lbslot.SetPlayerStats(lbslot.player);

            if (lbslot.player.GetStats().x > mostkills)
            {
                lbslot.gameObject.transform.SetAsFirstSibling();
                mostkills = Mathf.RoundToInt(lbslot.player.GetStats().x); 
            }
        }
    }

    public void UpdatePlayerSlot(Player player, int index)
    {
        leaderboardSlots[index].SetPlayerStats(player);
    }
}
