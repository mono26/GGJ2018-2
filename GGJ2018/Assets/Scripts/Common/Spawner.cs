using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner : MonoBehaviour
{
    [SerializeField] protected float spawnTimer;
    [SerializeField] int minTimeToSpawn = 0;
    [SerializeField] int maxTimeToSpawn = 10;
    [SerializeField] protected int maxTriesToSpawn = 10;
    [SerializeField] LayerMask layersToCheck;

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
        bool isAFreeSpot = true;
        if (Physics2D.OverlapCircle(_positionToCheck, _objectSize, layersToCheck) != null)
        {
            isAFreeSpot = false;
        }

        return isAFreeSpot;
    }
    public virtual void Spawn()
    {
        spawnTimer = Random.Range(minTimeToSpawn, maxTimeToSpawn);
    }
}
