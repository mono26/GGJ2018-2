using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroideSpawner : MonoBehaviour
{
    [Header("Asteroid settings")]
    [SerializeField]
    protected float maxForce = 3500f;
    [SerializeField]
    protected float minForce = 1500f;
    [SerializeField]
    protected float maxSpawnTime = 3f;
    [SerializeField]
    protected float minSpawnTime = 5f;
    [SerializeField]
    protected float offsetX;
    [SerializeField]
    protected float offsetY;

    [Header("Components")]
    [SerializeField]
    protected GameObject player;
    [SerializeField]
    protected Transform[] spawnPoints;
    [SerializeField]
    protected float timer;

    protected void Awake()
    {
        player = GameObject.Find("BobTheGreenAlien");
        GameObject[] sPoints = GameObject.FindGameObjectsWithTag("AsteroidSpawnPoint");
        spawnPoints = new Transform[sPoints.Length];
        for (int i = 0; i < sPoints.Length; i++)
        {
            spawnPoints[i] = sPoints[i].GetComponent<Transform>();
        }

        return;
    }
    void Start()
    {
        timer = Random.Range(minSpawnTime, maxSpawnTime);

        return;
    }
	
	// Update is called once per frame
	void Update ()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            LanzarAsteroide();
            timer = Random.Range(minSpawnTime, maxSpawnTime);
        }

        return;
	}

    private void LanzarAsteroide()
    {
        float force = Random.Range(minForce, maxForce);
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Vector2 direction = (player.transform.position - spawnPoint.position).normalized;
        Rigidbody2D asteroide = AsteroidPool.Instance.GetAsteroide();
        asteroide.transform.position = spawnPoint.position;
        asteroide.AddForce(direction * force, ForceMode2D.Force);

        return;
      }
}
