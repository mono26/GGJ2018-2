using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public bool IsPaused { get; protected set; }

    public void TriggerPause(bool _pause)
    {
        IsPaused = _pause;
        if (IsPaused == true)
        {
            LevelUIManager.Instance.ActivatePauseUI(true);
            Time.timeScale = 0;
            return;
        }
        else if (IsPaused == false)
        {
            LevelUIManager.Instance.ActivatePauseUI(false);
            Time.timeScale = 1;
            return;
        }
    }

}
