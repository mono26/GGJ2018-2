using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadLevel(string _level)
    {
        LoadManager.LoadScene(_level);
        return;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
