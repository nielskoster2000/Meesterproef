using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] int userDefinedBotCount = 0;
    [SerializeField] List<Humanoid> players = new List<Humanoid>();
    GameObject playersParent;

    private void Start()
    {
        playersParent = GameObject.Find("Players");
        SpawnBot("Test", new Vector3(-1.5f, -0.15f, -3f), Vector3.zero, new List<Ability>());
    }

    private void SpawnBots(int botcount)
    {

    }

    private void SpawnBot(string botname = "bot", Vector3 pos = default(Vector3), Vector3 rotation = default(Vector3), List<Ability> abilities = null)
    {
        GameObject bot = Instantiate<GameObject>(Resources.Load<GameObject>("Bot"));

        //Add bot to players 
        bot.transform.parent = playersParent.transform;
        players.Add(bot.GetComponent<Humanoid>());

        //Set name
        bot.GetComponent<Humanoid>().username = botname;

        //Set pos and rotation
        if (pos != null) { bot.transform.position = pos; } else { bot.transform.position = default(Vector3); }
        if (rotation != null) { bot.transform.rotation = Quaternion.Euler(rotation); } else { bot.transform.rotation = Quaternion.Euler(default(Vector3)); }

        //Add abilities 
        if (abilities != null)
        {
            foreach (Ability ability in abilities)
            {
                Ability ab = bot.AddComponent<Ability>(); 
                ab = ability;
            }
        }
/*        if (ability1 != null) {  Ability ab = bot.AddComponent<Ability>(); ab = ability1; }
        if (ability2 != null) {  Ability ab = bot.AddComponent<Ability>(); ab = ability2; }
        if (ability3 != null) {  Ability ab = bot.AddComponent<Ability>(); ab = ability3; }*/
    }
}
