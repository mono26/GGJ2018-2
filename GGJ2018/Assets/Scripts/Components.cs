using System.Collections;
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
        public Collider2D[] activePlanets = new Collider2D[6];
        public Collider2D[] ActivePlanets { get { return activePlanets; } }
        private int activeIndex = 0;

        private bool existe = false;

        [SerializeField]
        bool isRadarOn;
        public bool IsRadarOn { get { return isRadarOn; } }

        /*[SerializeField]
        private Transform target;
        public Transform Target { get { return target; } set { target = value; } }*/

        private Coroutine routine = null;

        public void Update()
        {

        }

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
                //Target = planets[0].transform;
                foreach (Collider2D planet in planets)
                {
                    if (planet.gameObject.tag == "Score Planet" || planet.gameObject.tag == "Fuel Planet")
                    {
                        foreach (Collider2D planeta in activePlanets)
                        {
                            if (planeta == planet)
                            {
                                existe = true;
                            }
                            else
                            {
                                existe = false;
                            }
                        }
                        if (!existe)
                        {
                            activePlanets[activeIndex] = planet;
                            if (activeIndex < activePlanets.Length -1)
                                activeIndex++;
                            else
                                activeIndex = 0;
                        }
                    }
                    /* var distance2 = (planet.transform.position - ship.transform.position).sqrMagnitude;
                    if (distance2 < distance1)
                    {
                        distance1 = distance2;
                        Target = planet.transform;
                    }*/
                    else yield return null;
                }
                yield return null;
            }
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
            if (activePlanets[index] != null)
            {
                float dist = (activePlanets[index].transform.position - ship.transform.position).sqrMagnitude;
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
            settings.SpriteEffect.SetActive(true);
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
            settings.SpriteEffect.SetActive(false);
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

            public GameObject SpriteEffect;
        }
    }
}
