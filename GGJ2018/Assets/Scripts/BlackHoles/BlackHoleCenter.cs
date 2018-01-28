using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleCenter : MonoBehaviour
{
    private GameObject Blackhole;
    // Use this for initialization
    void Start()
    {
        Blackhole = GetComponentInParent<BlackHole>().gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var vel = collision.GetComponent<Rigidbody2D>().velocity;
            collision.GetComponent<Rigidbody2D>().velocity = vel * 0.2f;
            StartCoroutine(ImplotarCR());
        }
    }

    private IEnumerator ImplotarCR()
    {
        Debug.Log("Empece Corrutina");
        yield return new WaitForSeconds(5);
        Destroy(Blackhole);
    }
}
