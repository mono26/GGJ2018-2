using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroide : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(ReleaseAsteroidCR());
	}

    public IEnumerator ReleaseAsteroidCR()
    {
        yield return new WaitForSeconds(5);
        AsteroidPool.Instance.ReleaseAsteroide(this.GetComponent<Rigidbody2D>());
    }

    // Update is called once per frame
    void Update ()
    {
		
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        
    }
}
