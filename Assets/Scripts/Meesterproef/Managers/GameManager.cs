using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] List<Level> levels = new List<Level>();
    [SerializeField] int chosenLevel = 0;
    [SerializeField] int userDefinedBotCount = 0;
    [SerializeField] int maxPlayerCount = 6; //This is players and bots!
    [SerializeField] List<Humanoid> players = new List<Humanoid>();
    string[] maleBotNames = new string[] { "Steve", "Maik", "Rik", "John" };
    string[] femaleBotNames = new string[] { "Charlotte", "Emma", "Noami", "Karen" };
    GameObject playersParent;

    int playerCount = 0;

    private void Start()
    {
        foreach (GameObject level in Resources.LoadAll<GameObject>("Levels/"))
        {
            levels.Add(level.GetComponent<Level>());
        }
    }

    public void ChangeBotCount(int amount)
    {
        userDefinedBotCount += amount;

        if (userDefinedBotCount < 0)
        {
            userDefinedBotCount = 0;
        }
        else if (userDefinedBotCount > maxPlayerCount)
        {
            userDefinedBotCount = maxPlayerCount;
        }
    }

    public void UpdateBotCount(Text text)
    {
        text.text = userDefinedBotCount.ToString();
    }

    public void UpdateLevelThumbnail(Image image)
    {
        image.sprite = levels[chosenLevel].thumbnail;
    }

    public void UpdateLevelName(Text text)
    {
        text.text = levels[chosenLevel].levelName;
    }

    public void ChangeLevel(int amount)
    {
        chosenLevel += amount;

        if (chosenLevel < 0)
        {
            chosenLevel = 0;
        }
        else if (chosenLevel > levels.Count - 1)
        {
            chosenLevel = levels.Count - 1;
        }
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);

        if (sceneName != "MainMenu")
        {
            SceneManager.sceneLoaded += LoadLevel;
        }
    }

    public void LoadLevel(Scene scene, LoadSceneMode mode)
    {
        //Load Level
        string levelString = "Levels/Level" + chosenLevel.ToString();
        GameObject level = Resources.Load<GameObject>(levelString);
        Instantiate(level, new Vector3(0, 0, 0), Quaternion.identity);
        RenderSettings.skybox = levels[chosenLevel].sky;

        FillLevel();
    }

    private string GetRandomBotName(bool male)
    {
        if (male)
        {
            return maleBotNames[Random.Range(0, maleBotNames.Length - 1)];
        }
        else
        {
            return femaleBotNames[Random.Range(0, femaleBotNames.Length -1)];
        }
    }

    public void FillLevel()
    {
        //Add players
        SetPlayerParent();
        SpawnPlayer(true, Settings.username, levels[chosenLevel].GetRandomSpawnPoint()); //Spawn Player
        SpawnPlayers(userDefinedBotCount);
        //Add other stuff?
    }

    public void SetPlayerParent()
    {
        playersParent = GameObject.Find("Players");
    }

    private void SpawnPlayers(int playerCount)
    {
        for (int i = 0; i < playerCount; i++)
        {
            SpawnPlayer(false, GetRandomBotName(true), levels[chosenLevel].GetRandomSpawnPoint());
        }
    }

    private void SpawnPlayer(bool isHuman = false, string name = "bot", NavPoint pos = default(NavPoint), Vector3 rotation = default(Vector3), List<Item> weapons = null, int equipItem = -1)
    {
        GameObject newPlayer;

        if (isHuman)
        {
            newPlayer = Instantiate<GameObject>(Resources.Load<GameObject>("Player"));
            //newPlayer.GetComponentInChildren<Camera>().targetDisplay = 1;
        }
        else
        {
            newPlayer = Instantiate<GameObject>(Resources.Load<GameObject>("Bot_root"));
            newPlayer.GetComponentInChildren<Camera>().targetDisplay = playerCount + 1;
        }

        newPlayer.GetComponentInChildren<Humanoid>().username = name;

        //Add to players 
        newPlayer.transform.parent = playersParent.transform;
        players.Add(newPlayer.GetComponent<Humanoid>());


        //Set pos and rotation
        if (pos != null) { newPlayer.transform.position = pos.transform.position; } else { newPlayer.transform.position = default(Vector3); }
        if (rotation != null) { newPlayer.transform.rotation = Quaternion.Euler(rotation); } else { newPlayer.transform.rotation = Quaternion.Euler(default(Vector3)); }

        Inventory inventory = newPlayer.GetComponentInChildren<Humanoid>().Inventory;

/*        //Add items to the spawned bot
        foreach (Item item in items)
        {
            GameObject newItem = Instantiate<GameObject>(item.transform.parent.gameObject);
            botInventory.Pickup(newItem.transform.GetComponentInChildren<Item>());
        }*/
/*
        //If equipItem is set to something higher than -1, we will try to equip the weapon which has been selected by the int in the inventory
        if (equipItem > -1 && botInventory.weapons.Contains([weapons.equipItem]))
        {
            botInventory.Equip(items[equipItem]);
        }*/
       
        playerCount++;
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif

        Application.Quit();
    }
}
