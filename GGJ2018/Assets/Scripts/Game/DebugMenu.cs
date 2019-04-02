using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugMenu : Singleton<DebugMenu> 
{
	[Header("Settings")]
	[SerializeField] Color isEnabled = Color.white;
	[SerializeField] Color isDisabled = Color.gray;
	[SerializeField] GameObject container;

	[Header("Buttons")]
	[SerializeField] Button debug = null;
	[SerializeField] Button asteroids = null;
	[SerializeField] Button blackholes = null;
	[SerializeField] Button rechargeFuel =  null;

	bool isDebugEnabled = false;
	bool areBlackholesEnabled = true;
	bool areAsteroidsEnabled =  true;

	public bool GetAsteroidsEnabled { get { return areAsteroidsEnabled; } }

	public bool GetBlackholesEnabled { get { return areBlackholesEnabled; } }

	void Start() 
	{
		areAsteroidsEnabled = true;
		areBlackholesEnabled =  true;

		isDebugEnabled = false;

		container.SetActive(false);
	}

	public void ToggleDebugMenu()
	{
		isDebugEnabled = !isDebugEnabled;

		container.SetActive(isDebugEnabled);
	}

	public void TogleAsteroids()
	{
		areAsteroidsEnabled = !areAsteroidsEnabled;

		asteroids.image.color = areAsteroidsEnabled ? isEnabled : isDisabled;
	}

	public void TogleBlackholes()
	{
		areBlackholesEnabled = !areBlackholesEnabled;

		blackholes.image.color = areBlackholesEnabled ? isEnabled : isDisabled;
	}

	public void RechargeFuel()
	{
		ShipEngine engine = FindObjectOfType<ShipEngine>();

		engine.RechargeFuel(999f);
	}
}
