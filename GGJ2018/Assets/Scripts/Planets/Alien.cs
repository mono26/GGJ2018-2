﻿// Copyright (c) What a Box Creative Studio. All rights reserved.

using System.Collections;
using UnityEngine;

// TODO Remove IAffectedByGravity
[RequireComponent(typeof(Rigidbody2D))]
public class Alien : SpawnableObject
{
    [Header("Alien settings")]
    [SerializeField] float lifeTime = 3.0f;
    [SerializeField] float groundCheckDistance = 0.5f;

    [SerializeField] LayerMask planetsLayer;

    [Header("Alien components")]
    [SerializeField] AutoDestroy destroyComponent;
    [SerializeField] Rigidbody2D bodyComponent;

    Coroutine deathRoutine;

    public Rigidbody2D GetBodyComponent { get { return bodyComponent; } }

    public AutoDestroy GetAutoDestroyComponent { get { return destroyComponent; } }

    public override void Awake()
    {
        base.Awake();

        if(destroyComponent == null)
        {
            destroyComponent = GetComponent<AutoDestroy>();
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

    private void OnTriggerEnter2D(Collider2D _collider)
    {
        if (!_collider.CompareTag("GravitationField")) 
        {
            return;
        }

        var parentPlanet = _collider.transform.parent;
        if (!parentPlanet.CompareTag("Planet"))
        {
            return;
        }

        if(deathRoutine == null)
        {
            return;
        }
            
        StopCoroutine(deathRoutine);
    }

    private void OnTriggerExit2D(Collider2D _collider)
    {
        if (!_collider.gameObject.CompareTag("GravitationField")) 
        {
            return;
        }

        var parentPlanet = _collider.transform.parent;
        if (!parentPlanet.CompareTag("Planet"))
        {
            return;
        }

        if (gameObject.activeInHierarchy == false)
        {
            return;
        }

        deathRoutine = StartCoroutine(KillAlienAfterLifeTime());
    }

    private IEnumerator KillAlienAfterLifeTime()
    {
        yield return new WaitForSeconds(lifeTime);

        Release();
        
        yield break;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // El component de AutoDestruccion se encargar de Spawnear la particula y de hacer relese al Pool
            // destroyComponent.AutoDestroyObject();
            // Si no se quiere que el component de AutoDestruccion maneje el Release al pool. NOTA: No se reproduce ninguna particula de esta manera.
            Ship ship = collision.gameObject.GetComponent<Ship>();
            if (ship.GetRayComponent.IsAlienRayOn)
            {
                LevelManager.Instance.IncreaseScore();
                Release();
            }
            else
            {
                TakeDamage();
            }
        }

        else if (collision.gameObject.CompareTag("Asteroid"))
        {
            TakeDamage();
        }
    }

    public void TakeDamage()
    {
        destroyComponent.AutoDestroyObject();
    }

    // public void ApplyGravity(Vector2 _normalizedGravityDirection, float _gravityForce, GravitySourceType _gravitySource)
    // {
    //     if(IsOnGround())
    //     {
    //         return;
    //     }

    //     // In case the user forgets to normalize the direction vector.
    //     Vector3 normalizedDirection = _normalizedGravityDirection.normalized;
    //     bodyComponent.AddForce(_normalizedGravityDirection * _gravityForce, ForceMode2D.Force);
    // }

    // public void ApplyRotation(Vector2 _normalizedRotationDirection, float _rotationSpeed)
    // {
    //     // In case the user forgets to normalize the direction vector.
    //     Vector3 normalizedDirection = _normalizedRotationDirection.normalized;
    //     bodyComponent.AddForce(_normalizedRotationDirection * _rotationSpeed, ForceMode2D.Force);
    // }

    // bool IsOnGround()
    // {
    //     bool touchingGround = false;
    //     var groundHit = Physics2D.Raycast(transform.position, -transform.up, groundCheckDistance, planetsLayer);
    //     if(groundHit.collider != null) 
    //     {
    //         if(groundHit.collider.CompareTag("Planet"))
    //         {
    //             touchingGround = true;
    //         }
    //     }
    //     return touchingGround;
    // }

    // public void RotateTowardsGravitationCenter(Vector2 _gravitationCenterDirection)
    // {
    //     transform.up = _gravitationCenterDirection;
    // }

    public override void Release()
    {
        PoolsManager.Instance.ReleaseObjectToPool(this);
    }
}
