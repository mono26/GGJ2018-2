using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetExplosion : AutoDestroyEffect 
{
	[SerializeField] int numberOfFragmentsToSpawn = 5;
	[SerializeField] float explosionForce = 1000;
	[SerializeField] int timeToSpawnGiantBlackHole = 5;

	Coroutine giantBlackSpawnProcess;

	public override void SpawnEffect()
	{
		SpawnFragments();
		SpawnGiantBlackhole();
	}
	void SpawnFragments()
	{
		float angleIncrease = 365/numberOfFragmentsToSpawn;
		float startAngle = Random.Range(0, 364);
		for (int i = 0; i < numberOfFragmentsToSpawn; i++)
		{
			Asteroid fragment = PoolsManager.Instance.GetObjectFromPool<Asteroid>();
			fragment.TurnCollisionOffForSeconds(1.5f);
			Vector2 launchDirection = CalculateLaunchDirection(startAngle + (i*angleIncrease));
			fragment.transform.position = transform.position;
			fragment.GetBodyComponent.AddForce(launchDirection * explosionForce, ForceMode2D.Impulse);
		}
	}

	Vector2 CalculateLaunchDirection(float _angle)
	{
		Vector3 launcheDirection = Vector2.zero;
		float x = Mathf.Cos(_angle);
		float y = Mathf.Sin(_angle);
		launcheDirection = new Vector2(x, y);
		return launcheDirection;
	}

	void SpawnGiantBlackhole()
	{
		GiantBlackhole giantBlackHole = PoolsManager.Instance.GetObjectFromPool<GiantBlackhole>();
		giantBlackHole.transform.position = transform.position;
	}
}
