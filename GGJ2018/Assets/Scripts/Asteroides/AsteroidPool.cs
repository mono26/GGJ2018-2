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
        Asteroid instance = Instantiate(asteroidePrefab);
        instance.gameObject.SetActive(false);
        asteroides.Add(instance);
    }

    public void ReleaseAsteroide(Asteroid asteroide)
    {
        asteroide.gameObject.SetActive(false);
        asteroides.Add(asteroide);
    }

    private Asteroid AllocateAsteroide()
    {
        Asteroid asteroide = asteroides[asteroides.Count - 1];
        asteroides.RemoveAt(asteroides.Count - 1);
        asteroide.gameObject.SetActive(true);
        return asteroide;
    }

    public Asteroid GetAsteroide()
    {
        if (asteroides.Count == 0)
            AddAsteroide();
        return AllocateAsteroide();
    }
}
