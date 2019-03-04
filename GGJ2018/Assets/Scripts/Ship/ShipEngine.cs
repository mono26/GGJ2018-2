// Copyright (c) What a Box Creative Studio. All rights reserved.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ShipEngine : ShipComponent, Damageable, EventHandler<BlackholeEvent>
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
        EventManager.AddListener<BlackholeEvent>(this);
        return;
    }

    private void OnDisable()
    {
        EventManager.RemoveListener<BlackholeEvent>(this);
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
            LevelManager.Instance.EndGame();
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
        if (_collision.gameObject.CompareTag("FuelPlanet"))
        {
            FuelPlanet planetComponent = _collision.gameObject.GetComponent<FuelPlanet>();
            if(planetComponent == null)
            {
                planetComponent = _collision.gameObject.GetComponentInParent<FuelPlanet>();
            }
            if(planetComponent != null)
            {
                planetComponent.RechargeShip(this);
            }
        }
        if (_collision.gameObject.CompareTag("Alien"))
        {
            // TODO eliminate magic number
            // RechargeFuel(5);
        }
    }

    public void RechargeFuel(float _fuelToAdd)
    {
        currentFuel += _fuelToAdd;
        currentFuel = Mathf.Clamp(currentFuel, 0, maxFuel);
        return;
    }

    private bool AreWeInABlackholeCenter(BlackholeEvent _blackHoleEvent)
    {
        bool centerReachead = false;
        if(_blackHoleEvent.GetEventType == BlackholeEventType.CenterReachead && _blackHoleEvent.GetAffectedObject.Equals(gameObject)){
            centerReachead = true;
        }
        return centerReachead;
    }

    public void ApplyEngineThrust(Vector3 _direction)
    {
        if(currentFuel > 0)
        {
            ship.GetBodyComponent.AddForce(_direction * thrust);
            LoseFuel();
        }
    }

    public void OnGameEvent(BlackholeEvent _blackHoleEvent)
    {
        if(AreWeInABlackholeCenter(_blackHoleEvent)) {
            TakeDamage(30);
        }
        return;
    }
}
