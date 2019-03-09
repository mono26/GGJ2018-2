using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnableObject : Spawnable
{
    [Header("Spawnable settings")]
    [SerializeField] private float radius;
    [SerializeField] Collider2D collisionBody = null;
    [SerializeField] SpriteRenderer visualRenderer = null;
    public float GetRadius { get { return radius; } }

    public virtual void Awake() 
    {
        if (collisionBody == null)
        {
            collisionBody = GetComponent<Collider2D>();
        }

        if (visualRenderer == null)
        {
            visualRenderer = GetComponent<SpriteRenderer>();
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
    }
}
