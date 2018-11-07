﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : Spawner
{
    [Header("Asteroid settings")]
    [SerializeField] protected float maxForce = 3500f;
    [SerializeField] protected float minForce = 1500f;

    [Header("Components")]
    [SerializeField] protected GameObject player;
    [SerializeField] protected Transform[] spawnPoints;

    private void Awake()
    {
        player = GameObject.Find("BobTheGreenAlien");
        GameObject[] sPoints = GameObject.FindGameObjectsWithTag("AsteroidSpawnPoint");
        spawnPoints = new Transform[sPoints.Length];
        for (int i = 0; i < sPoints.Length; i++) {
            spawnPoints[i] = sPoints[i].GetComponent<Transform>();
        }
        return;
    }
	
	protected override void Update ()
    {
        base.Update();
        if (spawnTimer <= 0) {
            Spawn();
        }
        return;
	}

    protected override void Spawn()
    {
        Asteroid spawnedAsteroid = GetAsteroidToSpawn();
        float radius = spawnedAsteroid.GetSpawnableComponent.GetRadius;
        Vector2 asteroidSize = new Vector2(radius, radius);
        for(int i = 0; i < maxTriesToSpawn; i++)
        {
            Vector3 spawnPosition = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
            if(IsAFreeSpot(spawnPosition, asteroidSize)) 
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
        Asteroid asteroid = AsteroidPool.Instance.GetAsteroide();
        asteroid.transform.position = new Vector2(999,999);
        return asteroid;
    }

    private void LaunchAsteroid(Asteroid _asteroidToLaunch, Vector3 _spawnPosition)
    {
        float force = Random.Range(minForce, maxForce);
        Vector2 unitDirection = (player.transform.position - _spawnPosition).normalized;
        _asteroidToLaunch.transform.position = _spawnPosition;
        _asteroidToLaunch.GetBodyComponent.AddForce(unitDirection * force, ForceMode2D.Force);
        return;
    }

    private void ReturnUnabledToSpawnAsteroid(Asteroid _asteroidToReturn)
    {
        AsteroidPool.Instance.ReleaseAsteroide(_asteroidToReturn);
        return;
    }
}