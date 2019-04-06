using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : SpawnableObject 
{
	[SerializeField] Rigidbody2D body;

	public override void Release()
	{
			PoolsManager.Instance.ReleaseObjectToPool(this);
	}

	public override void ResetState()
	{
			body.velocity = Vector2.zero;
	}

	void OnTriggerEnter2D(Collider2D other) 
	{
			Release();
	}

	public void Shoot(Vector2 _direction, float _force)
	{
			body.AddForce(_direction * _force, ForceMode2D.Impulse);
	}
}
