using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnableSound : Spawnable 
{
	[SerializeField] AudioSource sourceOfAudio;

	private float duration;

	void Update() 
	{
		// Mientras el sonido se este reproduciendo hacer nada
		// Cuando termine el sonido Release()
		if (duration > 0)
		{
			duration -= Time.deltaTime;
		}
		else
		{
			Release();
		}
	}
	public override void Release()
	{
		PoolsManager.Instance.ReleaseObjectToPool<SpawnableSound>(this);
	}

	public override void ResetState() 
	{
		// Reset sound audio source properties
	}

	public void PlaySound(AudioClip _sound, float _volume)
	{
		SetVolume(_volume);
		sourceOfAudio.clip = _sound;
		sourceOfAudio.Play();
		duration = _sound.length;
	}

	void SetVolume(float _volumeToSet)
	{
		sourceOfAudio.volume = _volumeToSet;
	}
}
