using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetWeapon : Spawnable 
{
	[Header("Settings")]
	[SerializeField] Transform barrelPivot = null;
	[SerializeField] Transform shootPoint = null;
	[SerializeField] float maxRoation = 45.0f;
	[SerializeField][Range(0, 1)] float rotationSpeed = 0.5f;

	[Header("Shooting")]
	[SerializeField] float cooldown = 1.0f;
	[SerializeField] float shootForce = 1000.0f;
	[SerializeField] Bullet bullet = null;

	Transform target = null;
	float lastShoot = 0.0f;
	
	// Update is called once per frame
	void Update () 
	{
		if (target == null)
		{
			return;
		}

		Vector3 targetDirection;
		if (IsInsideShootAngle())
		{
			Vector3 directionToTarget = target.position - transform.position;
			targetDirection = directionToTarget.normalized;
		}
		else
		{
			Vector3 localTargetPosition = transform.InverseTransformPoint(target.position);
			if (localTargetPosition.x < 0)
			{
				targetDirection = Quaternion.AngleAxis(-maxRoation, -transform.forward) * transform.up;
			}
			else
			{
				targetDirection = Quaternion.AngleAxis(maxRoation, -transform.forward) * transform.up;
			}
		}

		targetDirection.z = 0;
		Vector3 nextDirection = Vector3.Lerp(barrelPivot.up, targetDirection, rotationSpeed);
		barrelPivot.up = nextDirection;

		if (lastShoot + cooldown < Time.timeSinceLevelLoad)
		{
			Shoot();
		}
	}

	bool IsInsideShootAngle()
	{
		bool insideAngle = true;
		Vector3 directionToTarget = target.position - transform.position;
		float angle = Vector3.Angle(directionToTarget, transform.up);

		if (angle < -maxRoation || angle > maxRoation)
		{
			insideAngle = false;
		}

		return insideAngle;
	}

	void Shoot()
	{
		Bullet bulletToShoot = PoolsManager.Instance.GetObjectFromPool<Bullet>();
		bulletToShoot.transform.position = shootPoint.position;
		bulletToShoot.transform.rotation = shootPoint.rotation;
		bulletToShoot.Shoot(barrelPivot.up, shootForce);

		lastShoot = Time.timeSinceLevelLoad;
	}

	// void OnTriggerEnter2D(Collider2D _collider) 
	// {
	// 	if (!_collider.CompareTag("Player"))
	// 	{
	// 		return;
	// 	}

	// 	target = _collider.transform;
	// }

	// void OnTriggerExit2D(Collider2D _collider) 
	// {
	// 	if (!_collider.CompareTag("Player"))
	// 	{
	// 		return;
	// 	}

	// 	target = null;
	// }

	public void SetTarget(Transform _target)
	{
		target = _target;
	}
}
