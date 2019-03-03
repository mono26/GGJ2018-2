using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyComponent : MonoBehaviour 
{
	[SerializeField] int lifeTime = 10;
	[SerializeField] SpawnableObject spawnable = null;
	[SerializeField] AutoDestroyEffect autoDestroyEffect = null;
	[SerializeField] int delayForRelease = 0;

	Coroutine autoDestroyProcess;
	float lifeTimeCounter;

	void Awake() 
	{
		if (spawnable == null)
		{
			spawnable = GetComponent<SpawnableObject>();
		}

		if (autoDestroyEffect == null)
		{
			autoDestroyEffect = GetComponent<AutoDestroyEffect>();
		}
	}

	void OnEnable() 
	{
		lifeTimeCounter = lifeTime;
	}

	void Update() 
	{
		lifeTimeCounter -= Time.deltaTime;

		if (lifeTimeCounter <= 0)
		{
			AutoDestroy();
		}
	}

	public void AutoDestroy()
	{
		autoDestroyProcess = StartCoroutine(AutoDestroyWithDelay());
	}

	IEnumerator AutoDestroyWithDelay()
	{
		spawnable.EnableCollision(false);
		spawnable.DisplayVisuals(false);

		if (autoDestroyEffect != null)
		{
			autoDestroyEffect.SpawnEffect();
		}

		yield return new WaitForSeconds(delayForRelease);

		// Release to pool
		spawnable.Release();
	}
}
