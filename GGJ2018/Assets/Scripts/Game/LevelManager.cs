using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    //TODO create a class for managing UI only
    [Header("Level Manager settings")]
    [SerializeField]
    private int initialScorePoints = 0;
    [SerializeField]
    protected AudioClip loseSfx;
    [SerializeField]
    protected string mainMenuScene;
    [SerializeField]
    private int maxScorePoints = 30;
    [SerializeField]
    protected AudioClip winSfx;

    [Header("Components")]
    [SerializeField]
    private Image fuelBar;
    [SerializeField]
    private Text scoreText;

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

        scorePoints = initialScorePoints;

        return;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Set score text to the actual score number
        if (scoreText != null)
            scoreText.text = scorePoints.ToString();
		// Look if the player score limit has reach
        if (scorePoints >= maxScorePoints)
        {
            EndGame();
        }

        return;
	}

    public void IncreaseScore()
    {
        scorePoints += 1;

        return;
    }

    public void EndGame()
    {
        //TODO activate en game screen.
        //LoadManager.LoadScene("Credits");
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
