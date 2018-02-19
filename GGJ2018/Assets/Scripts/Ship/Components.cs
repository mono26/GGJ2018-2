using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Components
{
    [System.Serializable]
    public class Engine
    {
        private Ship ship;
        public Settings settings;

        [SerializeField]
        private float currentFuel;
        public float CurrentFuel { get { return currentFuel; } }

        // Event for fuel loss
            // Fijarse si el radar esta prendido
        // Event for recharging fuel
        public Engine(Ship _ship, Settings _settings)
        {
            ship = _ship;
            settings = _settings;
            currentFuel = settings.MaxFuel;
            return;
        }

        public void ApplyForce(Vector3 _direction)
        {
            ship.Rigidbody2D.AddForce(_direction * settings.Thrust);
            return;
        }

        public void RechargeFuel()
        {
            currentFuel = settings.MaxFuel;
            return;
        }

        public void LoseFuel()
        {
            if (ship.Radar.IsRadarOn)
            {
                currentFuel -= 2 * settings.FuelLossRate;
                return;
            }
            else
            {
                currentFuel -= 1 * settings.FuelLossRate;
                return;
            }
        }

        public void RecieveDamage(int daño)
        {
            currentFuel -= daño;
            return;
        }

        [System.Serializable]
        public class Settings
        {
            public float Thrust = 1.0f;

            public float MaxFuel = 9999.0f;
            public float FuelLossRate = 1.0f;
        }
    }

    [System.Serializable]
    public class Radar
    {
        private Ship ship;
        public LineRenderer signalOscillator;
        public Settings settings;

        [SerializeField]
        private Transform[] foundPlanets;
        [SerializeField]
        private int lookedSigneld = 0;
        [SerializeField]
        bool isRadarOn;

        public Transform[] FoundPlanets { get { return foundPlanets; } }
        public int LookedSigneld { get { return lookedSigneld; } }
        public bool IsRadarOn { get { return isRadarOn; } }

        private Coroutine lookForPlanets = null;
        private Coroutine lookDistanceToPlanet = null;

        public Radar(Ship _ship, Settings _settings)
        {
            ship = _ship;
            settings = _settings;
            foundPlanets = new Transform[settings.maxRadarCapacity];
            signalOscillator = _ship.settings.LineRenderer;
            return;
        }

        // Method for detecting planets in range
        private IEnumerator DetectPlanet()
        {
            // Check for all the colliders and store it in a variable
            Collider2D[] planets = Physics2D.OverlapCircleAll(ship.transform.position, settings.Range, settings.LayerMask);
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
            yield return new WaitForSeconds(settings.Rate);
            lookForPlanets = ship.StartCoroutine(DetectPlanet());
        }

        public void StartRadar()
        {
            isRadarOn = true;
            lookForPlanets = ship.StartCoroutine(DetectPlanet());
            lookDistanceToPlanet = ship.StartCoroutine(CheckDistanceToPlanetsInRadarAndRemove());
            return;
        }

        public void StopRadar()
        {
            isRadarOn = false;
            ship.StopCoroutine(lookForPlanets);
            ship.StopCoroutine(lookDistanceToPlanet);
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
                    if (foundPlanets[planet] && Vector2.Distance(foundPlanets[planet].transform.position, ship.transform.position) > settings.Range)
                    {
                        foundPlanets[planet] = null;
                    }
                }
            }
            yield return new WaitForSeconds(settings.Rate);
            lookDistanceToPlanet = ship.StartCoroutine(CheckDistanceToPlanetsInRadarAndRemove());
        }

        private void AddPlanetToEmptySpot(Transform _planet)
        {
            if (CheckIfIsInTheArray(_planet)) { return; }

            for(int index = 0; index < foundPlanets.Length; index++)
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
            for(int index = 0; index < foundPlanets.Length; index++)
            {
                if(_planet == foundPlanets[index])
                {
                    isInTheArray = true;
                    return isInTheArray;
                }
            }
            return isInTheArray;
        }

        [System.Serializable]
        public class Settings
        {
            public float Range;
            public float Rate = 1.0f;
            public LayerMask LayerMask;
            public int maxRadarCapacity;
        }
    }

    [System.Serializable]
    public class AlienRay
    {
        private Ship ship;
        private GameObject spriteEffect;
        public Settings settings;

        private RaycastHit2D[] aliensHit;

        private bool isAlienRayOn;
        public bool IsAlienRayOn { get { return isAlienRayOn; } }

        Coroutine routine = null;

        public AlienRay(Ship _ship, Settings _settings)
        {
            ship = _ship;
            settings = _settings;
            spriteEffect = _ship.settings.UfoRay;
        }

        private IEnumerator FindAliens()
        {
            aliensHit = Physics2D.CircleCastAll(ship.transform.position, settings.Radius, -ship.transform.up, settings.Range, settings.LayerMask);
            if (aliensHit.Length > 0)
            {
                foreach (RaycastHit2D hit in aliensHit)
                {
                    if (hit.collider.CompareTag("Alien"))
                    {
                        hit.collider.GetComponent<Rigidbody2D>().AddForce(hit.transform.up * settings.Strenght);
                    }
                }
            }
            else yield return null;
            yield return new WaitForSeconds(settings.Rate);
            routine = ship.StartCoroutine(FindAliens());
        }

        public void StartRay()
        {
            isAlienRayOn = true;
            routine = ship.StartCoroutine(FindAliens());
            spriteEffect.SetActive(true);
        }

        public void StopRay()
        {
            isAlienRayOn = false;
            ship.StopCoroutine(routine);
            spriteEffect.SetActive(false);
        }

        [System.Serializable]
        public class Settings
        {
            public float Range;
            public float Radius;
            public float Strenght;
            public float Rate;
            public LayerMask LayerMask;
        }
    }

    [System.Serializable]
    public class SignalOscillator
    {
        private LineRenderer signalOscillator;
        private Ship ship;
        public Settings settings;

        [SerializeField]
        private Vector3[] drawPositions = new Vector3[10];

        [SerializeField]
        private float rayLenght = 3f;
        [SerializeField]
        private float rayIterations;

        [Range(1, 40)]
        private float raySpeed = 5;
        [Range(1, 6)]
        private float rayPeriod = 10;

        public SignalOscillator(Ship _ship, Settings _settings)
        {
            ship = _ship;
            settings = _settings;
            signalOscillator = _ship.settings.LineRenderer;
        }

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
                    signalOscillator.startColor = settings.PlanetSignal;
                    signalOscillator.endColor = settings.PlanetSignal;
                }
                if (ship.Radar.FoundPlanets[ship.Radar.LookedSigneld].gameObject.tag == "Score Planet")
                {
                    signalOscillator.startColor = settings.GasStationSignal;
                    signalOscillator.endColor = settings.GasStationSignal;
                }
            }
            if (ship.Radar.IsRadarOn)
            {
                if (ship.Radar.FoundPlanets.Length > ship.Radar.LookedSigneld && ship.Radar.FoundPlanets[ship.Radar.LookedSigneld] != null)
                {
                    raySpeed = 40 * (settings.MinimunDistanceToPlanet / ship.Radar.CalculateDistanceToPlanet(ship.Radar.LookedSigneld));
                    rayPeriod = 6 * (settings.MinimunDistanceToPlanet / ship.Radar.CalculateDistanceToPlanet(ship.Radar.LookedSigneld));
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
            yield return new WaitForSeconds(settings.Ratio);
            ship.StartCoroutine(UpdateSignalOscilator());
        }

        private void LocateSignalOscilatorInTheWorld()
        {
            var camera = Camera.main;
            var position = new Vector2(camera.pixelWidth * settings.ScreenPositionX, camera.pixelHeight * settings.ScreenPositionY);
            position = camera.ScreenToWorldPoint(position);
            position = ship.transform.InverseTransformPoint(position);
            signalOscillator.transform.localPosition = position;
        }

        [System.Serializable]
        public class Settings
        {
            public float MinimunDistanceToPlanet = 5; //In centimeters
            public Color PlanetSignal;
            public Color GasStationSignal;

            public float Ratio;

            public float ScreenPositionX = 0.8f;
            public float ScreenPositionY = 0.9f;
        }
    }
}
