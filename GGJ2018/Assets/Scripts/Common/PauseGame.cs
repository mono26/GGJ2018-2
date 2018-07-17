using UnityEngine;

public class PauseGame : MonoBehaviour
{
    /// Puts the game on pause
    public virtual void PauseAction(bool _pause)
    {
        GameManager.Instance.TriggerPause(_pause);

        return;
    }
}
