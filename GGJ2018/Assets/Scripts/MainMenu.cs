using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    GameObject howToPlayPanel;

	// Use this for initialization
	void Start () {
		
	}

    public void PlayGame()
    {
        StartCoroutine(LoadLevel("Espacio"));
    }

    public void Credits()
    {
        // TODO load credits screen
        StartCoroutine(LoadLevel("Credits"));
    }

    public void GoToMainMenu()
    {
        StartCoroutine(LoadLevel("Main Menu"));
    }

    private IEnumerator LoadLevel(string _level)
    {
        var load = SceneManager.LoadSceneAsync(_level);
        Debug.Log("se empieza a cargar el nivel");
        if (_level == "Espacio")
        {
            //load.allowSceneActivation = false;
            howToPlayPanel.SetActive(true);
            yield return null;
        }

        while (load.progress < 0.9f)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //load.allowSceneActivation = true;
                Debug.Log(load.allowSceneActivation);
                yield return null;
            }
            // TODO loading screen
        }
        yield return null;
    }
}
