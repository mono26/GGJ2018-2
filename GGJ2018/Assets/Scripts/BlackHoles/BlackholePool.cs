using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackholePool : MonoBehaviour
{
    private static BlackholePool instance;

    public static BlackholePool Instance
    {
        get
        {
            return instance;
        }
    }

    [SerializeField]
    private Rigidbody2D blackholePrefab;
    [SerializeField]
    private int size;

    private List<Rigidbody2D> blackholes;

    // Use this for initialization
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            PrepareBlackHoles();
        }
        else
            Destroy(gameObject);
    }

    private void PrepareBlackHoles()
    {
        blackholes = new List<Rigidbody2D>();
        for (int i = 0; i > size; i++)
            AddBlackHole();
    }

    private void AddBlackHole()
    {
        Rigidbody2D instance = Instantiate(blackholePrefab);
        instance.gameObject.SetActive(false);
        blackholes.Add(instance);
    }

    public void ReleaseBlackholes(Rigidbody2D blackhole)
    {
        blackhole.gameObject.SetActive(false);
        blackholes.Add(blackhole);
    }

    public Rigidbody2D GetBlackHole()
    {
        if (blackholes.Count == 0)
            AddBlackHole();
        return AllocateBlackHole();
    }

    private Rigidbody2D AllocateBlackHole()
    {
        Rigidbody2D blackhole = blackholes[blackholes.Count - 1];
        blackholes.RemoveAt(blackholes.Count - 1);
        blackhole.gameObject.SetActive(true);
        return blackhole;
    }
}
