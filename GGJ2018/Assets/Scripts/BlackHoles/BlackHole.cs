// Copyright (c) What a Box Creative Studio. All rights reserved.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlackholeEventType { CenterReachead, LifeTimeEnd }

public class BlackholeEvent : GameEvent
{
    private GameObject affectedGameObject;
    private BlackholeEventType eventType;

    public GameObject GetAffectedObject { get { return affectedGameObject; } }
    public BlackholeEventType GetEventType { get { return eventType; } }

    public BlackholeEvent(GameObject _affectedGameObject, BlackholeEventType _eventType)
    {
        affectedGameObject = _affectedGameObject;
        eventType = _eventType;
        return;
    }
}

public class Blackhole : Planet
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
        if (lifeTimeCounter > 0)
        {
            lifeTimeCounter -= Time.deltaTime;
        }
        else
        {
            BlackholePool.Instance.ReleaseBlackhole(this);
        }

        base.FixedUpdate();
    }

    protected void CheckObjectsDistanceToCenter()
    {
        for (int i =  objsInGravitationField.Count - 1; i > -1; i--)
        {
            if(objsInGravitationField[i] == null)
            {
                objsInGravitationField.RemoveAt(i);
                continue;
            }

            if(Vector3.Distance(objsInGravitationField[i].GetBodyComponent.position, transform.position) < minimumDistanceToCenter) {
                EventManager.TriggerEvent<BlackholeEvent>(new BlackholeEvent(objsInGravitationField[i].GetBodyComponent.gameObject, BlackholeEventType.CenterReachead));
            }
        }
    }
}
