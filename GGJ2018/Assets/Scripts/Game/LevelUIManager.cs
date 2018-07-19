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

    public void ActivatePauseUI(bool _active) { pauseUI.SetActive(_active); }

    public void ActivateGameOverUI(bool _active) { gameOverUI.SetActive(_active); }

    public void ActivateWinUI(bool _active) { winUI.SetActive(_active); }
}
