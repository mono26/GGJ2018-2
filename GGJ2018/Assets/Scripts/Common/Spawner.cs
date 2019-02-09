using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner : MonoBehaviour
{
    [SerializeField] protected float spawnTimer;
    [SerializeField] int minTimeToSpawn = 0;
    [SerializeField] int maxTimeToSpawn = 10;
    [SerializeField] protected int maxTriesToSpawn = 10;
    [SerializeField] LayerMask planetLayerMask;
    [SerializeField] LayerMask blackholeLayerMask;

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

    protected bool IsAFreeSpot(Vector3 _positionToCheck, float _objectSize)
    {
        bool isAFreeSpot = false;
        RaycastHit2D freeSpotHit = Physics2D.CircleCast(_positionToCheck, _objectSize, Vector2.zero, 0, planetLayerMask | blackholeLayerMask);
        if (freeSpotHit.collider == null) 
        {
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
