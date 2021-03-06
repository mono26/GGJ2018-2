﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : ShipComponent
{
    [Header("Radar settings")]
    [SerializeField] float range = 30f;
    [SerializeField] float ticksPerSecond = 1.0f;
    [SerializeField] LayerMask planetsLayer;

    [Header("Editor debugging")]
    [SerializeField] Planet foundPlanetWithSignal = null;
    [SerializeField] bool isRadarOn = false;

    public Planet FoundPlanetsWithSignal { get { return foundPlanetWithSignal; } }
    public bool IsRadarOn { get { return isRadarOn; } }

    Coroutine planetDetection;
    Coroutine distanceDetection;

    protected virtual void Start()
    {
        foundPlanetWithSignal = null;

        isRadarOn = false;

        ToggleRadar();
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    // Method for detecting planets in range
    IEnumerator DetectPlanet()
    {
        Collider2D[] nearObjects = Physics2D.OverlapCircleAll(ship.transform.position, range, planetsLayer);
        if (nearObjects.Length > 0)
        {
            List<Planet> nearPlanets = new List<Planet>();
            foreach (Collider2D obj in nearObjects)
            {
                if (obj.gameObject.CompareTag("Planet") || obj.gameObject.CompareTag("FuelPlanet"))
                {
                    Planet planetComponent = obj.GetComponent<Planet>();
                    if (planetComponent == null)
                    {
                        planetComponent = obj.GetComponentInParent<Planet>();
                    }
                    if (planetComponent == null || planetComponent.GetSignal ==  null) 
                    {
                        continue;
                    }
                    if (planetComponent.GetSignal.GetState == SignalEmitter.SignalState.OFF)
                    {
                        continue;
                    }

                    nearPlanets.Add(planetComponent);
                }
            }

            AddNearestPlanetToRadar(nearPlanets);
        }

        yield return new WaitForSeconds(1 / ticksPerSecond);

        planetDetection = StartCoroutine(DetectPlanet());
    }

    void AddNearestPlanetToRadar(List<Planet> _planetsToCheck)
    {
        if(_planetsToCheck.Count == 0)
        {
            return;
        }

        float minDistance = CalculateSqrDistanceToPlanet(_planetsToCheck[0]);
        int planet = 0;
        for (int i = 0; i < _planetsToCheck.Count; i++)
        {
            float distance = CalculateSqrDistanceToPlanet(_planetsToCheck[i]);
            if (minDistance > distance)
            {
                minDistance = distance;
                planet = i;
            }
        }

        foundPlanetWithSignal = _planetsToCheck[planet];
    }

    public float CalculateSqrDistanceToPlanet(Planet _planet)
    {
        float dist = float.MaxValue;
        if (_planet == null)
        {
            return dist;
        }

        dist = (_planet.transform.position - ship.transform.position).sqrMagnitude;
        return dist;
    }

    IEnumerator CheckIfPlanetIsOutOfBoundsAndRemove()
    {
        if (foundPlanetWithSignal != null)
        {
            float maxRange = (range + foundPlanetWithSignal.GetRadius) * (range + foundPlanetWithSignal.GetRadius);
            if (CalculateSqrDistanceToPlanet(foundPlanetWithSignal) > maxRange)
            {
                foundPlanetWithSignal = null;
            }
        }
        
        yield return new WaitForSeconds(1 / ticksPerSecond);
        distanceDetection = StartCoroutine(CheckIfPlanetIsOutOfBoundsAndRemove());
    }

    void OnTriggerEnter2D(Collider2D _collider) 
    {
        if (!_collider.CompareTag("GravitationField"))
        {
            return;
        }
        
        ToggleRadar();
    }

    void OnTriggerExit2D(Collider2D other) 
    {
        if (!other.CompareTag("GravitationField"))
        {
            return;
        }

        ToggleRadar();
    }

    void ToggleRadar()
    {
        isRadarOn = !isRadarOn;
        if (isRadarOn) 
        {
            planetDetection = StartCoroutine(DetectPlanet());
            distanceDetection = StartCoroutine(CheckIfPlanetIsOutOfBoundsAndRemove());
        }

        if (!isRadarOn && planetDetection != null)
        {
            StopCoroutine(planetDetection);
        }

        if (!isRadarOn && distanceDetection != null)
        {
            StopCoroutine(distanceDetection);
        }
    }
}
