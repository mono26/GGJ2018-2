// Copyright (c) What a Box Creative Studio. All rights reserved.

using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Asteroid : SpawnableObject, EventHandler<BlackholeEvent>, IAffectedByGravity
{
    [Header("Asteroid components")]
    [SerializeField] protected Rigidbody2D bodyComponent = null;
    [SerializeField] AutoDestroy destructionComponent = null;
    [SerializeField] AudioClip crashSound = null;
    [SerializeField] AudioClip shieldCrashSound = null;

    public Rigidbody2D GetBodyComponent { get { return bodyComponent; } }

    // Use this for initialization
    public override void Awake ()
    {
        base.Awake();

        if(bodyComponent == null)
        {
            bodyComponent = GetComponent<Rigidbody2D>();
        }

        if (destructionComponent == null)
        {
            destructionComponent = GetComponent<AutoDestroy>();
        }
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

    protected void OnCollisionEnter2D(Collision2D _other)
    {
        if (_other.gameObject.CompareTag("Alien"))
        {
            return;
        }

        SoundManager.Instance.PlaySoundInPosition(transform.position, crashSound, 0.1f);

        destructionComponent.AutoDestroyObject();
    }

    protected void OnTriggerEnter2D(Collider2D _other)
    {
        if (_other.CompareTag("Shield"))
        {
            destructionComponent.AutoDestroyObject();
            SoundManager.Instance.PlaySoundInPosition(transform.position, shieldCrashSound, 0.1f);
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
            destructionComponent.AutoDestroyObject();
        }
    }

    public override void Release()
    {
        PoolsManager.Instance.ReleaseObjectToPool<Asteroid>(this);
    }
}
