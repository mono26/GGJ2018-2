// Copyright (c) What a Box Creative Studio. All rights reserved.

using UnityEngine;

public class DamageOnTouch : MonoBehaviour
{
	[Header("Damage On Touch settings")]
    [SerializeField] private string[] damageTags;
    [SerializeField] private float damageOnTouch;

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
                    CollidingWithDamageable(damageableComponent);
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

	public void SetDamageOnTouch(float _newDamage)
    {
        damageOnTouch = _newDamage;
        return;
    }
}
