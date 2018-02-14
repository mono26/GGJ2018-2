using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalOscillator : MonoBehaviour
{
    private LineRenderer signalOscillator;

    [SerializeField]
    private Ship ship;
    private float minimunDistanceToPlanet = 5; //In centimeters

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

    public float ratio;

    [SerializeField]
    private float screenPositionX = 0.8f;
    [SerializeField]
    private float screenPositionY = 0.9f;

    // Use this for initialization
    void Start ()
    {
        LocateSignalOscilatorInTheWorld();

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

        StartCoroutine(UpdateSignalOscilator());
    }
	
	// Update is called once per frame
	private IEnumerator UpdateSignalOscilator()
    {
        if (ship.Radar.FoundPlanets.Length > ship.Radar.LookedSigneld && ship.Radar.FoundPlanets[ship.Radar.LookedSigneld] != null)
        {
            if (ship.Radar.FoundPlanets[ship.Radar.LookedSigneld].gameObject.tag == "Fuel Planet")
            {
                signalOscillator.startColor = planetSignal;
                signalOscillator.endColor = planetSignal;
            }
            if (ship.Radar.FoundPlanets[ship.Radar.LookedSigneld].gameObject.tag == "Score Planet")
            {
                signalOscillator.startColor = gasStationSignal;
                signalOscillator.endColor = gasStationSignal;
            }
        }
        if (ship.Radar.IsRadarOn)
        {
            if (ship.Radar.FoundPlanets.Length > ship.Radar.LookedSigneld && ship.Radar.FoundPlanets[ship.Radar.LookedSigneld] != null)
            {
                raySpeed = 40 * (minimunDistanceToPlanet * minimunDistanceToPlanet / ship.Radar.CalculateDistanceToPlanet(ship.Radar.LookedSigneld));
                rayPeriod = 6 * (minimunDistanceToPlanet * minimunDistanceToPlanet / ship.Radar.CalculateDistanceToPlanet(ship.Radar.LookedSigneld));
            }
            else
            {
                raySpeed = 0;
                rayPeriod = 0;
            }

            for (int i = 0; i <= drawPositions.Length - 1; i++)
            {
                drawPositions[i].y = Mathf.Sin(rayPeriod * drawPositions[i].x + Time.time * raySpeed);
                signalOscillator.SetPositions(drawPositions);
            }
        }
        yield return new WaitForSeconds(ratio);
        StartCoroutine(UpdateSignalOscilator());
    }

    private void LocateSignalOscilatorInTheWorld()
    {
        var camera = Camera.main;
        var position = new Vector2(camera.pixelWidth * screenPositionX, camera.pixelHeight * screenPositionY);
        position = camera.ScreenToWorldPoint(position);
        position = ship.transform.InverseTransformPoint(position);
        transform.localPosition = position;
    }
}
