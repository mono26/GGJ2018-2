using System.Collections.Generic;
using UnityEngine;

public class BlackHolePool : Singleton<BlackHolePool>
{
    [Header("Black Hole Pool settings")]
    [SerializeField] private BlackHole blackholePrefab;
    [SerializeField] private int size = 10;

    [SerializeField] private List<BlackHole> blackholes;

    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();
        PrepareBlackHoles();
        return;
    }

    private void PrepareBlackHoles()
    {
        blackholes = new List<BlackHole>();
        for (int i = 0; i < size; i++)
            AddBlackHole();
    }

    private void AddBlackHole()
    {
        BlackHole instance = Instantiate(blackholePrefab);
        instance.gameObject.SetActive(false);
        blackholes.Add(instance);
    }

    public void ReleaseBlackHole(BlackHole blackhole)
    {
        blackhole.gameObject.SetActive(false);
        blackholes.Add(blackhole);
    }

    public BlackHole GetBlackHole()
    {
        if (blackholes.Count == 0)
            AddBlackHole();
        return AllocateBlackHole();
    }

    private BlackHole AllocateBlackHole()
    {
        BlackHole blackhole = blackholes[blackholes.Count - 1];
        blackholes.RemoveAt(blackholes.Count - 1);
        blackhole.gameObject.SetActive(true);
        return blackhole;
    }
}
