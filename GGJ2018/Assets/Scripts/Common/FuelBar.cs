using UnityEngine;
using UnityEngine.UI;

public class FuelBar : MonoBehaviour
{
    /// the healthbar's foreground bar
    [SerializeField]
    protected Image progressBar;
    [SerializeField]
    protected float _percent;

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
