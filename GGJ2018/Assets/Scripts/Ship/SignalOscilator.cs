using System.Collections;
using UnityEngine;

public class SignalOscilator : MonoBehaviour
{
    [Header("Signal Oscilator settings")]
    [SerializeField]
    protected float MinimunDistanceToPlanet = 5;    //In metters
    [SerializeField]
    protected Color PlanetSignal;
    [SerializeField]
    protected Color GasStationSignal;
    [SerializeField]
    protected float Ratio;
    [SerializeField]
    protected float ScreenPositionX = 0.8f;
    [SerializeField]
    protected float ScreenPositionY = 0.9f;

    [Header("Components")]
    protected LineRenderer signalOscillator;
    protected Ship ship;

    [Header("Editor debugging")]
    [SerializeField]
    protected Vector3[] drawPositions = new Vector3[10];
    [SerializeField]
    protected float rayLenght = 3f;
    [SerializeField]
    protected float rayIterations;
    [Range(1, 40)]
    protected float raySpeed = 5;
    [Range(1, 6)]
    protected float rayPeriod = 10;

    // Use this for initialization
    public void Start()
    {
        LocateSignalOscilatorInTheWorld();

        rayIterations = rayLenght / drawPositions.Length;
        signalOscillator.positionCount = drawPositions.Length;

        for (int i = 0; i <= drawPositions.Length - 1; i++)
        {

            drawPositions[i].x = rayIterations * i;
            drawPositions[i].y = Mathf.Sin(drawPositions[i].x);
            signalOscillator.SetPositions(drawPositions);
        }

        ship.StartCoroutine(UpdateSignalOscilator());
    }

    // Update is called once per frame
    private IEnumerator UpdateSignalOscilator()
    {
        if (ship.Radar.FoundPlanets.Length > ship.Radar.LookedSigneld && ship.Radar.FoundPlanets[ship.Radar.LookedSigneld] != null)
        {
            if (ship.Radar.FoundPlanets[ship.Radar.LookedSigneld].gameObject.tag == "Fuel Planet")
            {
                signalOscillator.startColor = PlanetSignal;
                signalOscillator.endColor = PlanetSignal;
            }
            if (ship.Radar.FoundPlanets[ship.Radar.LookedSigneld].gameObject.tag == "Score Planet")
            {
                signalOscillator.startColor = GasStationSignal;
                signalOscillator.endColor = GasStationSignal;
            }
        }
        if (ship.Radar.IsRadarOn)
        {
            if (ship.Radar.FoundPlanets.Length > ship.Radar.LookedSigneld && ship.Radar.FoundPlanets[ship.Radar.LookedSigneld] != null)
            {
                raySpeed = 40 * (MinimunDistanceToPlanet / ship.Radar.CalculateDistanceToPlanet(ship.Radar.LookedSigneld));
                rayPeriod = 6 * (MinimunDistanceToPlanet / ship.Radar.CalculateDistanceToPlanet(ship.Radar.LookedSigneld));
                raySpeed = Mathf.Clamp(raySpeed, 0, 40);
                rayPeriod = Mathf.Clamp(rayPeriod, 0, 40);
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
        yield return new WaitForSeconds(Ratio);
        ship.StartCoroutine(UpdateSignalOscilator());
    }

    private void LocateSignalOscilatorInTheWorld()
    {
        var camera = Camera.main;
        var position = new Vector2(camera.pixelWidth * ScreenPositionX, camera.pixelHeight * ScreenPositionY);
        position = camera.ScreenToWorldPoint(position);
        position = ship.transform.InverseTransformPoint(position);
        signalOscillator.transform.localPosition = position;
    }
}
