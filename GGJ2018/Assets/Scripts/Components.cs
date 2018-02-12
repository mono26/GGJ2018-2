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
            settings.RigidBody.AddForce(_direction * settings.Thrust);
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
            public Rigidbody2D RigidBody;

            public float Thrust = 1.0f;

            public float MaxFuel = 9999.0f;
            public float FuelLossRate = 1.0f;
        }
    }

    [System.Serializable]
    public class Radar
    {
        private Ship ship;
        public Settings settings;

        [SerializeField]
        private Transform[] foundPlanets;
        private int lookedSigneld = 0;
        private float distanceToPlanet;
        [SerializeField]
        bool isRadarOn;

        public Transform[] FoundPlanets { get { return foundPlanets; } }
        public int LookedSigneld { get { return lookedSigneld; } }
        public float DistanceToPlanet { get { return distanceToPlanet; } }
        public bool IsRadarOn { get { return isRadarOn; } }

        private Coroutine lookForPlanets = null;
        private Coroutine lookDistanceToPlanet = null;

        public Radar(Ship _ship, Settings _settings)
        {
            ship = _ship;
            settings = _settings;
            foundPlanets = new Transform[settings.maxRadarCapacity];
        }

        // Method for detecting planets in range
        private IEnumerator DetectPlanet()
        {
            Debug.Log("Radar is ticking");
            // Check for all the colliders and store it in a variable
            Collider2D[] planets = Physics2D.OverlapCircleAll(ship.transform.position, settings.Range, settings.LayerMask);
            Debug.Log(planets.Length);
            if (planets.Length > 0)
            {
                for (int planet = 0; planet < planets.Length; planet++)
                {
                    if (planets[planet].gameObject.CompareTag("Score Planet") || planets[planet].gameObject.CompareTag("Fuel Planet"))
                    {
                        if(foundPlanets[planet] == null)
                            foundPlanets[planet] = planets[planet].transform;
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
        }

        public void StopRadar()
        {
            isRadarOn = false;
            ship.StopCoroutine(lookForPlanets);
            ship.StopCoroutine(lookDistanceToPlanet);
        }

        public float CalcularDistancia(int index)
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
            else return;
        }

        public IEnumerator CheckDistanceToPlanetsInRadarAndRemove()
        {
            if (foundPlanets.Length > 0)
            {
                for (int planet = 0; planet < foundPlanets.Length; planet++)
                {
                    Debug.Log("Looking distance to found planets");
                    if (foundPlanets[planet] && Vector2.Distance(foundPlanets[planet].transform.position, ship.transform.position) > settings.Range)
                    {
                        foundPlanets[planet] = null;
                    }
                }
            }
            yield return new WaitForSeconds(settings.Rate);
            lookDistanceToPlanet = ship.StartCoroutine(CheckDistanceToPlanetsInRadarAndRemove());
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
            settings.SpriteEffect.SetActive(true);
        }

        public void StopRay()
        {
            isAlienRayOn = false;
            ship.StopCoroutine(routine);
            settings.SpriteEffect.SetActive(false);
        }

        [System.Serializable]
        public class Settings
        {
            public float Range;
            public float Radius;
            public float Strenght;
            public float Rate;
            public LayerMask LayerMask;

            public GameObject SpriteEffect;
        }
    }
}
