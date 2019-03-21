using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidSpawner : Spawner
{
    [Header("Settings")]

    //Speed values for random speed
    [SerializeField] float slowSpeed = 1600f;
    [SerializeField] float midSpeed = 1600f;
    [SerializeField] float fastSpeed = 1600f;
    [SerializeField] float maxSpeed = 3500f;

    //Probability distribution for random speed
    [SerializeField] float slowProbability;
    [SerializeField] float midProbability;
    [SerializeField] float fastProbability;
    [SerializeField] float maxProbability;

    //Probability distribution for random amount
    [SerializeField] float oneProbability;
    [SerializeField] float twoProbability;
    [SerializeField] float threeProbability;

    [SerializeField] float damageFormulaConstant;

    [SerializeField] float minScale = 1;
    [SerializeField] float maxScale = 2;
    
    [SerializeField] int minNumberOfAsteroids = 1;
    [SerializeField] int maxNumberOfAsteroids = 3;

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

        int numberOfAsteroids = GetRandomAmount();
        Vector3[] lastSpawnPositions = new Vector3[numberOfAsteroids];
        for (int i = 0; i < numberOfAsteroids; i++)
        {
            Asteroid spawnedAsteroid = GetAsteroidToSpawn();
           
            float radius = spawnedAsteroid.GetRadius;
            lastSpawnPositions[i] = Vector3.zero;
            for(int j = 0; j < maxTriesToSpawn; j++)
            {
                Vector3 spawnPosition = Vector3.zero;
                while (AlreadySpawnedInPosition(lastSpawnPositions, spawnPosition))
                {
                    spawnPosition = spawnPoints[Random.Range(0, spawnPoints.Length)].position;
                }

                if(IsAFreeSpot(spawnPosition, radius)) 
                {
                    LaunchAsteroid(spawnedAsteroid, spawnPosition);
                    lastSpawnPositions[i] = spawnPosition;
                    break;
                }
                else if (i == maxTriesToSpawn - 1)
                {
                    ReturnUnabledToSpawnAsteroid(spawnedAsteroid);
                }
            }
        }

        base.Spawn();
    }

    bool AlreadySpawnedInPosition(Vector3[] _positions, Vector3 _positionToCheck)
    {
        bool alreadySpawned = false;
        foreach (Vector3 position in _positions)
        {
            if (position.Equals(_positionToCheck))
            {
                alreadySpawned = true;
            }
        }

        return alreadySpawned;
    }

    Asteroid GetAsteroidToSpawn()
    {
        Asteroid asteroid = PoolsManager.Instance.GetObjectFromPool<Asteroid>();
        ScaleRandomly(ref asteroid);

        return asteroid;
    }

    void LaunchAsteroid(Asteroid _asteroidToLaunch, Vector3 _spawnPosition)
    {
        float force = GetRandomSpeed();
        float size = _asteroidToLaunch.transform.localScale.magnitude;
        _asteroidToLaunch.GetComponent<DamageOnTouch>().SetDamageOnTouch(GetAsteroidDamage(force, size));
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

    //Gets damage for spawned asteroid based on size and speed
    float GetAsteroidDamage(float speed, float size)
    {
        float damage = damageFormulaConstant * speed * size;

        return damage;
    }

    //Gets random amount for asteroids to spawn based on probability distribution
    int GetRandomAmount()
    {
        int amount;

        float amountProbability = Random.Range(0.0f, 1.1f);
        Debug.Log(amountProbability);

        if (amountProbability < oneProbability)
        {
            amount = 1;
            return amount;
        }
        else if (amountProbability < oneProbability + twoProbability)
        {
            amount = 2;
            return amount;
        }
        else
        {
            amount = 3;
            return amount;
        }
    }

    //Gets random speed for asteroids based on probability distribution
    float GetRandomSpeed()
    {
        float speed;

        float speedProbability = Random.Range(0.0f, 1.1f);
        Debug.Log(speedProbability);

        if (speedProbability < slowProbability)
        {
            speed = slowSpeed;
            return speed;
        }
        else if (speedProbability < slowProbability + midProbability)
        {
            speed = midSpeed;
            return speed;
        }
        else if (speedProbability < slowProbability + midProbability + fastProbability)
        {
            speed = fastSpeed;
            return speed;
        }
        else
        {
            speed = maxSpeed;
            return speed;
        }
    }
}
