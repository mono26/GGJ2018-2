using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalOscillator : MonoBehaviour {

    private LineRenderer signalOscillator;

    [SerializeField]
    private Ship ship;
    private float distance;

    [SerializeField]
    private Vector3[] drawPositions;

    [SerializeField]
    private float rayLenght = 3f;
    [SerializeField]
    private float rayIterations;

    [Range(1, 40)]
    public float raySpeed = 5;
    [Range(1,6)]
    public float rayPeriod = 10;

    public Color planetSignal;
    public Color gasStationSignal;
    public Color enemySignal;

    public bool planetDetected;
    public bool gasStationDetected;
    public bool enemyDetected;

    public float ratio;

    // Use this for initialization
    void Start () {
        signalOscillator = GetComponent<LineRenderer>();
        ship = GameObject.Find("Player").GetComponent<Ship>();

        rayIterations = rayLenght / drawPositions.Length;
        signalOscillator.positionCount = drawPositions.Length;

        for (int i = 0; i <= drawPositions .Length -1 ; i++)
        {
         
            drawPositions[i].x = rayIterations * i;
            drawPositions[i].y = Mathf.Sin(drawPositions[i].x);
            signalOscillator.SetPositions(drawPositions);
        }
      
	}
	
	// Update is called once per frame
	void Update ()
    {
        //distance= ship.distanciaOscilacion
        //Debug.Log(ship.distanciaOscilacion);
        if (ship.Radar.ActivePlanets[ship.PlanetaBuscado] != null)
        {
            if (ship.Radar.ActivePlanets[ship.PlanetaBuscado].gameObject.tag == "Fuel Planet")
            {
                gasStationDetected = true;
            }
            if (ship.Radar.ActivePlanets[ship.PlanetaBuscado].gameObject.tag == "Score Planet")
            {
                planetDetected = true;
            }
        }
        if (ship.Radar.IsRadarOn)
        {
            if (ship.Radar.activePlanets[ship.PlanetaBuscado] != null)
            {
                raySpeed = 40 - (ship.distanciaOscilacion / 15.4f);
                rayPeriod = 6 - (ship.distanciaOscilacion / 102.8f);
            }
            if (planetDetected)
            {
                signalOscillator.startColor = planetSignal;
                signalOscillator.endColor = planetSignal;
                gasStationDetected = false;
                enemyDetected = false;
            }

                   if (gasStationDetected)
                   {
                       signalOscillator.startColor = gasStationSignal;
                       signalOscillator.endColor = gasStationSignal;
                       planetDetected  = false;
                       enemyDetected = false;
                   }
                   if (enemyDetected)
                   {
                       signalOscillator.startColor = enemySignal;
                       signalOscillator.endColor = enemySignal;
                       planetDetected = false;
                       gasStationDetected = false;
                   }

                for (int i = 0; i <= drawPositions.Length - 1; i++)
                {

                    drawPositions[i].y = Mathf.Sin(rayPeriod * drawPositions[i].x + Time.time * raySpeed);
                    signalOscillator.SetPositions(drawPositions);
                }
            /*   for (int i = 0; i <= drawPositions.Length - 1; i++)
           {

               drawPositions[i].y = Mathf.Sin(rayPeriod  *drawPositions[i].x + Time.time * raySpeed);
               signalOscillator.SetPositions(drawPositions);
           }*/
        }
    }

}
