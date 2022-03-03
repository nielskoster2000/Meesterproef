using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] int userDefinedBotCount = 0;
    [SerializeField] List<Humanoid> players = new List<Humanoid>();
    GameObject playersParent;

    int botCount = 0;

    private void Start()
    {
        playersParent = GameObject.Find("Players");
        SpawnBot("Steve", new Vector3(-1.5f, -0.15f, -3f), Vector3.zero);
        SpawnBot("Micheal", new Vector3(10f, -0.15f, -1f), Vector3.zero);
    }

    private void SpawnBots(int botcount)
    {
    }

    private void SpawnBot(string botname = "bot", Vector3 pos = default(Vector3), Vector3 rotation = default(Vector3))
    {
        GameObject bot = Instantiate<GameObject>(Resources.Load<GameObject>("Bot"));

        //Add bot to players 
        bot.transform.parent = playersParent.transform;
        players.Add(bot.GetComponent<Humanoid>());

        //Set bot cam target display
        bot.GetComponentInChildren<Camera>().targetDisplay = botCount + 1;

        //Set name
        bot.GetComponent<Humanoid>().username = botname;

        //Set pos and rotation
        if (pos != null) { bot.transform.position = pos; } else { bot.transform.position = default(Vector3); }
        if (rotation != null) { bot.transform.rotation = Quaternion.Euler(rotation); } else { bot.transform.rotation = Quaternion.Euler(default(Vector3)); }

        botCount++;
    }
}
