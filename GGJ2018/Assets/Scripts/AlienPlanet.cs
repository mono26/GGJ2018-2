using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienPlanet : Planet
{
    [SerializeField]
    private Transform[] aliens = null;

    // Use this for initialization
    public override void Start()
    {
        base.Start();
        foreach (Transform alien in aliens)
        {
            if (alien != null)
            {
                LocateAliens(alien);
                ChangeUpDirectionTowardsPlanet(alien);
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
                ChangeUpDirectionTowardsPlanet(alien);
            }
        }
    }

    private void LocateAliens(Transform _alien)
    {
        _alien.position = GetComponent<CircleCollider2D>().bounds.min;
    }

    private void ChangeUpDirectionTowardsPlanet(Transform _alien)
    {
        _alien.up = (_alien.position - transform.position).normalized;
    }
}
