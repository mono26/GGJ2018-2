using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    //TODO create a class for managing UI only
    [Header("Level Manager settings")]
    [SerializeField]
    protected AudioClip loseSfx = null;
    [SerializeField]
    protected string mainMenuScene = null;
    [SerializeField]
    private int maxScorePoints = 30;
    [SerializeField]
    protected AudioClip winSfx = null;

    [Header("Components")]
    [SerializeField]
    private Image fuelBar;
    [SerializeField]
    private Text scoreText; //TODO move this to the LevelUIManager

    [Header("Editor debbuging")]
    private int scorePoints;

    protected override void Awake()
    {
        base.Awake();

        if(scoreText != null)
            scoreText.gameObject.SetActive(false);
        if (fuelBar != null)
            fuelBar.gameObject.SetActive(false);

        return;
    }

    void Start ()
    {
        if (scoreText != null)
            scoreText.gameObject.SetActive(true);
        if (fuelBar != null)
            fuelBar.gameObject.SetActive(true);

        scorePoints = 0;

        return;
	}
	
    public void IncreaseScore()
    {
        scorePoints += 1;

        if (scoreText != null)
        {
            scoreText.text = scorePoints.ToString();
        }

        if (scorePoints >= maxScorePoints)
        {
            EndGame();
        }
    }

    public void EndGame()
    {
        //TODO activate end game screen.
        LoadManager.LoadScene("Credits");
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
        //EventManager.TriggerEvent(new GameEvent(GameEventTypes.UnPause));
        LoadManager.LoadScene(SceneManager.GetActiveScene().name);
        return;
    }
}
