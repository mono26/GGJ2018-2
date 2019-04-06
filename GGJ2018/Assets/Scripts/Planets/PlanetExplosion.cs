using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetExplosion : AutoDestroyEffect 
{
	[SerializeField] int numberOfFragmentsToSpawn = 5;
	[SerializeField] float explosionForce = 1000;
	[SerializeField] float minScale = 1;
	[SerializeField] float maxScale = 3;

	public override void SpawnEffect()
	{
		SpawnFragments();
		SpawnGiantBlackhole();
	}
	void SpawnFragments()
	{
		float angleIncrease = 360/numberOfFragmentsToSpawn;
		float startAngle = Random.Range(0, 359);
		for (int i = 0; i < numberOfFragmentsToSpawn; i++)
		{
			Asteroid fragment = PoolsManager.Instance.GetObjectFromPool<Asteroid>();
			ScaleRandomly(ref fragment);
			fragment.TurnCollisionOffForSeconds(0.5f);
			Vector2 launchDirection = CalculateLaunchDirection(startAngle + (i * angleIncrease));
			fragment.transform.position = transform.position;
			fragment.GetBodyComponent.AddForce(launchDirection * explosionForce, ForceMode2D.Impulse);
		}
	}

	Vector2 CalculateLaunchDirection(float _angle)
	{
		Vector3 launcheDirection = Vector2.zero;
		float x = Mathf.Cos(Mathf.Deg2Rad * _angle);
		float y = Mathf.Sin(Mathf.Deg2Rad * _angle);
		launcheDirection = new Vector2(x, y);
		return launcheDirection;
	}

	void SpawnGiantBlackhole()
	{
		GiantBlackhole giantBlackHole = PoolsManager.Instance.GetObjectFromPool<GiantBlackhole>();
		giantBlackHole.transform.position = transform.position;
	}

	void ScaleRandomly(ref Asteroid _asteroidToScale)
    {
        float randomScale = Random.Range(minScale, maxScale);
        _asteroidToScale.transform.localScale = new Vector3(randomScale, randomScale, 1);
    }
}
