using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : Spawner
{
    [Header("Settings")]
    [SerializeField] float maxForce = 3500f;
    [SerializeField] float minForce = 1500f;
    [SerializeField] float minScale = 1;
    [SerializeField] float maxScale = 2;

    [Header("Components")]
    [SerializeField] GameObject player = null;
    [SerializeField] Transform[] spawnPoints = null;

    void Awake()
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

    Asteroid GetAsteroidToSpawn()
    {
        Asteroid asteroid = PoolsManager.Instance.GetObjectFromPool<Asteroid>();
        ScaleRandomly(ref asteroid);
        return asteroid;
    }

    void LaunchAsteroid(Asteroid _asteroidToLaunch, Vector3 _spawnPosition)
    {
        float force = Random.Range(minForce, maxForce);
        Vector2 unitDirection = (player.transform.position - _spawnPosition).normalized;
        _asteroidToLaunch.transform.position = _spawnPosition;
        _asteroidToLaunch.transform.right = unitDirection;
        _asteroidToLaunch.GetBodyComponent.AddForce(unitDirection * force, ForceMode2D.Force);
    }

    void ReturnUnabledToSpawnAsteroid(Asteroid _asteroidToReturn)
    {
        PoolsManager.Instance.ReleaseObjectToPool<Asteroid>(_asteroidToReturn);
    }

    void ScaleRandomly(ref Asteroid _asteroidToScale)
    {
        float randomScale = Random.Range(minScale, maxScale);
        _asteroidToScale.transform.localScale = new Vector3(randomScale, randomScale, 1);
    }
}
