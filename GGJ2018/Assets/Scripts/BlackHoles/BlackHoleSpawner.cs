using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleSpawner : Spawner
{
    [Header("Black Hole Spawner settings")]
    [SerializeField] private float minDistanceFromPlayer = 6;
    [SerializeField] private float maxDistanceFromPlayer = 9;

    [Header("Black Hole Spawner components")]
    [SerializeField] protected GameObject player;

    protected override void Update()
    {
        base.Update();
        if (spawnTimer <= 0) {
            Spawn();
        }
        return;
	}

    protected override void Spawn()
    {
        BlackHole spawnedBlackHole = GetBlackHoleToSpawn();
        float radius = spawnedBlackHole.GetSpawnableComponent.GetRadius;
        for(int i = 0; i < maxTriesToSpawn; i++)
        {
            Vector3 spawnPosition = CalculateRandomPosition();
            if (IsAFreeSpot(spawnPosition, radius)) 
            {
                PositionBlackHole(spawnedBlackHole, spawnPosition);
                break;
            }
            else if (i == maxTriesToSpawn - 1){
                ReturnUnabledToSpawnBlackHole(spawnedBlackHole);
            }
        }
        base.Spawn();
        return;
    }

    private Vector3 CalculateRandomPosition()
    {
        float randomDistance = Random.Range(minDistanceFromPlayer, maxDistanceFromPlayer);
        float xDistance = Random.Range(-randomDistance, randomDistance);
        float yDistance = Random.Range(-randomDistance, randomDistance);
        Vector3 spawnPosition = player.transform.position + new Vector3(xDistance, yDistance);
        return spawnPosition;
    }

    private BlackHole GetBlackHoleToSpawn()
    {
        BlackHole blackhole = BlackHolePool.Instance.GetBlackHole();
        blackhole.transform.position = new Vector2(999,999);
        return blackhole;
    }
    
    private void PositionBlackHole(BlackHole _blackHoleToPosition, Vector3 _spawnPosition)
    {
        _blackHoleToPosition.transform.position = _spawnPosition;
        return;
    }

    private void ReturnUnabledToSpawnBlackHole(BlackHole _blackHoleToReturn)
    {
        BlackHolePool.Instance.ReleaseBlackHole(_blackHoleToReturn);
        return;
    }
}
