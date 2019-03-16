using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAffectedByGravity 
{
    /// <summary>
    /// Get the required RigidBody2D component.
    /// </summary>
    Rigidbody2D GetBodyComponent { get; }

	/// <summary>
    /// Applies gravity to the rigidbody component.
    /// </summary>
    /// <param name="_direction"> The normalized direction of the gravity.</param>
    /// <param name="_force"> The amount of force to apply. Not affected by DeltaTime or FixedDeltaTime.</param>
	void ApplyGravity(Vector2 _direction, float _force, GravitySourceType _gravitySource);

    /// <summary>
    /// Apply a rotational force when the object enter a gravitational field. Normally the direction is tangential to the objects direction to the ceneter.
    /// </summary>
    /// <param name="_direction"> The normalized direction of the gravity.</param>
    /// <param name="_force"> The amount of force to apply. Not affected by DeltaTime or FixedDeltaTime.</param>
    void ApplyRotation(Vector2 _direction, float _force);

    /// <summary>
    /// Rotate the objects so it is aligned towards the center of the gravitational field. Using the transform up vector.
    /// </summary>
    /// <param name="_gravitationCenterDirection"> Direction towards the center. Usually the direction from the object towards the center.</param>
    void RotateTowardsGravitationCenter(Vector2 _gravitationCenterDirection);
}
