using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private static GameController instance;
    public static GameController Instance { get { return instance; } }

    private Ship ship;

    [SerializeField]
    private float scorePoints;
    public float ScorePoints { get { return scorePoints; } }
    [SerializeField]
    private Text scoreText = null;
    [SerializeField]
    private Slider fuelSlider = null;

	// Use this for initialization
	void Start ()
    {
        instance = this;
        ship = GameObject.Find("Player").GetComponent<Ship>();
        fuelSlider.maxValue = ship.Engine.settings.maxFuel;
	}
	
	// Update is called once per frame
	void Update ()
    {
        // Set score text to the actual score number
        scoreText.text = scorePoints.ToString();
        fuelSlider.value = ship.Engine.CurrentFuel;
		// Look if the player score limit has reach
        if (ScorePoints > 3)
        {
            // TODO win level
        }
	}

    public void IncreaseScore()
    {
        scorePoints += 1;
    }

    public void WinGame()
    {
        SceneManager.LoadSceneAsync("Credits");
    }
}
