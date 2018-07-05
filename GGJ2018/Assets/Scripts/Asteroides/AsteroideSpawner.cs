using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroideSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

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

    [SerializeField]
    private float timer;

    private float tiempo;

    // Use this for initialization
    void Start()
    {
        tiempo = Random.Range(minSpawnTime, maxSpawnTime);
        player = GameObject.Find("BobTheGreenAlien");
        timer = tiempo;
	}
	
	// Update is called once per frame
	void Update ()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            LanzarAsteroide();
            timer = tiempo;
        }
	}

    private void LanzarAsteroide()
    {
        float force = Random.Range(minForce, maxForce);
        this.transform.LookAt(player.transform);
        Rigidbody2D asteroide = AsteroidPool.Instance.GetAsteroide();
        asteroide.transform.position = this.transform.position;
        asteroide.AddForce(transform.forward * force, ForceMode2D.Force);
      }
}
