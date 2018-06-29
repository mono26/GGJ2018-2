﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : MonoBehaviour
{
    [Header("Radar settings")]
    [SerializeField]
    protected float range;
    [SerializeField]
    protected float rate = 1.0f;
    [SerializeField]
    protected LayerMask layerMask;
    [SerializeField]
    protected int maxRadarCapacity;

    [Header("Components")]
    [SerializeField]
    protected Ship ship;
    [SerializeField]
    protected LineRenderer signalOscillator;

    [Header("Editor debugging")]
    [SerializeField]
    protected Transform[] foundPlanets;
    public Transform[] FoundPlanets { get { return foundPlanets; } }
    [SerializeField]
    protected int lookedSigneld = 0;
    public int LookedSigneld { get { return lookedSigneld; } }
    [SerializeField]
    protected bool isRadarOn;
    public bool IsRadarOn { get { return isRadarOn; } }

    // Method for detecting planets in range
    private IEnumerator DetectPlanet()
    {
        // Check for all the colliders and store it in a variable
        Collider2D[] planets = Physics2D.OverlapCircleAll(ship.transform.position, range, layerMask);
        if (planets.Length > 0)
        {
            foreach (Collider2D planet in planets)
            {
                if (planet.gameObject.CompareTag("Score Planet") || planet.gameObject.CompareTag("Fuel Planet"))
                {
                    AddPlanetToEmptySpot(planet.transform);
                }
            }
            yield return null;
        }
        yield return new WaitForSeconds(rate);
        StartCoroutine(DetectPlanet());
    }

    public void StartRadar()
    {
        isRadarOn = true;
        StartCoroutine(DetectPlanet());
        StartCoroutine(CheckDistanceToPlanetsInRadarAndRemove());
        return;
    }

    public void StopRadar()
    {
        isRadarOn = false;
        StopCoroutine(DetectPlanet());
        StopCoroutine(CheckDistanceToPlanetsInRadarAndRemove());
        return;
    }

    public float CalculateDistanceToPlanet(int index)
    {
        if (foundPlanets[index] != null)
        {
            float dist = (foundPlanets[index].transform.position - ship.transform.position).magnitude;
            return dist;
        }
        return 0;
    }

    public void ChangeFrecuency(int _value)
    {
        _value = Mathf.Clamp(_value, -1, 1);
        if (lookedSigneld > 0 && lookedSigneld < foundPlanets.Length)
        {
            lookedSigneld += _value;
        }
        else
        {
            lookedSigneld = 0;
        }
        while (foundPlanets[lookedSigneld] == null)
        {
            lookedSigneld += _value;
        }
        return;
    }

    public IEnumerator CheckDistanceToPlanetsInRadarAndRemove()
    {
        if (foundPlanets.Length > 0)
        {
            for (int planet = 0; planet < foundPlanets.Length; planet++)
            {
                if (foundPlanets[planet] && Vector2.Distance(foundPlanets[planet].transform.position, ship.transform.position) > range)
                {
                    foundPlanets[planet] = null;
                }
            }
        }
        yield return new WaitForSeconds(rate);
        StartCoroutine(CheckDistanceToPlanetsInRadarAndRemove());
    }

    private void AddPlanetToEmptySpot(Transform _planet)
    {
        if (CheckIfIsInTheArray(_planet)) { return; }

        for (int index = 0; index < foundPlanets.Length; index++)
        {
            if (!foundPlanets[index])
            {
                foundPlanets[index] = _planet;
                return;
            }
        }
        return;
    }

    private bool CheckIfIsInTheArray(Transform _planet)
    {
        bool isInTheArray = false;
        for (int index = 0; index < foundPlanets.Length; index++)
        {
            if (_planet == foundPlanets[index])
            {
                isInTheArray = true;
                return isInTheArray;
            }
        }
        return isInTheArray;
    }
}