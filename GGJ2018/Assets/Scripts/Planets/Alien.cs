﻿using System.Collections;
using UnityEngine;

public class Alien : MonoBehaviour
{
    [Header("Alien settings")]
    [SerializeField]
    protected float lifeTime = 5.0f;
    [SerializeField]
    protected Planet planet;

    protected void Awake()
    {
        if (planet == null)
            planet.GetComponentInParent<Planet>();

        return;
    }

    public IEnumerator KillAlien()
    {
        yield return new WaitForSeconds(lifeTime);

        DestroyObject(gameObject);

        yield break;
    }

    protected void OnTriggerEnter2D(Collider2D _collider)
    {
        if (_collider.CompareTag("GravitationField"))
        {
            StopCoroutine(KillAlien());
        }

        return;
    }

    public void OnTriggerExit2D(Collider2D _collision)
    {
        if (_collision.gameObject.CompareTag("GravitationField"))
        {
            StartCoroutine(KillAlien());
        }

        return;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Destroy(collision.gameObject);
            GameController.Instance.IncreaseScore();

            //TODO increase fuel.
        }

        return;
    }
}
