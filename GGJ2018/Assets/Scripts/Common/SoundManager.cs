using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager> 
{
	[SerializeField] AudioSource backGroundSource;
	public void PlaySoundInPosition(Vector3 _position, AudioClip _sound, float _volume)
	{
		SpawnableSound audio = PoolsManager.Instance.GetObjectFromPool<SpawnableSound>();
		audio.PlaySound(_sound, _volume);
	}

	public void PlaySoundInObject(AudioSource _source, AudioClip _sound, float _volume)
	{
		_source.volume = _volume;
		_source.clip = _sound;
	}

	public void PlayBackGroundSound(AudioClip _sound, float _volume)
	{
		backGroundSource.volume = _volume;
		backGroundSource.clip = _sound;
		backGroundSource.Play();
	}
}
