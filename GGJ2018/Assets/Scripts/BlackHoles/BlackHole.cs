using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    [SerializeField]
    private float force = 1f;
    private float distancia;

    [SerializeField]
    private float tiempoRestante;

    private bool isInactive;

	// Use this for initialization
	void Start ()
    {
        tiempoRestante = 10;
        isInactive = true;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (isInactive)
        {
            tiempoRestante -= Time.deltaTime;
        }

        if (tiempoRestante <= 0)
        {
            Destroy(this.gameObject);
            BlackholePool.Instance.ReleaseBlackholes(this.GetComponent<Rigidbody2D>());
        }
	}

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isInactive = false;
            Vector2 direccion = this.transform.position - collision.transform.position;
            distancia = Vector2.Distance(this.transform.position, collision.transform.position);
            float fuerza = force;
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(direccion * fuerza);
        }
        
    }
}
