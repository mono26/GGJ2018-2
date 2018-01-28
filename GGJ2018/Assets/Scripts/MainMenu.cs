using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayGame()
    {
        StartCoroutine(LoadGame());
    }

    public void Credits()
    {
        // TODO load credits screen
    }

    private IEnumerator LoadGame()
    {
        var load = SceneManager.LoadSceneAsync(1);
        while(load.isDone)
        {
            // TODO loading screen
        }
        yield return null;
    }
}
