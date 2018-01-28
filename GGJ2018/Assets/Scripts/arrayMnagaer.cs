using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class arrayMnagaer : MonoBehaviour
{
    private Ship ship;
    int indexSalida = 0;
	// Use this for initialization
	void Start () {
        ship = GameObject.Find("Player").GetComponent<Ship>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8)
        {

            indexSalida = Array.IndexOf(ship.Radar.ActivePlanets, collision.gameObject);
            Debug.Log(collision.name);
            Debug.Log(indexSalida);
            ship.Radar.ActivePlanets[indexSalida] = null;
        }
    }
}
