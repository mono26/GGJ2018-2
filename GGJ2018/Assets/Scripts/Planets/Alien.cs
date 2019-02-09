// Copyright (c) What a Box Creative Studio. All rights reserved.

using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Alien : MonoBehaviour, EventHandler<HealthEvent>, IAffectedByGravity
{
    [Header("Alien settings")]
    [SerializeField] float lifeTime = 3.0f;
    [SerializeField] float groundCheckDistance = 0.5f;

    [SerializeField] LayerMask planetsLayer;

    [Header("Alien components")]
    [SerializeField] protected Health healthComponent;
    [SerializeField] Rigidbody2D bodyComponent;

    Coroutine deathRoutine;

    public Rigidbody2D GetBodyComponent { get { return bodyComponent; } }

    private void Awake() 
    {
        if(healthComponent == null)
        {
            healthComponent = GetComponent<Health>();
        }
        if(bodyComponent == null)
        {
            bodyComponent = GetComponent<Rigidbody2D>();
        }

        return;
    }

    void OnDrawGizmos() 
    {
        Gizmos.DrawRay(transform.position, -transform.up * groundCheckDistance);
    }

    protected void OnEnable()
    {
        EventManager.AddListener<HealthEvent>(this);
        return;
    }

    protected void OnDisable()
    {
        EventManager.RemoveListener<HealthEvent>(this);
        return;
    }

    private void OnTriggerEnter2D(Collider2D _collider)
    {
        if (!_collider.CompareTag("GravitationField")) 
        {
            return;
        }

        var parentPlanet = _collider.transform.parent;
        if(parentPlanet.CompareTag("Planet"))
        {
            if(deathRoutine == null)
            {
                return;
            }

            StopCoroutine(deathRoutine);
        }
    }

    private void OnTriggerExit2D(Collider2D _collision)
    {
        if (_collision.gameObject.CompareTag("GravitationField")) 
        {
            deathRoutine = StartCoroutine(KillAlienAfterLifeTime());
        }
    }

    private IEnumerator KillAlienAfterLifeTime()
    {
        yield return new WaitForSeconds(lifeTime);

        healthComponent.TakeDamage(999);
        
        yield break;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            LevelManager.Instance.IncreaseScore();
        }
        return;
    }

    private bool AreWeDead(HealthEvent _healthEvent)
    {
        bool weAreDead = false;
        if(_healthEvent.GetGameObjectWithHealth == gameObject && _healthEvent.GetEventType == HealthEventType.HealthDepleted){
            weAreDead = true;
        }
        return weAreDead;
    }

    public void ApplyGravity(Vector2 _normalizedGravityDirection, float _gravityForce)
    {
        if(IsOnGround())
        {
            return;
        }

        // In case the user forgets to normalize the direction vector.
        Vector3 normalizedDirection = _normalizedGravityDirection.normalized;
        bodyComponent.AddForce(_normalizedGravityDirection * _gravityForce, ForceMode2D.Force);
    }

    public void ApplyRotationSpeed(Vector2 _normalizedRotationDirection, float _rotationSpeed)
    {
        // In case the user forgets to normalize the direction vector.
        Vector3 normalizedDirection = _normalizedRotationDirection.normalized;
        bodyComponent.AddForce(_normalizedRotationDirection * _rotationSpeed, ForceMode2D.Force);
    }

    bool IsOnGround()
    {
        bool touchingGround = false;
        var groundHit = Physics2D.Raycast(transform.position, -transform.up, groundCheckDistance, planetsLayer);
        if(groundHit.collider != null) 
        {
            if(groundHit.collider.CompareTag("Planet"))
            {
                touchingGround = true;
            }
        }
        return touchingGround;
    }

    public void RotateTowardsGravitationCenter(Vector2 _gravitationCenterDirection)
    {
        transform.up = _gravitationCenterDirection;
    }

    public void OnGameEvent(HealthEvent _healthEvent)
    {
        if(AreWeDead(_healthEvent)) 
        {
            Destroy(gameObject);
        }
        return;
    }
}
