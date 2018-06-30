using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private static GameController instance;
    public static GameController Instance { get { return instance; } }

    private Ship ship;

    //TODO create a class for managing UI only

    [SerializeField]
    private int initialScorePoints = 0;
    [SerializeField]
    private int maxScorePoints = 30;
    private int scorePoints;
    public int ScorePoints { get { return scorePoints; } }
    [SerializeField]
    private Text scoreText = null;

    [SerializeField]
    private Image fuelBar = null;

    private void Awake()
    {
        if (instance)
        {
            Destroy(instance);
            instance = this;
        }

        instance = this;

        scoreText.gameObject.SetActive(false);
        fuelBar.gameObject.SetActive(false);
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
    }

    public void WinGame()
    {
        ScreenManager.Instance.StartCoroutine
            (
                ScreenManager.Instance.LoadLevel("Credits")
            );
    }

    public void LoseGame()
    {
        ScreenManager.Instance.StartCoroutine
            (
                ScreenManager.Instance.LoadLevel("Credits")
            );
    }
}
