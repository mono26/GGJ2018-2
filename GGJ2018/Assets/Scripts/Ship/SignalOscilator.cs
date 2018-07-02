using System.Collections;
using UnityEngine;

public class SignalOscilator : ShipComponent
{
    [Header("Signal Oscilator settings")]
    [SerializeField]
    protected float minimunDistanceToPlanet = 5;    //In metters
    [SerializeField]
    protected Color planetSignal = Color.green;
    [SerializeField]
    protected Color gasStationSignal = Color.blue;
    [SerializeField]
    protected float ticksPerSecond = 10;
    [SerializeField]
    protected float screenPositionX = 0.8f;
    [SerializeField]
    protected float screenPositionY = 0.9f;

    [Header("Components")]
    [SerializeField]
    protected Radar radar;
    [SerializeField]
    protected LineRenderer oscilator;

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
        oscilator.positionCount = drawPositions.Length;

        for (int i = 0; i <= drawPositions.Length - 1; i++)
        {

            drawPositions[i].x = rayIterations * i;
            drawPositions[i].y = Mathf.Sin(drawPositions[i].x);
            oscilator.SetPositions(drawPositions);
        }

        ship.StartCoroutine(UpdateSignalOscilator());
    }

    // Update is called once per frame
    private IEnumerator UpdateSignalOscilator()
    {
        if(radar != null)
        {
            if (radar.IsRadarOn == true)
                oscilator.gameObject.SetActive(true);
            else if (radar.IsRadarOn == false)
                oscilator.gameObject.SetActive(false);

            if (radar.FoundPlanetsWithSignal.Count > radar.LookedSigneld && radar.FoundPlanetsWithSignal[radar.LookedSigneld] != null)
            {
                if (radar.FoundPlanetsWithSignal[radar.LookedSigneld].Signal.Type == SignalEmitter.SignalType.AlienPlanet)
                {
                    oscilator.startColor = planetSignal;
                    oscilator.endColor = planetSignal;
                }
                if (radar.FoundPlanetsWithSignal[radar.LookedSigneld].Signal.Type == SignalEmitter.SignalType.FuelPlanet)
                {
                    oscilator.startColor = gasStationSignal;
                    oscilator.endColor = gasStationSignal;
                }
            }
            if (radar.IsRadarOn)
            {
                if (radar.FoundPlanetsWithSignal.Count > radar.LookedSigneld && radar.FoundPlanetsWithSignal[radar.LookedSigneld] != null)
                {
                    raySpeed = 40 * (minimunDistanceToPlanet / radar.CalculateDistanceToPlanet(radar.LookedSigneld));
                    rayPeriod = 6 * (minimunDistanceToPlanet / radar.CalculateDistanceToPlanet(radar.LookedSigneld));
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
                    oscilator.SetPositions(drawPositions);
                }
            }
        }
        yield return new WaitForSeconds(1 / ticksPerSecond);
        ship.StartCoroutine(UpdateSignalOscilator());
    }

    private void LocateSignalOscilatorInTheWorld()
    {
        var camera = Camera.main;
        var position = new Vector2(camera.pixelWidth * screenPositionX, camera.pixelHeight * screenPositionY);
        position = camera.ScreenToWorldPoint(position);
        position = ship.transform.InverseTransformPoint(position);
        oscilator.transform.localPosition = position;
    }
}
