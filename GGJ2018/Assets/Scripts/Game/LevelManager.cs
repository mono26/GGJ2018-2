using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    //TODO create a class for managing UI only
    [Header("Level Manager settings")]
    [SerializeField] AudioClip loseSfx = null;
    [SerializeField] string mainMenuScene = null;
    [SerializeField] int maxScorePoints = 30;
    [SerializeField] AudioClip winSfx = null;
    [SerializeField] AudioClip backGroundMusic;

    [Header("Components")]
    [SerializeField] Text scoreText; //TODO move this to the LevelUIManager

    [Header("Editor debbuging")]
    [SerializeField] private int scorePoints;

    protected override void Awake()
    {
        base.Awake();

        if(scoreText != null)
        {
            scoreText.gameObject.SetActive(false);
        }
    }

    void Start ()
    {
        if (scoreText != null)
        {
            scoreText.gameObject.SetActive(true);
        }

        scorePoints = 0;

        SoundManager.Instance.PlayBackGroundSound(backGroundMusic, 1.0f);
	}
	
    public void IncreaseScore()
    {
        scorePoints += 1;


        if (scoreText != null)
        {
            scoreText.text = scorePoints.ToString();
        }

    }

    public void EndGame()
    {
        Time.timeScale = 0;

        if (scorePoints > PlayerPrefs.GetInt("GlobalScore", 0))
        {
            PlayerPrefs.SetInt("GlobalScore", scorePoints);
        }

        LevelUIManager.Instance.ActivateGameOverUI(true);

        return;
    }

    public void QuitLevel()
    {
        Time.timeScale = 1;
        LoadManager.LoadScene(mainMenuScene);
        //SoundManager.Instance.StopSound();
        return;
    }

    public void RetryLevel()
    {
        Time.timeScale = 1;
        //EventManager.TriggerEvent(new GameEvent(GameEventTypes.UnPause));
        LoadManager.LoadScene(SceneManager.GetActiveScene().name);
        return;
    }
}
