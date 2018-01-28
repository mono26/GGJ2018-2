//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleSpawn : MonoBehaviour
{
    [SerializeField]
    private float spawnRate;

    private Rigidbody2D blackhole;
    public GameObject blackHolePref;
    private float offsetX;
    private float offsetY;

    public float maxOffsetX;
    public float maxOffsetY;

    // Use this for initialization
    void Start ()
    {
        spawnRate = 10;
	}
	
	// Update is called once per frame
	void Update ()
    {
        spawnRate -= Time.deltaTime;
        if (spawnRate <= 0)
        {
            spawnRate = 10;
            spawnBlackHole();
        }
	}

    private void spawnBlackHole()
    {
        offsetX = Random.Range(-maxOffsetX, maxOffsetX);
        offsetY = Random.Range(-maxOffsetY, maxOffsetY);
        var positionX = this.transform.position.x + offsetX;
        var positionY = this.transform.position.y + offsetY;
        GameObject blackholeGO = Instantiate(blackHolePref);
        //blackhole = BlackholePool.Instance.GetBlackHole();
        blackholeGO.transform.position = new Vector2(positionX, positionY);
    }
}
