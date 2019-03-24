// Copyright (c) What a Box Creative Studio. All rights reserved.
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageOnTouch : MonoBehaviour
{
	[Header("Damage On Touch settings")]
    [SerializeField] private string[] damageTags;
    [SerializeField] private float damageOnTouch;
    [SerializeField] private float sendDamageTimer;

    Coroutine sendDamageRoutine;

    protected virtual void CollidingWithDamageable(Damageable _damageable)
    {
        _damageable.TakeDamage(damageOnTouch);
        return;
    }

    protected virtual void OnCollisionEnter(Collision _collision)
    {
        foreach(string tag in damageTags)
        {
            if(_collision.gameObject.CompareTag(tag))
            {
                Damageable damageableComponent = GetDamageableComponentFrom(_collision.gameObject);
                if (damageableComponent != null) {
                    CollidingWithDamageable(damageableComponent);
                }
            }
        }
        return;
    }

	private Damageable GetDamageableComponentFrom(GameObject _targetGameObject)
	{
		Damageable targetTamageable = _targetGameObject.GetComponent<Damageable>();
		return targetTamageable;
	}

    protected virtual void OnTriggerEnter2D(Collider2D _collider)
    {
        foreach (string tag in damageTags)
        {
            if (_collider.gameObject.CompareTag(tag))
            {
                Damageable damageableComponent = GetDamageableComponentFrom(_collider.gameObject);
                if (damageableComponent != null) {
                    sendDamageRoutine = StartCoroutine(SendDamage(damageableComponent));

                }
            }
        }
        return;
    }

    protected virtual void OnTriggerExit2D(Collider2D _collider)
    {
        foreach (string tag in damageTags)
        {
            if (_collider.gameObject.CompareTag(tag))
            {
                Damageable damageableComponent = GetDamageableComponentFrom(_collider.gameObject);
                if (damageableComponent != null)
                {
                    StopCoroutine(sendDamageRoutine);

                }
            }
        }
        return;
    }

    private void OnCollisionEnter2D(Collision2D _collider) 
    {
        foreach (string tag in damageTags)
        {
            if (_collider.gameObject.CompareTag(tag))
            {
                Damageable damageableComponent = GetDamageableComponentFrom(_collider.gameObject);
                if (damageableComponent != null) {
                    CollidingWithDamageable(damageableComponent);
                }
            }
        }
        return;
    }

    private IEnumerator SendDamage(Damageable damageableComponent)
    {
        CollidingWithDamageable(damageableComponent);
        yield return new WaitForSeconds(1);
        sendDamageRoutine = StartCoroutine(SendDamage(damageableComponent));
    }

	public void SetDamageOnTouch(float _newDamage)
    {
        damageOnTouch = _newDamage;
        return;
    }
}
