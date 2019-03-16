using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour, IAffectedByGravity 
{
	[Header("Settings")]
	[SerializeField] float groundCheckDistance = 0.5f;
    [SerializeField] LayerMask planetsLayer = 1 << LayerMask.NameToLayer("Planet");

	[Header("Dependencies")]
	[SerializeField] Rigidbody2D bodyComponent;
    public Rigidbody2D GetBodyComponent { get { return bodyComponent; } }

	public virtual void ApplyGravity(Vector2 _normalizedGravityDirection, float _gravityForce, GravitySourceType _gravitySource)
    {
        if(IsOnGround())
        {
            return;
        }

        // In case the user forgets to normalize the direction vector.
        Vector3 normalizedDirection = _normalizedGravityDirection.normalized;
        bodyComponent.AddForce(_normalizedGravityDirection * _gravityForce, ForceMode2D.Force);
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

    public void ApplyRotation(Vector2 _normalizedRotationDirection, float _rotationSpeed)
    {
        // In case the user forgets to normalize the direction vector.
        Vector3 normalizedDirection = _normalizedRotationDirection.normalized;
        bodyComponent.AddForce(_normalizedRotationDirection * _rotationSpeed, ForceMode2D.Force);
    }

	public void RotateTowardsGravitationCenter(Vector2 _gravitationCenterDirection)
    {
        transform.up = _gravitationCenterDirection;
    }
}
