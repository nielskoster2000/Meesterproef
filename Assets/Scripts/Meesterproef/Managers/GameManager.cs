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
        for (int i = 0; i < botcount; i++)
        {
            SpawnBot();
        }
    }

    private void SpawnBot(string botname = "bot", Vector3 pos = default(Vector3), Vector3 rotation = default(Vector3), List<Item> items = null, int equipItem = -1)
    {
        GameObject bot = Instantiate<GameObject>(Resources.Load<GameObject>("Bot_root"));

        //Add bot to players 
        bot.transform.parent = playersParent.transform;
        players.Add(bot.GetComponent<Humanoid>());

        //Set bot cam target display
        bot.GetComponentInChildren<Camera>().targetDisplay = botCount + 1;

        //Set name
        bot.GetComponentInChildren<Humanoid>().username = botname;

        //Set pos and rotation
        if (pos != null) { bot.transform.position = pos; } else { bot.transform.position = default(Vector3); }
        if (rotation != null) { bot.transform.rotation = Quaternion.Euler(rotation); } else { bot.transform.rotation = Quaternion.Euler(default(Vector3)); }

        Inventory botInventory = bot.GetComponentInChildren<Humanoid>().Inventory;

/*        //Add items to the spawned bot
        foreach (Item item in items)
        {
            GameObject newItem = Instantiate<GameObject>(item.transform.parent.gameObject);
            botInventory.Pickup(newItem.transform.GetComponentInChildren<Item>());
        }*/

        //If equipItem is set to something higher than -1, we will try to equip the weapon which has been selected by the int in the inventory
        if (equipItem > -1 && botInventory.items.Contains(items[equipItem]))
        {
            botInventory.Equip(items[equipItem]);
        }
       
        botCount++;
    }
}
