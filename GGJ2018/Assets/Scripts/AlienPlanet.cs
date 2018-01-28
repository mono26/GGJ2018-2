using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienPlanet : Planet
{
    [SerializeField]
    private Transform[] aliens;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        foreach (Transform alien in aliens)
        {
            if (alien != null)
            {
                alien.RotateAround(transform.position, transform.forward, GravitationalFieldStrenght * Time.deltaTime);
                alien.up = (alien.position - transform.position).normalized;
            }
        }
        // Make all the aliens rotate 
    }

    // Update is called once per frame
    void Update ()
    {
        foreach (Transform alien in aliens)
        {
            if (alien != null)
            {
                alien.RotateAround(transform.position, transform.forward, GravitationalFieldStrenght * Time.deltaTime);
                alien.up = (alien.position - transform.position).normalized;
            }
        }
    }
}
