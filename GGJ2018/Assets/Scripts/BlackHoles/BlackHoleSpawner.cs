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
        if (!DebugMenu.Instance.GetBlackholesEnabled)
        {
            return;
        }

        base.Update();

        if (spawnTimer <= 0) 
        {
            Spawn();
        }
	}

    public override void Spawn()
    {
        Blackhole spawnedBlackhole = GetBlackholeToSpawn();
        float radius = spawnedBlackhole.GetGravFieldRadius;
        for(int i = 0; i < maxTriesToSpawn; i++)
        {
            Vector3 spawnPosition = CalculateRandomPosition();
            if (IsAFreeSpot(spawnPosition, radius)) 
            {
                PositionBlackhole(spawnedBlackhole, spawnPosition);
                SoundManager.Instance.PlaySoundInPosition(transform.position, spawnSound, 0.5f);
                break;
            }
            else if (i == maxTriesToSpawn - 1)
            {
                ReturnUnabledToSpawnBlackhole(spawnedBlackhole);
            }
        }

        base.Spawn();
    }

    private Vector3 CalculateRandomPosition()
    {
        float randomDistance = Random.Range(minDistanceFromPlayer, maxDistanceFromPlayer);
        int angle = Random.Range(0, 360);
        float x = Mathf.Cos(Mathf.Deg2Rad * angle) * randomDistance;
        float y = Mathf.Sin(Mathf.Deg2Rad * angle) * randomDistance;

        Vector3 spawnPosition = player.transform.position + new Vector3(x, y);
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
