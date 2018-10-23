// Copyright (c) What a Box Creative Studio. All rights reserved.

using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Alien : MonoBehaviour, EventHandler<HealthEvent>
{
    [Header("Alien settings")]
    [SerializeField] protected float lifeTime = 3.0f;

    [Header("Alien components")]
    [SerializeField] protected Health healthComponent;

    private void Awake() 
    {
        healthComponent = GetComponent<Health>();
        return;
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
        if (_collider.CompareTag("GravitationField")) {
            StopCoroutine(KillAlienAfterLifeTime());
        }
        return;
    }

    private IEnumerator KillAlienAfterLifeTime()
    {
        yield return new WaitForSeconds(lifeTime);
        healthComponent.TakeDamage(999);
        DestroyObject(gameObject);
        yield break;
    }

    private void OnTriggerExit2D(Collider2D _collision)
    {
        if (_collision.gameObject.CompareTag("GravitationField")) {
            StartCoroutine(KillAlienAfterLifeTime());
        }
        return;
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

    public void OnGameEvent(HealthEvent _healthEvent)
    {
        if(AreWeDead(_healthEvent)) {
            Destroy(gameObject);
        }
        return;
    }
}
