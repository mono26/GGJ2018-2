// Copyright (c) What a Box Creative Studio. All rights reserved.

using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Asteroid : MonoBehaviour, EventHandler<BlackholeEvent>, IAffectedByGravity
{
    [Header("Asteroid settings")]
    [SerializeField] protected float lifeTime = 10;

    [Header("Asteroid components")]
    [SerializeField] protected GameObject deadParticle;
    [SerializeField] protected Rigidbody2D bodyComponent;
    [SerializeField] private SpawnableObject spawnableComponent;

    public Rigidbody2D GetBodyComponent { get { return bodyComponent; } }
    public SpawnableObject GetSpawnableComponent { get { return spawnableComponent; } }

    // Use this for initialization
    protected void Start ()
    {
        if(bodyComponent == null)
        {
            bodyComponent = GetComponent<Rigidbody2D>();
        }

        StartCoroutine(DeadTimer());
	}

    protected void OnEnable()
    {
        EventManager.AddListener<BlackholeEvent>(this);
        return;
    }

    protected void OnDisable()
    {
        EventManager.RemoveListener<BlackholeEvent>(this);
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

    protected void OnCollisionEnter2D(Collision2D collision)
    {
        ReleaseAsteroid();
    }

    protected void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.CompareTag("Shield"))
        {
            ReleaseAsteroid();
        }
    }

    private bool WeReachedABlackholeCenter(BlackholeEvent _blackholeEvent)
    {
        bool centerReached = false;
        if (_blackholeEvent.GetAffectedObject.Equals(bodyComponent) && _blackholeEvent.GetEventType == BlackholeEventType.CenterReachead) 
        {
            centerReached = true;
        }
        return centerReached;
    }

    public void ApplyGravity(Vector2 _normalizedGravityDirection, float _gravityForce, GravitySourceType _gravitySource)
    {
        // In case the user forgets to normalize the direction vector.
        Vector3 normalizedDirection = _normalizedGravityDirection.normalized;
        bodyComponent.AddForce(_normalizedGravityDirection * _gravityForce, ForceMode2D.Force);
    }

    public void ApplyRotationSpeed(Vector2 _normalizedRotationDirection, float _rotationForce)
    {
        // In case the user forgets to normalize the direction vector.
        Vector3 normalizedDirection = _normalizedRotationDirection.normalized;
        bodyComponent.AddForce(_normalizedRotationDirection * _rotationForce, ForceMode2D.Force);
    }

    public void RotateTowardsGravitationCenter(Vector2 _gravitationCenterDirection)
    {
        return;
    }

    public void OnGameEvent(BlackholeEvent _blackHoleEvent)
    {
        if (WeReachedABlackholeCenter(_blackHoleEvent)) 
        {
            ReleaseAsteroid();
        }
    }
}
