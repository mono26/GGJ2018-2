using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ScreenManager : MonoBehaviour
{
    private static ScreenManager instance;
    public static ScreenManager Instance { get { return instance; } }

    [SerializeField]
    private float minDuration = 1.5f;

    private void Awake()
    {
        if (instance)
        {
            Destroy(instance);
            instance = this;
        }

        instance = this;
    }

    public IEnumerator LoadLevel(string _level)
    {
        yield return SceneManager.LoadSceneAsync("Load Scene");

        bool continueToLevel = false;

        // Load level async
        yield return SceneManager.LoadSceneAsync(_level, LoadSceneMode.Additive);

        var text = GameObject.Find("State Text");
        text.GetComponent<Text>().text = "Click to continue";

        while (!continueToLevel)
        {
            if (Input.GetMouseButton(0))
            {
                continueToLevel = true;
                yield return null;
            }
            yield return null;
        }

        yield return SceneManager.UnloadSceneAsync("Load Scene");
    }
}
