using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDestroy : MonoBehaviour 
{
	[Header("Settings")]
	[SerializeField] int minLifeTime = 5, maxLifeTime = 15;
	[SerializeField] SpawnableObject spawnable = null;
	[SerializeField] AutoDestroyEffect autoDestroyEffect = null;
	[SerializeField] bool enableTimedAutoDestroy = true;

	[Header("Editor debugging")]
	[SerializeField] float lifeTimeCounter;

	public int GetMinLifeTime { get { return minLifeTime; } }

	public int GetMaxLifeTime { get { return maxLifeTime; } }

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
		if (!enableTimedAutoDestroy)
		{
			return;
		}

		int lifeTime = Random.Range(minLifeTime, maxLifeTime);
		lifeTimeCounter = lifeTime;
	}

	void Update() 
	{	
		if (!enableTimedAutoDestroy)
		{
			return;
		}
		
		lifeTimeCounter -= Time.deltaTime;

		if (lifeTimeCounter <= 0)
		{
			AutoDestroyObject();
		}
	}

	public void AutoDestroyObject()
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

	public void SetLifeTime(float _lifeTime)
	{
		lifeTimeCounter = _lifeTime;
	}
}
