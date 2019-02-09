using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidPool : Singleton<AsteroidPool>
{
    [Header("Asteroid Pool settings")]
    [SerializeField] private Asteroid asteroidePrefab;

    [SerializeField] private List<Asteroid> asteroides;

    [SerializeField] private int size = 10;

    protected override void Awake()
    {
        base.Awake();
        PrepareAsteroide();
    }

    private void PrepareAsteroide()
    {
        asteroides = new List<Asteroid>();
        for (int i = 0; i < size; i++)
            AddAsteroide();
    }

    private void AddAsteroide()
    {
        Asteroid asteroid = Instantiate(asteroidePrefab);
        asteroid.transform.SetParent(transform);
        asteroid.gameObject.SetActive(false);
        asteroides.Add(asteroid);
    }

    public void ReleaseAsteroide(Asteroid asteroide)
    {
        asteroide.transform.SetParent(transform);
        asteroide.transform.position = transform.position;
        asteroide.gameObject.SetActive(false);
        asteroides.Add(asteroide);
    }

    public Asteroid GetAsteroide()
    {
        if (asteroides.Count == 0)
            AddAsteroide();
        return AllocateAsteroide();
    }

    private Asteroid AllocateAsteroide()
    {
        Asteroid asteroide = asteroides[asteroides.Count - 1];
        asteroides.RemoveAt(asteroides.Count - 1);
        asteroide.transform.SetParent(null);
        asteroide.gameObject.SetActive(true);
        return asteroide;
    }
}
