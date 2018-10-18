using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner : MonoBehaviour
{
    [SerializeField] protected float spawnTimer;
    [SerializeField] protected int minTimeToSpawn = 0;
    [SerializeField] protected int maxTimeToSpawn = 10;
    [SerializeField] protected int maxTriesToSpawn = 10;

    private void Start()
    {
        spawnTimer = Random.Range(minTimeToSpawn, maxTimeToSpawn);
        return;
    }

    protected virtual void Update()
    {
        spawnTimer -= Time.deltaTime;
        return;
    }

    protected bool IsAFreeSpot(Vector3 _positionToCheck, Vector2 _objectSize)
    {
        bool isAFreeSpot = false;
        RaycastHit2D freeSpotHit = Physics2D.BoxCast(_positionToCheck, _objectSize, 0, Vector2.zero, 0);
        if (freeSpotHit.collider == null) {
            isAFreeSpot = true;
        }
        return isAFreeSpot;
    }
    protected virtual void Spawn()
    {
        spawnTimer = Random.Range(minTimeToSpawn, maxTimeToSpawn);
        return;
    }
}
