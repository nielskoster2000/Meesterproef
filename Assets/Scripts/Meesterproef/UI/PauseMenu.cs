using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    User user;

    private void Awake()
    {
        user = GameManager.FindComponentInParentRecursive(transform, typeof(User)).GetComponent<User>();
    }

    public void ResumeGame()
    {
        user.PauseGame(false);
        user.ShowCursor(false);
    }

    public void ToggleSettings()
    {
        //Perhaps implement later
        print("ToggleSettings");
    }

    public void Retire()
    {
        user = GameManager.FindComponentInParentRecursive(transform, typeof(User)).GetComponent<User>();
        Managers.gameManagerInstance.ClearPlayers();
        user.leaderboard.Clear();
        SceneManager.LoadScene("MainMenu");
        user.ShowCursor(true);
        //user.PauseGame(false);
    }

    public void Quit()
    {
        GameManager.QuitGame();
    }
}
