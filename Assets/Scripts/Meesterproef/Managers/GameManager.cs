using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] List<Level> levels = new List<Level>();
    Level currentLevel;
    [SerializeField] List<GameObject> weapons = new List<GameObject>();
    List<Weapon> inventoryWeapons = new List<Weapon>();
    [SerializeField] int chosenLevel = 0;
    [SerializeField] public int userDefinedBotCount = 0;
    [SerializeField] public int maxPlayerCount = 6; //This is players and bots!
    public static List<Player> players = new List<Player>();
    string[] botNames = new string[] { "Steve", "Maik", "Rik", "John", "Micheal" };
    GameObject playersParent;
    User user = null;
    public static float remainingGameTime;

    int playerCount = 0;

    public List<AudioSource> audioSources = new List<AudioSource>();

    private void Start()
    {
        foreach (GameObject level in Resources.LoadAll<GameObject>("Levels/"))
        {
            levels.Add(level.GetComponent<Level>());
        }

        foreach (GameObject item in weapons)
        {
            inventoryWeapons.Add(item.GetComponentInChildren<Weapon>());
        }

        SceneManager.sceneLoaded += LoadLevel;

        GetAudioComponents();
        SetAudioLevels();
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

    public void SetBotCount(int amount)
    {
        userDefinedBotCount = Mathf.Clamp(amount, 0, maxPlayerCount);
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
    }

    public void LoadLevel(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "MainMenu")
        {       
            //Load Level
            string levelString = "Levels/Level" + chosenLevel.ToString();
            GameObject levelObject = Resources.Load<GameObject>(levelString);
            currentLevel = Instantiate(levelObject, new Vector3(0, 0, 0), Quaternion.identity).GetComponent<Level>();
            RenderSettings.skybox = levels[chosenLevel].sky;

            FillLevel();
        }
    }

    private string GetRandomBotName()
    {
        return botNames[Random.Range(0, botNames.Length)];
    }

    public void FillLevel()
    {
        //Add players
        SetPlayerParent();

        //Bots
        SpawnPlayers(userDefinedBotCount);

        //Actual player
        Player player = new Player(Settings.username);
        player.humanoid = SpawnHumanoid(player, true, levels[chosenLevel].GetRandomSpawnPoint(), default, new List<Weapon> { inventoryWeapons[0] });
        player.humanoid.player = player;
        players.Add(player);

        user = player.humanoid.GetComponent<User>();
        user.leaderboard.SetLeaderboardSlots();

        foreach (Player p in players)
        {
            p.humanoid.OnDeath.AddListener(user.leaderboard.SortSlots);
        }



        //Add other stuff?
        if (Settings.MatchDuration > 0)
        {
            StartCoroutine(startMatchTime());
        }

        GetAudioComponents();
        SetAudioLevels();
    }

    public void SetPlayerParent()
    {
        playersParent = GameObject.Find("Players");
    }

    private void SpawnPlayers(int playerCount)
    {
        for (int i = 0; i < playerCount; i++)
        {
            Player player = new Player(GetRandomBotName());
            player.humanoid = SpawnHumanoid(player, false, levels[chosenLevel].GetRandomSpawnPoint(), default, new List<Weapon> { inventoryWeapons[0] });
            player.humanoid.player = player;
            players.Add(player);
        }
    }

    public void ClearPlayers()
    {
        foreach (Player player in players)
        {
            Destroy(player.humanoid);
        }

        players.Clear();
    }

    public Humanoid RespawnHumanoid(Player player)
    {
        Humanoid humanoid = SpawnHumanoid(player, player.IsHuman, levels[chosenLevel].GetRandomSpawnPoint(), default, new List<Weapon>() { inventoryWeapons[0] });

        return humanoid;
    }

    IEnumerator StartRespawn(Player player)
    {
        DestroyImmediate(player.humanoid.transform.parent.gameObject);
        player.humanoid = null;


        //Update humanoid list in combat
        foreach (Player p in players)
        {
            if (p.humanoid != null)
            {
                Combat combat = p.humanoid.gameObject.GetComponent<Combat>();
                if (combat != null) { combat.GetPlayers(); }
            }
        }

        yield return new WaitForSeconds(3);

        player.humanoid = RespawnHumanoid(player);
        player.humanoid.player = player;
        player.humanoid.OnDeath.AddListener(user.leaderboard.SortSlots);
    }

    private Humanoid SpawnHumanoid(Player player, bool isHuman = false, NavPoint pos = default, Vector3 rotation = default, List<Weapon> weapons = null)
    {
        GameObject newPlayer;

        if (isHuman)
        {
            newPlayer = Instantiate(Resources.Load<GameObject>("Player"));
            //newPlayer.GetComponentInChildren<Camera>().targetDisplay = 1;
        }
        else
        {
            newPlayer = Instantiate(Resources.Load<GameObject>("Bot_root"));
            newPlayer.GetComponentInChildren<Camera>().targetDisplay = playerCount + 1;
        }

        Humanoid newPlayerHumanoid = newPlayer.GetComponentInChildren<Humanoid>();

        newPlayerHumanoid.OnDeath.AddListener(delegate { 
            StartCoroutine(StartRespawn(player));
        });

        if (Settings.MatchMaxKills > 0)
        {
            newPlayerHumanoid.OnMaxKillsReached.AddListener(delegate
            {
                StartCoroutine(EndGame());
            }
            );
        }

        //Add to players 
        newPlayer.transform.parent = playersParent.transform;

        //Set pos and rotation
        if (pos != null) { newPlayer.transform.position = pos.transform.position; } else { newPlayer.transform.position = default; }
        if (rotation != null) { newPlayer.transform.rotation = Quaternion.Euler(rotation); } else { newPlayer.transform.rotation = Quaternion.Euler(default); }

        Inventory inventory = newPlayer.GetComponentInChildren<Humanoid>().Inventory;

        if (weapons != null)
        {
            //Add items to the spawned bot
            foreach (Weapon weapon in weapons)
            {
                GameObject newWeapon = Instantiate(weapon.transform.parent.gameObject, inventory.hand.transform);
                Item newWeaponItem = newWeapon.transform.GetComponentInChildren<Item>();
                inventory.Pickup(newWeaponItem);
                inventory.Equip((Weapon)newWeaponItem);
            }
        }

        inventory.Equip(inventory.weapons[inventory.selectedWeapon]);

        if (isHuman)
        {
            UI userInventory = (UI)inventory;
            userInventory.SelectSlot(inventory.selectedWeapon);
        }
        
        playerCount++;

        return newPlayerHumanoid;
    }

    public static GameObject FindChildRecursive(Transform transform, string name)
    {
        foreach (Transform child in transform)
        {
            if (child.name == name)
            {
                return child.gameObject;
            }

            GameObject obj = FindChildRecursive(child, name);
            if (obj != null) return obj;
        }

        return null;
    }

    public static GameObject FindParentRecursive(Transform transform, string name)
    {
        foreach (Transform child in transform)
        {
            if (child.name == name)
            {
                return child.gameObject;
            }

            GameObject obj = FindChildRecursive(child, name);
            if (obj != null) return obj;
        }

        return null;
    }

    public static Component FindComponentInParentRecursive(Transform transform, System.Type type)
    {
        Component c = transform.GetComponent(type);
        if (c != null)
        {
            return c;
        }
        else
        {
            if (transform.parent == null)
            {
                return null;
            }

            return FindComponentInParentRecursive(transform.parent, type);
        }
    }

    public IEnumerator startMatchTime()
    {
        StartCoroutine(startCountdown());
        remainingGameTime = Settings.MatchDuration;
        yield return new WaitForSeconds(Settings.MatchDuration);
        StartCoroutine(EndGame());
    }

    public IEnumerator startCountdown()
    {
        yield return remainingGameTime -= Time.deltaTime;
    }


    public void SetAudioLevels()
    {
        foreach (AudioSource audioSource in audioSources)
        {
            audioSource.volume = Settings.volume * 0.01f; //Convert it to 0-1 
        }
    }

    public void GetAudioComponents()
    {
        audioSources.Clear();

        foreach (AudioSource audioSource in Resources.FindObjectsOfTypeAll(typeof(AudioSource)))
        {
            audioSources.Add(audioSource);
        }
    }

    public IEnumerator EndGame()
    {
        Settings settings = Managers.settingsInstance;
        //Set leaderboard
        user.leaderboard.gameObject.SetActive(true);
        user.leaderboard.transform.parent = currentLevel.gameoverCanvas.transform;
        user.leaderboard.transform.localPosition = new Vector3(0, 0, 0);
        user.leaderboard.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));
        user.leaderboard.transform.localScale = new Vector3(-1, 1, 1);

        //Set cams
        user.GetComponent<Humanoid>().cam.enabled = false;
        currentLevel.gameOverCamera.enabled = true;
        user.enabled = false;

        //Pause game
        Settings.PauseGame(true);

        //Wait 10 secs
        yield return new WaitForSeconds(10);

        //Clear players and such
        ClearPlayers();
        user.leaderboard.Clear();
        SceneManager.LoadScene("MainMenu");
        Settings.PauseGame(false);
        user.ShowCursor(true); 
    }

    public static void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #endif

        Application.Quit();
    }
}
