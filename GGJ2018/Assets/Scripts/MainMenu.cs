using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private ScreenManager screenManager;

	// Use this for initialization
	void Start () {
		
	}

    public void PlayGame()
    {
        screenManager.StartCoroutine(screenManager.LoadLevel("Espacio"));
    }

    public void Credits()
    {
        screenManager.StartCoroutine(screenManager.LoadLevel("Credits"));
    }

    public void GoToMainMenu()
    {
        screenManager.StartCoroutine(screenManager.LoadLevel("Main Menu"));
    }
}
