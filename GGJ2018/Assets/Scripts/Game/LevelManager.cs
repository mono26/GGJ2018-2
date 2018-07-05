using UnityEngine;
using UnityEngine.UI;

public class LevelManager : Singleton<LevelManager>
{
    private Ship ship;

    //TODO create a class for managing UI only

    [SerializeField]
    private int initialScorePoints = 0;
    [SerializeField]
    private int maxScorePoints = 30;
    private int scorePoints;
    public int ScorePoints { get { return scorePoints; } }
    [SerializeField]
    private Text scoreText;

    [SerializeField]
    private Image fuelBar;

    protected override void Awake()
    {
        base.Awake();

        scoreText.gameObject.SetActive(false);
        fuelBar.gameObject.SetActive(false);

        return;
    }
    // Use this for initialization
    void Start ()
    {
        scoreText.gameObject.SetActive(true);
        fuelBar.gameObject.SetActive(true);

        scorePoints = initialScorePoints;
        ship = GameObject.Find("BobTheGreenAlien").GetComponent<Ship>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Set score text to the actual score number
        scoreText.text = scorePoints.ToString();
		// Look if the player score limit has reach
        if (ScorePoints >= maxScorePoints)
        {
            WinGame();
        }
	}

    public void IncreaseScore()
    {
        scorePoints += 1;
        return;
    }

    public void WinGame()
    {
        GameManager.Instance.StartCoroutine
            (
                GameManager.Instance.LoadLevel("Credits")
            );
    }

    public void LoseGame()
    {
        GameManager.Instance.StartCoroutine
            (
                GameManager.Instance.LoadLevel("Credits")
            );
    }
}
