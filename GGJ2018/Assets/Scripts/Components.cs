using System.Collections;
using UnityEngine;
using UnityEditor;

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
            currentFuel = settings.maxFuel;
        }

        public void RechargeFuel()
        {
            currentFuel = settings.maxFuel;
        }

        public void LoseFuel()
        {
            if (ship.Radar.IsRadarOn)
            {
                currentFuel -= 2 * settings.fuelLossRate;
            }
            else currentFuel -= 1 * settings.fuelLossRate;
        }

        [System.Serializable]
        public class Settings
        {
            public float maxFuel = 9999.0f;
            public float fuelLossRate = 1.0f;
        }
    }

    [System.Serializable]
    public class Radar
    {
        private Ship ship;
        public Settings settings;

        [SerializeField]
        public Collider2D[] ActivePlanets = new Collider2D[6];
        public Collider2D[] activePlanets { get { return ActivePlanets; } }
        private int activeIndex = 0;

        [SerializeField]
        bool isRadarOn;
        public bool IsRadarOn { get { return isRadarOn; } }

        [SerializeField]
        private Transform target;
        public Transform Target { get { return target; } set { target = value; } }

        private Coroutine routine = null;

        public Radar(Ship _ship, Settings _settings)
        {
            ship = _ship;
            settings = _settings;
        }

        // Method for detecting planets in range
        private IEnumerator DetectPlanet()
        {
            Debug.Log("Radar is ticking");
            // Check for all the colliders and store it in a variable
            Collider2D[] planets = Physics2D.OverlapCircleAll(ship.transform.position, settings.Range, settings.LayerMask);
            if (planets.Length > 0)
            {
                // Saves the distance of the first object
                var distance1 = (planets[0].transform.position - ship.transform.position).sqrMagnitude;
                Target = planets[0].transform;
                foreach (Collider2D planet in planets)
                {
                    if (planet.gameObject.tag == "Score Planet" || planet.gameObject.tag == "Fuel Planet")
                    {
                        bool existe = ArrayUtility.Contains(ActivePlanets, planet);
                        if (!existe)
                        {
                            ActivePlanets[activeIndex] = planet;
                            if (activeIndex < ActivePlanets.Length)
                                activeIndex++;
                            else
                                activeIndex = 0;
                           

                        }
                    }
                    var distance2 = (planet.transform.position - ship.transform.position).sqrMagnitude;
                    if (distance2 < distance1)
                    {
                        distance1 = distance2;
                        Target = planet.transform;
                    }
                    else yield return null;
                   
                }

            }

            else yield return null;
            yield return new WaitForSeconds(settings.Rate);
            routine = ship.StartCoroutine(DetectPlanet());
        }

        public void StartRadar()
        {
            isRadarOn = true;
            routine = ship.StartCoroutine(DetectPlanet());
        }

        public void StopRadar()
        {
            isRadarOn = false;
            ship.StopCoroutine(routine);
        }

        public float calcularDistancia(int index)
        {
            if (ActivePlanets[index] != null)
            {
                float dist = (ActivePlanets[index].transform.position - ship.transform.position).sqrMagnitude;
                return dist;
            }
            return 0;

        }
        [System.Serializable]
        public class Settings
        {
            public float Range;
            public float Rate = 1.0f;
            public LayerMask LayerMask;
        }
    }

    [System.Serializable]
    public class AlienRay
    {
        private Ship ship;
        public Settings settings;

        private RaycastHit2D[] aliensHit;

        private bool isAlienRayOn;
        public bool IsAlienRayOn { get { return isAlienRayOn; } }

        Coroutine routine = null;

        public AlienRay(Ship _ship, Settings _settings)
        {
            ship = _ship;
            settings = _settings;
        }

        private IEnumerator FindAliens()
        {
            Debug.Log("Alien Ray is ticking");
            aliensHit = Physics2D.CircleCastAll(ship.transform.position, settings.Radius, -ship.transform.forward, settings.Range, settings.LayerMask);
            if (aliensHit.Length > 0)
            {
                Debug.Log("Econtre colliders");
                foreach (RaycastHit2D hit in aliensHit)
                {
                    if (hit.collider.CompareTag("Alien"))
                    {
                        Debug.Log("Le di al alien");
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
        }

        public void StopRay()
        {
            isAlienRayOn = false;
            ship.StopCoroutine(routine);
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
}
