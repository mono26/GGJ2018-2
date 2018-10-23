// Copyright (c) What a Box Creative Studio. All rights reserved.

using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Asteroid : MonoBehaviour, EventHandler<BlackHoleEvent>
{
    [SerializeField] protected GameObject deadParticle;
    [SerializeField] protected float lifeTime = 10;

    [SerializeField] protected Rigidbody2D bodyComponent;
    [SerializeField] private SpawnableObject spawnableComponent;

    public Rigidbody2D GetBodyComponent { get { return bodyComponent; } }
    public SpawnableObject GetSpawnableComponent { get { return spawnableComponent; } }

    // Use this for initialization
    protected void Start ()
    {
        if(bodyComponent == null)
            bodyComponent = GetComponent<Rigidbody2D>();

        StartCoroutine(DeadTimer());

        return;
	}

    protected void OnEnable()
    {
        EventManager.AddListener<BlackHoleEvent>(this);
        return;
    }

    protected void OnDisable()
    {
        EventManager.RemoveListener<BlackHoleEvent>(this);
        return;
    }

    protected IEnumerator DeadTimer()
    {
        yield return new WaitForSeconds(lifeTime);
        AsteroidPool.Instance.ReleaseAsteroide(this);
        yield break;
    }

    public void ReleaseAsteroid()
    {
        // TODO pass direction of movement from the asteroid to the rotation of the particle
        var particle = Instantiate(deadParticle, transform.position, transform.rotation);
        particle.transform.rotation = Quaternion.FromToRotation(particle.transform.up, bodyComponent.velocity);
        AsteroidPool.Instance.ReleaseAsteroide(this);
        return;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        ReleaseAsteroid();
        return;
    }

    public void OnGameEvent(BlackHoleEvent _blackHoleEvent)
    {
        if (_blackHoleEvent.GetAffectedObject.Equals(bodyComponent) == true) {
            ReleaseAsteroid();
        }
        return;
    }
}
