﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PoolSettings
{	
    public Spawnable[] prefabs;
    public int size = 10;

	// TODO Crear getters.
}
public class GameObjectPool
{
	[Header("Pool settings")]
    [SerializeField] PoolSettings settings;
	[SerializeField] public List<Spawnable> pool;

	[SerializeField] Transform poolContainer = null;

	public GameObjectPool(PoolSettings _settings)
	{
		settings = _settings;

		if (poolContainer == null)
		{
			poolContainer = new GameObject(_settings.prefabs[0].name + "_Container").transform;
		}

		PreparePool();

	}
    void PreparePool()
    {
        pool = new List<Spawnable>();
        for (int i = 0; i < settings.size; i++)
		{
			AddGameObject();
		}
    }

     void AddGameObject()
    {
        int prefabIndex = Random.Range(0, settings.prefabs.Length);
        Spawnable obj = GameObject.Instantiate(settings.prefabs[prefabIndex]);
        obj.transform.SetParent(poolContainer);
        obj.gameObject.SetActive(false);
        pool.Add(obj);
    }

    Spawnable AllocateGameObject()
    {
        Spawnable obj = pool[pool.Count - 1];
        pool.RemoveAt(pool.Count - 1);
        obj.transform.SetParent(null);
        obj.gameObject.SetActive(true);
        return obj;
    }

	public void ReleaseGameObject(Spawnable _obj)
    {
        _obj.transform.SetParent(poolContainer);
        _obj.transform.position = poolContainer.position;
        _obj.gameObject.SetActive(false);
        pool.Add(_obj);
    }

    public Spawnable GetGameObject()
    {
        if (pool.Count == 0)
        {
            AddGameObject();
        }

        Spawnable objectFromPool = AllocateGameObject();
        objectFromPool.ResetState();
        return objectFromPool;
    }
}
