using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Engine : ShipComponent, EventHandler<BlackHoleCenterReachEvent>
{
    [Header("Engine settings")]
    [SerializeField]
    protected float fuelLossPerSecond = 1.0f;
    [SerializeField]
    protected float maxFuel = 9999.0f;
    [SerializeField]
    protected float thrust = 1000f;

    [Header("Components ")]
    [SerializeField]
    protected ProgressBar fuelDisplay;

    [Header("Editor debugging")]
    [SerializeField]
    protected float currentFuel;
    public float CurrentFuel { get { return currentFuel; } }

    protected void Start()
    {
        currentFuel = maxFuel;
        fuelDisplay.UpdateBar(currentFuel, maxFuel);
        return;
    }

    protected void OnEnable()
    {
        EventManager.AddListener<BlackHoleCenterReachEvent>(this);
        return;
    }

    protected void OnDisable()
    {
        EventManager.RemoveListener<BlackHoleCenterReachEvent>(this);
        return;
    }

    public override void EveryFrame()
    {
        base.EveryFrame();

        fuelDisplay.UpdateBar(currentFuel, maxFuel);

        if (currentFuel <= 0)
        {
            LevelManager.Instance.EndGame();
        }

        return;
    }

    protected override void HandleInput()
    {
        // Input from the mouse VERTICAL
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            // Check if there is enough fuel for moving
            if (currentFuel > 0)
            {
                ApplyForce(transform.up);
                LoseFuel();
            }
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            // Check if there is enough fuel for moving
            if (currentFuel > 0)
            {
                ApplyForce(-transform.up);
                LoseFuel();
            }
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            if (currentFuel > 0)
            {
                ApplyForce(-transform.right);
                LoseFuel();
            }
        }
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            if (currentFuel > 0)
            {
                ApplyForce(transform.right);
                LoseFuel();
            }
        }

        return;
    }

    public void ApplyForce(Vector3 _direction)
    {
        ship.ShipBody.AddForce(_direction * thrust);
        return;
    }

    public void RechargeFuel(float _fuelToAdd)
    {
        currentFuel += _fuelToAdd;
        currentFuel = Mathf.Clamp(currentFuel, 0, maxFuel);
        return;
    }

    public void LoseFuel()
    {
        currentFuel -= fuelLossPerSecond * Time.deltaTime;
        return;
    }

    public void RecieveDamage(int daño)
    {
        currentFuel -= daño;
        return;
    }

    private void OnCollisionEnter2D(Collision2D _collision)
    {
        // Look if is a fuel planet or a signal planet
        if (_collision.gameObject.CompareTag("Planet"))
        {
            Planet planetComponent = _collision.gameObject.GetComponent<Planet>();
            if(planetComponent.Signal != null && planetComponent.Signal.Type == SignalEmitter.SignalType.FuelPlanet)
                RechargeFuel(maxFuel);
        }
        if (_collision.gameObject.CompareTag("Asteroide"))
        {
            RecieveDamage(25);
            Debug.Log("Recibi daño de asteroide");
            //TODO: definir bien el daño del asteroide
        }

        if (_collision.gameObject.CompareTag("Alien"))
        {
            // TODO eliminate magic number
            RechargeFuel(5);
        }
    }

    public void OnEvent(BlackHoleCenterReachEvent _blackHoleEvent)
    {
        if(_blackHoleEvent.affectedObject.Equals(ship.ShipBody) == true)
        {
            RecieveDamage(30);
        }

        return;
    }
}
