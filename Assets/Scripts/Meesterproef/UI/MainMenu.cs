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

    GameManager gameManager;

    public void ClickedPlay()
    {
        gameManager = Managers.instance.gameObject.GetComponentInChildren<GameManager>();
        mainMenu.SetActive(false);
        gameSetup.SetActive(true);
        UpdateLevelPreview();
    }

    public void ClickedFight()
    {
        gameManager = Managers.instance.gameObject.GetComponentInChildren<GameManager>();
        gameManager.LoadScene("GameplayLevel");

    }

    public void UpdateLevelPreview()
    {
        gameManager.UpdateLevelName(levelName);
        gameManager.UpdateLevelThumbnail(levelThumbnail);
    }
}
