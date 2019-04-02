using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager> 
{
	[SerializeField] AudioSource backGroundSource;
	public void PlaySoundInPosition(Vector3 _position, AudioClip _sound, float _volume, bool _loop = false)
	{
		SpawnableSound audio = PoolsManager.Instance.GetObjectFromPool<SpawnableSound>();
		audio.PlaySound(_sound, _volume, _loop);
	}

	public void PlaySoundInObject(ref AudioSource _source, AudioClip _sound, float _volume, bool _loop = false, float _duration = 1.0f)
	{
		_source.volume = _volume;
		_source.loop = _loop;

		PlayPlaySoundForCertainAmountOfTime(_source, _sound, _duration);
	}

	public void PlayPlaySoundForCertainAmountOfTime(AudioSource _source, AudioClip _sound, float _duration)
	{
		float soundDuration = _sound.length;
		float _percentageToPlay = soundDuration * _duration;
		_source.clip = _sound;
		_source.SetScheduledEndTime(AudioSettings.dspTime + _percentageToPlay);
		_source.Play();
	}

	public void PlayBackGroundSound(AudioClip _sound, float _volume, float _duration = 1.0f, bool _loop = false)
	{
		backGroundSource.volume = _volume;
		if (_duration == 1.0f && _loop)
		{
			backGroundSource.loop = _loop;
		}

		PlayPlaySoundForCertainAmountOfTime(backGroundSource, _sound, _duration);
	}
}
