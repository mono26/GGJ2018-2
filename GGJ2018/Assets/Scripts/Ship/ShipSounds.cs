using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSounds : MonoBehaviour 
{
	[Header("Ship sound settings")]
	[SerializeField] AudioClip raySound = null;
	[SerializeField] AudioClip shieldSound = null;
	[SerializeField] AudioClip fuelRefillSound = null;

	[Header("Ship sound settings")]
	[SerializeField] AudioSource shipAudio = null;
	[SerializeField] Ship ship = null;


	bool isRaySoundOn;
	bool isShieldSoundOn;
	float lastFuelAmount;

	void Awake() 
	{
		if (shipAudio == null)
		{
			shipAudio = GetComponent<AudioSource>();
		}

		if (ship == null)
		{
			ship = GetComponent<Ship>();
		}
	}

	void Start() 
	{
		lastFuelAmount = ship.GetEngineComponent.GetMaxFuel;
	}
	void Update() 
	{
		if (ship.GetRayComponent.IsAlienRayOn && !isRaySoundOn)
		{
			PlayRaySound();
		}
		else if (!ship.GetRayComponent.IsAlienRayOn & isRaySoundOn)
		{
			StopRaySound();
		}

		if (ship.GetShieldComponent.IsShieldOn && !isShieldSoundOn)
		{
			PlayShieldSound();
		}
		else if (!ship.GetShieldComponent.IsShieldOn & isShieldSoundOn)
		{
			StopShieldSound();
		}

		float currentFuel = ship.GetEngineComponent.CurrentFuel;
		if (currentFuel > lastFuelAmount)
		{
			PlayFueldRefilSound();
		}
	}

	void LateUpdate() 
	{
		lastFuelAmount = ship.GetEngineComponent.CurrentFuel;
	}

	public void PlayRaySound()
	{
		isRaySoundOn = true;
		SoundManager.Instance.PlaySoundInObject(ref shipAudio, raySound, 0.7f, true);
	}

	public void PlayShieldSound()
	{
		isShieldSoundOn = true;
		SoundManager.Instance.PlaySoundInObject(ref shipAudio, shieldSound, 0.7f, true);
	}

	public void PlayFueldRefilSound()
	{
		float maxFuel = ship.GetEngineComponent.GetMaxFuel;
		float currentFuel = ship.GetEngineComponent.CurrentFuel;
		float differenceBetweenLastAndCurrent = currentFuel - lastFuelAmount;
		float duration = differenceBetweenLastAndCurrent / maxFuel;
		SoundManager.Instance.PlaySoundInObject(ref shipAudio, fuelRefillSound, 0.3f, false, duration);
	}
	void StopRaySound()
	{
		isRaySoundOn = false;
		shipAudio.Stop();
	}

	void StopShieldSound()
	{
		isShieldSoundOn = false;
		shipAudio.Stop();
	}
}
