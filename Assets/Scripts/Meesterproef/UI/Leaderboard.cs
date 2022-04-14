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


    private void Awake()
    {
        lbFFA = transform.GetChild(0).gameObject;
        lbTDM = transform.GetChild(1).gameObject;

        ShowGamemodeLeaderboard();
    }

    public void ShowGamemodeLeaderboard()
    {
        lbFFA.SetActive(System.Convert.ToBoolean(currentGameMode));
        lbTDM.SetActive(!System.Convert.ToBoolean(currentGameMode));
    }
}
