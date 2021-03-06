﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnableObject : Spawnable
{
    [Header("Spawnable settings")]
    [SerializeField] private float radius = 1.0f;
    [SerializeField] Collider2D collisionBody = null;
    public float GetRadius { get { return radius; } }
    public Collider2D GetCollisionBody { get { return collisionBody; } }

    public virtual void Awake() 
    {
        if (collisionBody == null)
        {
            collisionBody = GetComponent<Collider2D>();
        }
    }

    public void EnableCollision(bool _enable)
    {
        collisionBody.enabled = _enable;
    }

    public void DisplayVisuals(bool _display)
    {
        GetComponent<SpriteRenderer>().enabled = _display;
    }

    public void TurnCollisionOffForSeconds(float _seconds)
    {
        StartCoroutine(DisableCollisionForSeconds(_seconds));
    }

    IEnumerator DisableCollisionForSeconds(float _seconds)
    {
        EnableCollision(false);

        yield return new WaitForSeconds(_seconds);

        EnableCollision(true);
    }

    public override void ResetState()
    {
        EnableCollision(true);
        DisplayVisuals(true);

        transform.localScale = Vector3.one;
    }
}
