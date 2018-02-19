﻿using System.Collections;
using UnityEngine;

public class Ship : MonoBehaviour
{
    public Settings settings;

    public Rigidbody2D Rigidbody2D { get { return settings.Rigidbody2D; } }

    private Vector2 Force;
    private bool isOnGravitationalField;

    private Components.Engine engine;
    private Components.Radar radar;
    private Components.AlienRay alienRay;
    private Components.SignalOscillator signalOscillator;

    public Components.Engine Engine { get { return engine; } }
    public Components.Radar Radar { get { return radar; } }
    public Components.AlienRay AlienRay { get { return alienRay; } }
    public Components.SignalOscillator SignalOscillator { get { return signalOscillator; } }

	// Use this for initialization
	void Awake()
    {
        engine = new Components.Engine(this, settings.EngineSettings);
        radar = new Components.Radar(this, settings.RadarSettings);
        alienRay = new Components.AlienRay(this, settings.AlienRaySettings);
        signalOscillator = new Components.SignalOscillator(this, settings.SignalOscillator);
	}

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, Radar.settings.Range);
        Gizmos.DrawWireSphere(transform.position, AlienRay.settings.Radius);
    }

    private void Start()
    {
        signalOscillator.Start();
    }
    // Update is called once per frame
    void Update ()
    {
        // Input for the radar
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (Engine.CurrentFuel > 0 && !Radar.IsRadarOn)
            {
                Radar.StartRadar();
                return;
            }
            else if(Radar.IsRadarOn)
            {
                Radar.StopRadar();
                return;
            }
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (!AlienRay.IsAlienRayOn)
            {
                AlienRay.StartRay();
                return;
            }
            else if (AlienRay.IsAlienRayOn)
            {
                AlienRay.StopRay();
                return;
            }
        }

        //TODO use change frecuency method
        //Input for changing frecuency
        if (Input.GetKeyDown(KeyCode.L))
        {
            //TODO method for increase the index of the looked planet
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            //TODO method for decrease the index of the looked planet
        }

        // Input from keyboard HORIZONTAL
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (Engine.CurrentFuel > 0)
            {
                Engine.ApplyForce(-transform.right);
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
                Engine.ApplyForce(transform.right);
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
                Engine.ApplyForce(transform.up);
                Engine.LoseFuel();
            }
            else return;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            // Check if there is enough fuel for moving
            if (Engine.CurrentFuel > 0)
            {
                Engine.ApplyForce(-transform.up);
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
        if (collision.gameObject.CompareTag("Asteroide"))
        {
            Engine.RecieveDamage(25);
            Debug.Log("Recibi daño de asteroide");
            collision.gameObject.GetComponent<Asteroide>().ReleaseAsteroid();
            //TODO: definir bien el daño del asteroide
            //TODO Enviar o destruir asteoroide
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

        if (collision.gameObject.CompareTag("Blackhole"))
        {
            Debug.Log("Recibi daño de Hoyo Negro");
            Engine.RecieveDamage(30);
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
            var direction = (transform.position - _planet.transform.position).normalized;
            transform.up = Vector2.Lerp(transform.up, direction, 0.05f);
            yield return null;
        }
        yield return null;
    }

    [System.Serializable]
    public class Settings
    {
        public Rigidbody2D Rigidbody2D;
        public LineRenderer LineRenderer;
        public GameObject UfoRay;

        public Components.Engine.Settings EngineSettings;
        public Components.Radar.Settings RadarSettings;
        public Components.AlienRay.Settings AlienRaySettings;
        public Components.SignalOscillator.Settings SignalOscillator;
    }
}