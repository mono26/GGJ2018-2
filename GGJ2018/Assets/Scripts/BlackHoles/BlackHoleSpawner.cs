using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleSpawner : Spawner
{
    [Header("Black Hole Spawner settings")]
    [SerializeField] float minDistanceFromPlayer = 6;
    [SerializeField] float maxDistanceFromPlayer = 9;

    [Header("Black Hole Spawner components")]
    [SerializeField] GameObject player;
    [SerializeField] AudioClip spawnSound;

    protected override void Update()
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
        Blackhole spawnedBlackhole = GetBlackholeToSpawn();
        float radius = spawnedBlackhole.GetRadius;
        for(int i = 0; i < maxTriesToSpawn; i++)
        {
            Vector3 spawnPosition = CalculateRandomPosition();
            if (IsAFreeSpot(spawnPosition, radius)) 
            {
                PositionBlackhole(spawnedBlackhole, spawnPosition);
                SoundManager.Instance.PlaySoundInPosition(transform.position, spawnSound, 0.5f);
                break;
            }
            else if (i == maxTriesToSpawn - 1){
                ReturnUnabledToSpawnBlackhole(spawnedBlackhole);
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

    private Blackhole GetBlackholeToSpawn()
    {
        Blackhole blackhole = PoolsManager.Instance.GetObjectFromPool<Blackhole>();
        blackhole.transform.position = new Vector2(999,999);
        return blackhole;
    }
    
    private void PositionBlackhole(Blackhole _blackHoleToPosition, Vector3 _spawnPosition)
    {
        _blackHoleToPosition.transform.position = _spawnPosition;
        return;
    }

    private void ReturnUnabledToSpawnBlackhole(Blackhole _blackHoleToReturn)
    {
        PoolsManager.Instance.ReleaseObjectToPool<Blackhole>(_blackHoleToReturn);
        return;
    }
}
