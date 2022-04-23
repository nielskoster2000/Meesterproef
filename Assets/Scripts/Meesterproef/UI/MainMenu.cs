using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject gameSetup;
    [SerializeField] GameObject options;
    [SerializeField] GameObject credits;

    [Space]

    [SerializeField] Text levelName;
    [SerializeField] Image levelThumbnail;
    [SerializeField] InputField botCounter;
    [SerializeField] InputField maxKills;
    [SerializeField] InputField timeLimit;

    GameManager gameManager;
    Settings settings;

    private void Awake()
    {
        ApplyVolume();
    }

    public void ClickedPlay()
    {
        gameManager = Managers.gameManagerInstance;
        mainMenu.SetActive(false);
        gameSetup.SetActive(true);
        UpdateLevelPreview();
        gameManager.SetBotCount(0);
        gameManager.UpdateBotCount(botCounter.textComponent);
        botCounter.text = gameManager.userDefinedBotCount.ToString();
    }

    public void ClickedFight()
    {
        gameManager = Managers.gameManagerInstance;

        int.TryParse(maxKills.text, out Settings.MatchMaxKills);
        int.TryParse(botCounter.text, out gameManager.userDefinedBotCount);

        gameManager.LoadScene("GameplayLevel");
        Settings.PauseGame(false);
    }

    public void UpdateLevelPreview()
    {
        gameManager = Managers.gameManagerInstance;
        gameManager.UpdateLevelName(levelName);
        gameManager.UpdateLevelThumbnail(levelThumbnail);
    }

    public void ChangeLevel(int number)
    {
        gameManager = Managers.gameManagerInstance;
        gameManager.ChangeLevel(number);
        UpdateLevelPreview();
    }

    public void ChangeBotCount(int amount)
    {
        gameManager = Managers.gameManagerInstance;

        if (int.TryParse(botCounter.text, out int result))
        {
            if ((amount + result) < 0 || (amount + result) > gameManager.maxPlayerCount)
            {
                amount = 0;
            }

            amount += result;
            botCounter.text = amount.ToString();
            gameManager.userDefinedBotCount = amount;
        }
    }

    public void ChangeMaxKillsCount(int amount)
    {
        if ((amount + Settings.MatchMaxKills) < 0)
        {
            amount = 0;
        }

        //If we can add amount to the number in maxkills.text, we'll add it
        if (int.TryParse(maxKills.text, out int result))
        {
            amount += result;
            maxKills.text = amount.ToString();
            Settings.MatchMaxKills = int.Parse(maxKills.text);
        }
    }

    public void ChangeTimeLimit(int amount)
    {
        if ((amount + Settings.MatchDuration) < 0)
        {
            amount = 0;
        }

        //If we can add amount to the number in timeLimit.text, we'll add it
        if (int.TryParse(timeLimit.text, out int result))
        {
            amount += result;
            timeLimit.text = amount.ToString();
            Settings.MatchDuration = int.Parse(timeLimit.text) * 60;
        }
    }

    public void SetTimeLimit(string text)
    {
        Settings.MatchDuration = int.Parse(text) * 60; 
    }

    public void SetKillLimit(string text)
    {
        Settings.MatchMaxKills = int.Parse(text);
    }

    public void SetBotCount(string text)
    {
        gameManager = Managers.gameManagerInstance;

        if (int.TryParse(text, out int result))
        {
            if (result <= 6 || result >= 0)
            {
                gameManager.userDefinedBotCount = int.Parse(text);
            }
        }
    }

    public void ApplySettings()
    {
        settings = Managers.settingsInstance;
        settings.SaveToPlayerPrefs();
        settings.LoadFromPlayerPrefs();
    }

    public void SaveSetting(InputField inputField)
    {
        settings = Managers.settingsInstance;
        settings.SaveSetting(inputField);
    }

    public void SaveSetting(Slider slider)
    {
        settings = Managers.settingsInstance;
        settings.SaveSetting(slider);
    }

    public void SaveSetting(Toggle toggle)
    {
        settings = Managers.settingsInstance;
        settings.SaveSetting(toggle);
    }

    public void ApplyVolume()
    {
        gameManager = Managers.gameManagerInstance;
        gameManager.GetAudioComponents();
        gameManager.SetAudioLevels();
    }
}
