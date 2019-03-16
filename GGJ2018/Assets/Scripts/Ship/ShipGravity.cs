using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipGravity : Gravity
{
    public override void ApplyGravity(Vector2 _normalizedGravityDirection, float _gravityForce, GravitySourceType _gravitySource)
    {
        if(_gravitySource.Equals(GravitySourceType.Planet))
        {
            return;
        }

        // In case the user forgets to normalize the direction vector.
        Vector3 normalizedDirection = _normalizedGravityDirection.normalized;
        GetBodyComponent.AddForce(_normalizedGravityDirection * _gravityForce, ForceMode2D.Force);
    }
}
