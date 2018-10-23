// Copyright (c) What a Box Creative Studio. All rights reserved.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Engine : ShipComponent, Damageable, EventHandler<BlackHoleEvent>
{
    [Header("Engine settings")]
    [SerializeField] private float fuelLossPerSecond = 1.0f;
    [SerializeField] private float maxFuel = 9999.0f;
    [SerializeField] private float thrust = 1000f;

    [Header("Components ")]
    [SerializeField] private ProgressBar fuelDisplay;

    [Header("Editor debugging")]
    [SerializeField] private float currentFuel;

    public float GetCurrentFuel { get { return currentFuel; } }

    private void Start()
    {
        currentFuel = maxFuel;
        fuelDisplay.UpdateBar(currentFuel, maxFuel);
        return;
    }

    private void OnEnable()
    {
        EventManager.AddListener<BlackHoleEvent>(this);
        return;
    }

    private void OnDisable()
    {
        EventManager.RemoveListener<BlackHoleEvent>(this);
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

    private void ApplyForce(Vector3 _direction)
    {
        ship.ShipBody.AddForce(_direction * thrust);
        return;
    }

    private void LoseFuel()
    {
        currentFuel -= fuelLossPerSecond * Time.deltaTime;
        return;
    }

    public override void EveryFrame()
    {
        base.EveryFrame();
        fuelDisplay.UpdateBar(currentFuel, maxFuel);
        if (currentFuel <= 0) {
            LevelUIManager.Instance.ActivateGameOverUI(true);
        }
        return;
    }

    public void TakeDamage(float daño)
    {
        currentFuel -= daño;
        return;
    }

    private void OnCollisionEnter2D(Collision2D _collision)
    {
        if (_collision.gameObject.CompareTag("Planet"))
        {
            Planet planetComponent = _collision.gameObject.GetComponent<Planet>();
            if(planetComponent.Signal != null && planetComponent.Signal.Type == SignalEmitter.SignalType.FuelPlanet){
                RechargeFuel(maxFuel);
            }
        }
        if (_collision.gameObject.CompareTag("Alien"))
        {
            // TODO eliminate magic number
            RechargeFuel(5);
        }
        return;
    }

    private bool AreWeInABlackHoleCenter(BlackHoleEvent _blackHoleEvent)
    {
        bool centerReachead = false;
        if(_blackHoleEvent.GetEventType == BlackHoleEventType.CenterReachead && _blackHoleEvent.GetAffectedObject.Equals(gameObject)){
            centerReachead = true;
        }
        return centerReachead;
    }

    public void OnGameEvent(BlackHoleEvent _blackHoleEvent)
    {
        if(AreWeInABlackHoleCenter(_blackHoleEvent)) {
            TakeDamage(30);
        }
        return;
    }

    public void RechargeFuel(float _fuelToAdd)
    {
        currentFuel += _fuelToAdd;
        currentFuel = Mathf.Clamp(currentFuel, 0, maxFuel);
        return;
    }
}
