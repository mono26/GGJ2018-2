using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienPlanet : Planet
{
    public enum PlanetState
    {
        hasAliens,
        noAliens
    }

    [SerializeField]
    private PlanetState planetState;
    public PlanetState State { get { return planetState; } }

    [SerializeField]
    private int numberOfAliens;

    [SerializeField]
    private GameObject alien;

    private List<Transform> aliens = null;

    public override void Awake()
    {
        base.Start();
        aliens = new List<Transform>(numberOfAliens);
        for (int index = 0; index < numberOfAliens; index++)
        {
            var tempAlien = Instantiate(alien, transform.position, transform.rotation, transform.Find("Aliens"));
            aliens.Add(tempAlien.transform);
        }
    }
    // Use this for initialization
    public override void Start()
    {
        base.Start();
        foreach (Transform alien in aliens)
        {
            if (alien != null && alien.gameObject.activeInHierarchy)
            {
                LocateAliens(alien);
                ChangeUpDirectionTowardsPlanet(alien);
            }
        }
    }

    // Update is called once per frame
    void Update ()
    {
        if (aliens.Count > 0 && planetState == PlanetState.hasAliens)
        {
            planetState = PlanetState.noAliens;
            return;
        }
        else if (planetState != PlanetState.hasAliens)
        {
            planetState = PlanetState.hasAliens;
        }

        foreach (Transform alien in aliens)
        {
            if (alien != null && alien.gameObject.activeInHierarchy)
            {
                alien.RotateAround(transform.position, transform.forward, GravitationalFieldStrenght * Time.deltaTime);
                ChangeUpDirectionTowardsPlanet(alien);
            }
            else
            {
                aliens.Remove(alien);
                return;
            }
        }
        return;
    }

    private void LocateAliens(Transform _alien)
    {
        var angle = Random.Range(0f, 360f);
        var x = Mathf.Cos(angle) * PlanetRadius;
        var y = Mathf.Sin(angle) * PlanetRadius;
        _alien.position = transform.position + new Vector3(x, y);
    }

    private void ChangeUpDirectionTowardsPlanet(Transform _alien)
    {
        _alien.up = (_alien.position - transform.position).normalized;
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }
}
