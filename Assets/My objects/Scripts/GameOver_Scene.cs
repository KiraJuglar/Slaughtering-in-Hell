using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver_Scene : MonoBehaviour
{
    private void Update()
    {
        if (Input.anyKey)
        {
            LoadMainMenu();
        }
    }
    public void LoadGame()
    {

        SceneManager.LoadScene("Level1");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
