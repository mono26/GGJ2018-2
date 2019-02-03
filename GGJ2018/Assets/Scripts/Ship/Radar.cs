using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radar : ShipComponent
{
    [Header("Radar settings")]
    [SerializeField]
    protected float range = 30f;
    [SerializeField]
    protected float ticksPerSecond = 1.0f;
    [SerializeField]
    protected LayerMask layerMask;

    [Header("Editor debugging")]
    [SerializeField]
    protected List<Planet> foundPlanetsWithSignal;
    public List<Planet> FoundPlanetsWithSignal { get { return foundPlanetsWithSignal; } }
    [SerializeField]
    protected bool isRadarOn;
    public bool IsRadarOn { get { return isRadarOn; } }
    [SerializeField]
    protected int lookedSignal = 0;
    public int LookedSigneld { get { return lookedSignal; } }
    [SerializeField]
    protected Collider2D[] nearplanets;

    protected Coroutine planetDetection;
    protected Coroutine distanceDetection;

    protected virtual void Start()
    {
        foundPlanetsWithSignal = new List<Planet>();
        return;
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, range);
    }

    protected void OnEnable()
    {
        isRadarOn = false;
        return;
    }

    // Method for detecting planets in range
    private IEnumerator DetectPlanet()
    {
        nearplanets = Physics2D.OverlapCircleAll(ship.transform.position, range, layerMask);
        if (nearplanets.Length > 0)
        {
            foreach (Collider2D planet in nearplanets)
            {
                if (planet.gameObject.CompareTag("Planet"))
                {
                    Planet planetComponent = planet.GetComponent<Planet>();
                    if (planetComponent.Signal !=  null) {
                        AddPlanetWithSignal(planetComponent);
                    }
                }
            }
        }
        yield return new WaitForSeconds(1 / ticksPerSecond);
        planetDetection = StartCoroutine(DetectPlanet());
        yield break;
    }

    private void AddPlanetWithSignal(Planet _planet)
    {
        if (foundPlanetsWithSignal.Contains(_planet) == false)
        {
            Debug.Log(_planet.gameObject.name + "Adding Planet" + _planet);
            foundPlanetsWithSignal.Add(_planet);
        }
        return;
    }

    private IEnumerator CheckDistanceToPlanetsInRadarAndRemove()
    {
        if (foundPlanetsWithSignal.Count > 0)
        {
            for (int i = 0; i < foundPlanetsWithSignal.Count; i++)
            {
                if (foundPlanetsWithSignal[i] != null && CalculateDistanceToPlanet(i) > range + foundPlanetsWithSignal[i].GetPlanetRadius)
                {
                    foundPlanetsWithSignal.Remove(foundPlanetsWithSignal[i]);
                    if (lookedSignal >= foundPlanetsWithSignal.Count){
                        lookedSignal = foundPlanetsWithSignal.Count - 1;
                    }
                }
            }
        }
        yield return new WaitForSeconds(1 / ticksPerSecond);
        distanceDetection = StartCoroutine(CheckDistanceToPlanetsInRadarAndRemove());
    }

    private void StartRadar()
    {
        isRadarOn = true;
        planetDetection = StartCoroutine(DetectPlanet());
        distanceDetection = StartCoroutine(CheckDistanceToPlanetsInRadarAndRemove());
        return;
    }

    private void StopRadar()
    {
        isRadarOn = false;
        StopCoroutine(planetDetection);
        StopCoroutine(distanceDetection);
        return;
    }

    public void ActivateRadar()
    {
        isRadarOn = !isRadarOn;
        if (isRadarOn) 
        {
            StartRadar();
        }
        else 
        {
            StopRadar();
        }
        return;
    }

    public float CalculateDistanceToPlanet(int index)
    {
        if (foundPlanetsWithSignal[index] != null)
        {
            float dist = Vector3.Distance(ship.transform.position, foundPlanetsWithSignal[index].transform.position);
            return dist;
        }
        return 0;
    }

    public void ChangeFrecuency(int _value)
    {
        _value = Mathf.Clamp(_value, -1, 1);
        if (lookedSignal >= 0 && lookedSignal < foundPlanetsWithSignal.Count) {
            lookedSignal += _value;
        }
        else {
            lookedSignal = 0;
        }
        if(foundPlanetsWithSignal.Count > 0)
        {
            while (foundPlanetsWithSignal[lookedSignal] == null)
            {
                if (lookedSignal >= 0 && lookedSignal < foundPlanetsWithSignal.Count) {
                    lookedSignal += _value;
                }
                else {
                    lookedSignal = 0;
                }
            }
        }
        return;
    }
}
