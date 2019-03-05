using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroyComponent : MonoBehaviour 
{
	[SerializeField] int minLifeTime = 5, maxLifeTime = 15;
	[SerializeField] SpawnableObject spawnable = null;
	[SerializeField] AutoDestroyEffect autoDestroyEffect = null;
	[SerializeField] int delayForRelease = 0;

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
		int lifeTime = Random.Range(minLifeTime, maxLifeTime);
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
		spawnable.EnableCollision(false);
		spawnable.DisplayVisuals(false);

		if (autoDestroyEffect != null)
		{
			autoDestroyEffect.SpawnEffect();
		}

		// Release to pool
		spawnable.Release();
	}
}
