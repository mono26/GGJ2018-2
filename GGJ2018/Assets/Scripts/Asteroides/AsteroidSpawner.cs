using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : Spawner
{
    [Header("Asteroid settings")]
    [SerializeField] protected float maxForce = 3500f;
    [SerializeField] protected float minForce = 1500f;

    [Header("Components")]
    [SerializeField] protected GameObject player = null;
    [SerializeField] protected Transform[] spawnPoints = null;

    private void Awake()
    {
        if (player == null)
        {
            player = GameObject.Find("PFB_BobTheGreenAlien");
        }

        if (spawnPoints == null)
        {
            GameObject[] sPoints = GameObject.FindGameObjectsWithTag("AsteroidSpawnPoint");
            spawnPoints = new Transform[sPoints.Length];
            for (int i = 0; i < sPoints.Length; i++)
            {
                spawnPoints[i] = sPoints[i].GetComponent<Transform>();
            }
        }

    }
	
	protected override void Update ()
    {
        base.Update();
        if (spawnTimer <= 0) 
        {
            Spawn();
        }
        return;
	}

    public override void Spawn()
    {
        Asteroid spawnedAsteroid = GetAsteroidToSpawn();
        float radius = spawnedAsteroid.GetRadius;
        for(int i = 0; i < maxTriesToSpawn; i++)
        {
            Vector3 spawnPosition = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
            if(IsAFreeSpot(spawnPosition, radius)) 
            {
                LaunchAsteroid(spawnedAsteroid, spawnPosition);
                break;
            }
            else if (i == maxTriesToSpawn - 1){
                ReturnUnabledToSpawnAsteroid(spawnedAsteroid);
            }
        }
        base.Spawn();
        return;
    }

    private Asteroid GetAsteroidToSpawn()
    {
        Asteroid asteroid = PoolsManager.Instance.GetObjectFromPool<Asteroid>();
        asteroid.transform.position = new Vector2(999,999);
        return asteroid;
    }

    private void LaunchAsteroid(Asteroid _asteroidToLaunch, Vector3 _spawnPosition)
    {
        float force = Random.Range(minForce, maxForce);
        Vector2 unitDirection = (player.transform.position - _spawnPosition).normalized;
        _asteroidToLaunch.transform.position = _spawnPosition;
        _asteroidToLaunch.GetBodyComponent.AddForce(unitDirection * force, ForceMode2D.Force);
    }

    private void ReturnUnabledToSpawnAsteroid(Asteroid _asteroidToReturn)
    {
        PoolsManager.Instance.ReleaseObjectToPool<Asteroid>(_asteroidToReturn);
    }
}
