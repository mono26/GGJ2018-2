using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gravity : MonoBehaviour, IAffectedByGravity
{
	[Header("Settings")]
	[SerializeField] float groundCheckDistance = 0.5f;
    [SerializeField] LayerMask planetsLayer;

	[Header("Dependencies")]
	[SerializeField] Rigidbody2D bodyComponent;
    public Rigidbody2D GetBodyComponent { get { return bodyComponent; } }

	public virtual void ApplyGravity(Planet _planet)
    {
        if(IsOnGround())
        {
            return;
        }

        Vector2 directionToCenter = (_planet.GetCenterPosition - (Vector2)transform.position).normalized;
        float gravityForceToApply = _planet.GetGravityForce * GetBodyComponent.mass;

        // In case the user forgets to normalize the direction vector.
        bodyComponent.AddForce(directionToCenter * gravityForceToApply * Time.fixedDeltaTime, ForceMode2D.Force);
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

    public void ApplyRotation(Planet _planet)
    {
        Vector2 vectorFromCenter = ((Vector2)transform.position - _planet.GetCenterPosition).normalized;
        Vector2 directionToCenterTanget = new Vector2(-vectorFromCenter.y, vectorFromCenter.x).normalized * _planet.GetGravFieldDirection;
        // Debug.DrawRay(transform.position, directionToCenterTanget, Color.green, 3);
        float rotationForceToApply = _planet.GetRotationForce * GetBodyComponent.mass;

        // In case the user forgets to normalize the direction vector.
        bodyComponent.AddForce(directionToCenterTanget * rotationForceToApply * Time.fixedDeltaTime, ForceMode2D.Force);
    }

	public void RotateTowardsGravitationCenter(Planet _planet)
    {
        // Debug.DrawLine(transform.position, _planet.transform.position, Color.yellow, 3);
        Vector2 vectorFromCenter = ((Vector2)transform.position - _planet.GetCenterPosition).normalized;
        // Debug.DrawRay(transform.position, directionToCenter, Color.green, 3);
        Vector2 targetRotation = Vector2.Lerp((Vector2)transform.up, vectorFromCenter, 0.05f);

        transform.up = targetRotation;
    }
}
