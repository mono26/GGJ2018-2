using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : SpawnableObject 
{
	[SerializeField] float damage = 5.0f;
	[SerializeField] Rigidbody2D body = null;

	public override void Release()
	{
		PoolsManager.Instance.ReleaseObjectToPool(this);
	}

	public override void ResetState()
	{
		EnableCollision(true);
		DisplayVisuals(true);

		body.velocity = Vector2.zero;
	}

	void OnTriggerEnter2D(Collider2D _collider) 
	{
		if (!_collider.CompareTag("Player"))
        {
			ShipEngine engine = _collider.GetComponent<ShipEngine>();
			if (engine != null)
			{
        		engine.TakeDamage(damage);
			}
        }

		Release();
	}

	public void Shoot(Vector2 _direction, float _force)
	{
		body.AddForce(_direction * _force, ForceMode2D.Impulse);
	}
}
