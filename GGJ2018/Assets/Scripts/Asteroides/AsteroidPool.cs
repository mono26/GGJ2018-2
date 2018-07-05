using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidPool : Singleton<AsteroidPool>
{
    [Header("Asteroid Pool settings")]
    [SerializeField]
    private Rigidbody2D asteroidePrefab;

    private List<Rigidbody2D> asteroides;

    private void PrepareAsteroide()
    {
        asteroides = new List<Rigidbody2D>();
        for (int i = 0; i > asteroides.Capacity; i++)
            AddAsteroide();
    }

    private void AddAsteroide()
    {
        Rigidbody2D instance = Instantiate(asteroidePrefab);
        instance.gameObject.SetActive(false);
        asteroides.Add(instance);
    }

    public void ReleaseAsteroide(Rigidbody2D asteroide)
    {
        asteroide.gameObject.SetActive(false);
        asteroides.Add(asteroide);
    }

    public Rigidbody2D GetAsteroide()
    {
        if (asteroides.Count == 0)
            AddAsteroide();
        return AllocateAsteroide();
    }

    private Rigidbody2D AllocateAsteroide()
    {
        Rigidbody2D asteroide = asteroides[asteroides.Count - 1];
        asteroides.RemoveAt(asteroides.Count - 1);
        asteroide.gameObject.SetActive(true);
        return asteroide;
    }
}
