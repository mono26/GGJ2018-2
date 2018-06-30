using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsteroideSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    public Rigidbody2D asteroide;

    public float force;
    public float offsetX;
    public float offsetY;

    [SerializeField]
    private float timer;

     private float tiempo;

    // Use this for initialization
    void Start()
    {
        tiempo = Random.Range(3f, 10f);
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
        force = Random.Range(150f, 350f);
        this.transform.LookAt(player.transform);
        asteroide = AsteroidPool.Instance.GetAsteroide();
        asteroide.transform.position = this.transform.position;
        asteroide.AddForce(transform.forward * force);
      }
}
