using System.Collections;
using UnityEngine;
using UnityEditor;
public class Ship : MonoBehaviour
{
    [SerializeField]
    Settings settings;

    private int planetaBuscado = 0;
    public int PlanetaBuscado { get { return planetaBuscado; } }
    public float distanciaOscilacion;

    private Rigidbody2D Rigidbody2D { get { return GetComponent<Rigidbody2D>(); } }
    private Vector2 Force;
    private bool isOnGravitationalField;

    public Components.Engine Engine;
    public Components.Radar Radar;
    public Components.AlienRay AlienRay;

    [SerializeField]
    private LayerMask planetLayer;

	// Use this for initialization
	void Start ()
    {
        Engine = new Components.Engine(this, settings.EngineSettings);
        Radar = new Components.Radar(this, settings.RadarSettings);
        AlienRay = new Components.AlienRay(this, settings.AlienRaySettings);
	}

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, Radar.settings.Range);
        Gizmos.DrawRay(transform.position, -transform.up);
    }

    // Update is called once per frame
    void Update ()
    {
        distanciaOscilacion = Radar.calcularDistancia(planetaBuscado);
        var planetasactivos = Radar.activePlanets;

        for (int i = 0; i < Radar.activePlanets.Length; i++)
        {
            if (Radar.ActivePlanets[i] != null)
            {
                if ((Radar.activePlanets[i].transform.position - this.transform.position).sqrMagnitude > 617f)
                {
                    Debug.LogWarning("si me active");
                    Radar.ActivePlanets[i] = null;
                }
            }
        }

        // Input for the radar
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (Engine.CurrentFuel > 0 && !Radar.IsRadarOn)
            {
                Radar.StartRadar();
                return;
            }
            else if(Radar.IsRadarOn)
            {
                Radar.StopRadar();
            }
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            if (!AlienRay.IsAlienRayOn)
            {
                AlienRay.StartRay();
                return;
            }
            else if (AlienRay.IsAlienRayOn)
            {
                AlienRay.StopRay();
            }
        }

        //Input for changing frecuency
        if (Input.GetKeyDown(KeyCode.L) && planetaBuscado < 6)
        {
            planetaBuscado++;
        }
        if (Input.GetKeyDown(KeyCode.K) && planetaBuscado > 0)
        {
            planetaBuscado--;
        }
        // Input from keyboard HORIZONTAL
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (Engine.CurrentFuel > 0)
            {
                Force = transform.right * -settings.HorizontalThrust;
                Rigidbody2D.AddForce(Force);
                Engine.LoseFuel();
                return;
            }
            else return;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            // Check if there is enough fuel for moving
            if (Engine.CurrentFuel > 0)
            {
                Force = transform.right * settings.HorizontalThrust;
                Rigidbody2D.AddForce(Force);
                Engine.LoseFuel();
                return;
            }
            else return;
        }

        // Input from the mouse VERTICAL
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            // Check if there is enough fuel for moving
            if (Engine.CurrentFuel > 0)
            {
                Force = transform.up * settings.VerticalThrust;
                Rigidbody2D.AddForce(Force);
                Engine.LoseFuel();
            }
            else return;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            // Check if there is enough fuel for moving
            if (Engine.CurrentFuel > 0)
            {
                Force = transform.up * -settings.VerticalThrust;
                Rigidbody2D.AddForce(Force);
                Engine.LoseFuel();
                return;
            }
            else return;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Look if is a fuel planet or a signal planet
        if (collision.gameObject.CompareTag("Fuel Planet"))
        {
            Engine.RechargeFuel();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Gravitation Field"))
        {
            Debug.Log("Entrando en la orbita del planeta");
            isOnGravitationalField = true;
            StartCoroutine(RotateArroundPlanet(collision.GetComponentInParent<Planet>()));
        }

        if (collision.gameObject.CompareTag("Alien"))
        {
            Destroy(collision.gameObject);
            GameController.Instance.IncreaseScore();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Gravitation Field"))
        {
            isOnGravitationalField = false;
        }
    }

    private IEnumerator RotateArroundPlanet(Planet _planet)
    {
        while (isOnGravitationalField)
        {
            transform.RotateAround(_planet.transform.position, _planet.transform.forward, _planet.GravitationalFieldStrenght * Time.deltaTime);
            transform.up = transform.position - _planet.transform.position;
            yield return null;
        }
        yield return null;
    }

    [System.Serializable]
    public class Settings
    {
        public float VerticalThrust = 1.0f;
        public float HorizontalThrust = 1.0f;

        public Components.Engine.Settings EngineSettings;
        public Components.Radar.Settings RadarSettings;
        public Components.AlienRay.Settings AlienRaySettings;
    }
}
