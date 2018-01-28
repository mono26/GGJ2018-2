using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    [SerializeField]
    private float force = 1f;

    private float distancia;
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        
	}

    private void OnTriggerStay2D(Collider2D collision)
    {
        Vector2 direccion = this.transform.position - collision.transform.position;
        distancia = Vector2.Distance(this.transform.position, collision.transform.position);
        float fuerza = force /(distancia*2);
        collision.gameObject.GetComponent<Rigidbody2D>().AddForce(direccion * fuerza);
        Debug.Log(distancia);
    }
}
