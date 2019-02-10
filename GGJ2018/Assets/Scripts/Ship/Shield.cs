using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : ShipComponent 
{
	[Header("Shield settings")]
	[SerializeField] float maxDuration = 5.0f;

	[Header("Shield components")]
	[SerializeField] GameObject shieldGameObject;

	[Header("Editor debugging")]
	[SerializeField] float duration;
	[SerializeField] bool isShieldOn = false;
    public bool IsShieldOn { get { return isShieldOn; } }

	void Start() 
	{
		shieldGameObject.SetActive(false);
		isShieldOn = false;
	}
	public override void EveryFrame()
	{
		if (isShieldOn)
		{
			duration -= Time.deltaTime;
			if(duration <= 0)
			{
				ToggleShield();
			}
		}

		if (!isShieldOn)
		{
			duration += Time.deltaTime;
		}

		duration = Mathf.Clamp(duration, 0, maxDuration);
	}

	public void ToggleShield()
	{
		isShieldOn = !isShieldOn;

		if (shieldGameObject == null)
		{
			Debug.LogError(gameObject.name + " is missing in Component " + name + " shield GameObject reference.");
			return;
		}

		shieldGameObject.SetActive(isShieldOn);
	}
}
