using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    /// the healthbar's foreground bar
    [SerializeField]
    protected Image progressBar;
    [SerializeField]
    protected float _percent;
    [SerializeField]
    protected Color fullGasColor;
    [SerializeField]
    protected Color midGasColor;
    [SerializeField]
    protected Color lowGasColor;

    protected void Awake()
    {
        if(progressBar == null)
            progressBar = transform.Find("ProgressFill").GetComponent<Image>();
        progressBar.color = fullGasColor;
        return;
    }

    public virtual void UpdateBar(float currentValue, float maxValue)
    {
        _percent = currentValue / maxValue;
        if (progressBar != null)
        {
            progressBar.fillAmount = _percent;
        }
        if (_percent < 1)
            progressBar.color = fullGasColor;

        if (_percent < 0.6)
            progressBar.color = midGasColor;

        if (_percent < 0.3)
            progressBar.color = lowGasColor;
        


        return;
    }
}
