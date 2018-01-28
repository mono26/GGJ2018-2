using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroidPool : MonoBehaviour
{
    private static AsteroidPool instance;

    public static AsteroidPool Instance
    {
        get
        {
            return instance;
        }
    }

    [SerializeField]
    private Rigidbody2D asteroidePrefab;
    [SerializeField]
    private int size;

    private List<Rigidbody2D> asteroides;

    // Use this for initialization
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            PrepareAsteroide();
        }
        else
            Destroy(gameObject);
    }

    private void PrepareAsteroide()
    {
        asteroides = new List<Rigidbody2D>();
        for (int i = 0; i > size; i++)
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
