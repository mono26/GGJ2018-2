using UnityEngine;
using UnityEngine.UI;

public class LevelUIManager : Singleton<LevelUIManager>
{
    [Header("Game state UI's")]
    [SerializeField]
    private GameObject gameOverUI;
    [SerializeField]
    private GameObject pauseUI;
    [SerializeField]
    private GameObject boostButton, rayButton;
    [SerializeField]
    private GameObject movementJoystick;
    [SerializeField]
    private bool playerInOrbit;
    [SerializeField]
    private bool playerCanBoost;
    [SerializeField]
    private Color buttonActiveColor;
    [SerializeField]
    private Color buttonUnableColor;
    [SerializeField]
    private Image boostButtonImage;


    private void OnEnable()
    {
        Ship.UpdateButton += UpdateButton;
        Ship.PlayerInOrbit += PlayerInOrbit;
        ShipEngine.PlayerCanBoost += PlayerCanBoost;
    }
    private void OnDisable()
    {
        Ship.UpdateButton -= UpdateButton;
        Ship.PlayerInOrbit -= PlayerInOrbit;
        ShipEngine.PlayerCanBoost -= PlayerCanBoost;
    }

    protected override void Awake()
    {
        base.Awake();



        Transform inputContainer = transform.Find("Virtual_Input");
        if (boostButton == null) {
            boostButton = inputContainer.Find("Boost_Button").gameObject;
        }
        if (rayButton == null) {
            rayButton = inputContainer.Find("Ray_Button").gameObject;
        }
        if (movementJoystick == null) {
            movementJoystick = inputContainer.Find("Movement_Joystick").gameObject;
        }
        if (movementJoystick == null) {
            movementJoystick = inputContainer.Find("Frequency_Joystick").gameObject;
        }
        if (pauseUI == null) {
            pauseUI = inputContainer.Find("PauseUI").gameObject;
        }
        if (gameOverUI == null) {
            gameOverUI = inputContainer.Find("GameOverUI").gameObject;
        }
        return;
    }

    protected void Start ()
    {
        playerInOrbit = false;
        PlayerCanBoost();
        ActivatePauseUI(false);
        ActivateGameOverUI(false);
        return;
    }

    public void ActivatePauseUI(bool _active) { pauseUI.SetActive(_active); }

    //Text to store the global score from playerprefs
    private Text globalScoreText;
    private int globalScore;

    public void ActivateGameOverUI(bool _active)
    {
        globalScore = PlayerPrefs.GetInt("GlobalScore");
        gameOverUI.SetActive(_active);
        globalScoreText = gameOverUI.GetComponentInChildren<Text>();
        globalScoreText.text = globalScore.ToString();     
    }

    public void ActivatePlayerControls(bool _active)
    {
        movementJoystick.SetActive(_active);
        boostButton.SetActive(_active);
        rayButton.SetActive(_active);
    }


    private void PlayerCanBoost()
    {

        if (!playerCanBoost)
        {
            playerCanBoost = true;
            boostButtonImage.color = buttonActiveColor;
        }
        else
        {
            playerCanBoost = false;
            boostButtonImage.color = buttonUnableColor;
        }
    }

    private void UpdateButton()
    {
        if (playerInOrbit)
        {
            boostButton.SetActive(false);
            rayButton.SetActive(true);
        }
        else
        {
            boostButton.SetActive(true);
            rayButton.SetActive(false);
        }
    }

    private void PlayerInOrbit()
    {
        if (!playerInOrbit)
            playerInOrbit = true;
        else
            playerInOrbit = false;
    }
}
