using System.Collections.Generic;
using UnityEngine;

public class BlackholePool : Singleton<BlackholePool>
{
    [Header("Black Hole Pool settings")]
    [SerializeField] private Blackhole blackholePrefab;

    [SerializeField] private List<Blackhole> blackholes;

    [SerializeField] private int size = 10;

    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();
        PrepareBlackholes();
        return;
    }

    private void PrepareBlackholes()
    {
        blackholes = new List<Blackhole>();
        for (int i = 0; i < size; i++)
            AddBlackhole();
    }

    private void AddBlackhole()
    {
        Blackhole blackhole = Instantiate(blackholePrefab);
        blackhole.transform.SetParent(transform);
        blackhole.gameObject.SetActive(false);
        blackholes.Add(blackhole);
    }

    public void ReleaseBlackhole(Blackhole blackhole)
    {
        blackhole.transform.SetParent(transform);
        blackhole.transform.position = transform.position;
        blackhole.gameObject.SetActive(false);
        blackholes.Add(blackhole);
    }

    public Blackhole GetBlackhole()
    {
        if (blackholes.Count == 0)
            AddBlackhole();
        return AllocateBlackhole();
    }

    private Blackhole AllocateBlackhole()
    {
        Blackhole blackhole = blackholes[blackholes.Count - 1];
        blackholes.RemoveAt(blackholes.Count - 1);
        blackhole.transform.SetParent(null);
        blackhole.gameObject.SetActive(true);
        return blackhole;
    }
}
