// Copyright (c) What a Box Creative Studio. All rights reserved.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlackHoleEventType { CenterReachead, LifeTimeEnd }

public class BlackHoleEvent : GameEvent
{
    private GameObject affectedGameObject;
    private BlackHoleEventType eventType;

    public GameObject GetAffectedObject { get { return affectedGameObject; } }
    public BlackHoleEventType GetEventType { get { return eventType; } }

    public BlackHoleEvent(GameObject _affectedGameObject, BlackHoleEventType _eventType)
    {
        affectedGameObject = _affectedGameObject;
        eventType = _eventType;
        return;
    }
}

public class BlackHole : Planet
{
    [Header("Black Hole settings")]
    [SerializeField] private float lifeTime;
    protected float minimumDistanceToCenter;
    [SerializeField] private float lifeTimeCounter;

    [Header("Balck Hole components")]
    [SerializeField] private SpawnableObject spawnableComponent;

    public SpawnableObject GetSpawnableComponent { get { return spawnableComponent; } }

    protected void OnEnable ()
    {
        lifeTimeCounter = lifeTime;
        return;
	}
	
	protected override void FixedUpdate ()
    {
        if (lifeTimeCounter <= 0)
        {
            BlackHolePool.Instance.ReleaseBlackHole(this);
            return;
        }
        ApplyGravityOnObjects();
        lifeTimeCounter -= Time.deltaTime;
        return;
    }

    protected void CheckObjectsDistanceToCenter()
    {
        foreach(Rigidbody2D body in objectsInsideGravitationField)
        {
            if(Vector3.Distance(body.position, transform.position) < minimumDistanceToCenter) {
                EventManager.TriggerEvent<BlackHoleEvent>(new BlackHoleEvent(body.gameObject, BlackHoleEventType.CenterReachead));
            }
        }
        return;
    }
}
