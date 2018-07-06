using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    /// the healthbar's foreground bar
    [SerializeField]
    protected Image progressBar;
    [SerializeField]
    protected float _percent;

    protected void Awake()
    {
        if(progressBar == null)
            progressBar = transform.Find("ProgressFill").GetComponent<Image>();

        return;
    }

    public virtual void UpdateBar(float currentValue, float maxValue)
    {
        _percent = currentValue / maxValue;
        if (progressBar != null)
        {
            progressBar.fillAmount = _percent;
        }

        return;
    }
}
