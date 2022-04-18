using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    User user;
    Button ResumeGame;
    Button Settings;
    Button RetireGame;
    Button QuitGame;

    private void Awake()
    {
        user = GameManager.FindComponentInParentRecursive(transform, typeof(User)).GetComponent<User>();
        ResumeGame = transform.GetChild(0).GetComponent<Button>();
        Settings = transform.GetChild(1).GetComponent<Button>();
        RetireGame = transform.GetChild(2).GetComponent<Button>();
        QuitGame = transform.GetChild(3).GetComponent<Button>();

        ResumeGame.onClick.AddListener(PauseGame);
        Settings.onClick.AddListener(ToggleSettings);
        RetireGame.onClick.AddListener(Retire);
        QuitGame.onClick.AddListener(GameManager.QuitGame);
    }

    public void PauseGame()
    {
        //user.PauseGame();
        user.ShowCursor(true);
        gameObject.SetActive(false);
    }

    public void ToggleSettings()
    {
        //Perhaps implement later
    }

    public void Retire()
    {
        user.ShowCursor(true);
        SceneManager.LoadScene("MainMenu");
    }
}
