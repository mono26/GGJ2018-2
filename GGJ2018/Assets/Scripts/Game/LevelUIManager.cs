using UnityEngine;

public class LevelUIManager : Singleton<LevelUIManager>
{
    [Header("Game state UI's")]
    [SerializeField]
    private GameObject gameOverUI;
    [SerializeField]
    private GameObject pauseUI;
    [SerializeField]
    private GameObject winUI;
    [SerializeField]
    private GameObject radarButton, rayButton;
    [SerializeField]
    private GameObject movementJoystick, radarFrequencyJoystick;

    protected override void Awake()
    {
        base.Awake();

        Transform inputContainer = transform.Find("Virtual_Input");
        if (radarButton == null) {
            radarButton = inputContainer.Find("Radar_Button").gameObject;
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
        if (winUI == null) {
            winUI = inputContainer.Find("WinGameUI").gameObject;
        }
        if (gameOverUI == null) {
            gameOverUI = inputContainer.Find("GameOverUI").gameObject;
        }
        return;
    }

    protected void Start ()
    {
        ActivatePauseUI(false);
        ActivateGameOverUI(false);
        ActivateWinUI(false);
        return;
    }

    public void ActivatePauseUI(bool _active) { pauseUI.SetActive(_active); }

    public void ActivateGameOverUI(bool _active) { gameOverUI.SetActive(_active); }

    public void ActivateWinUI(bool _active) { winUI.SetActive(_active); }

    public void ActivatePlayerControls(bool _active)
    {
        movementJoystick.SetActive(_active);
        radarButton.SetActive(_active);
        rayButton.SetActive(_active);
    }

    // Encontrar mejor manera de hacer esto. Si por event system o alguna otra manera.
    public void DisplayInputButton(string _buttonToActivate, bool _buttonState)
    {
        switch (_buttonToActivate)
        {
            case "RadarButton":
            {
                radarButton.SetActive(_buttonState);
                break;
            }
            case "RayButton":
            {
                rayButton.SetActive(_buttonState);
                break;
            }
            default:
            {
                break;
            }
        }
    }
}
