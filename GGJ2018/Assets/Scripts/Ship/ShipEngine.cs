// Copyright (c) What a Box Creative Studio. All rights reserved.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ShipEngine : ShipComponent, Damageable, EventHandler<BlackholeEvent>
{
    [Header("Engine settings")]
    [SerializeField] private float fuelLossPerSecond = 1.0f;
    [SerializeField] private float fuelLossPerBoost = 5.0f;
    [SerializeField] private float maxFuel = 9999.0f;
    [SerializeField] private float thrust = 1000f;
    [SerializeField] private float boost = 3000f;
    

    [Header("Components ")]
    [SerializeField] private ProgressBar fuelDisplay;

    [Header("Editor debugging")]
    [SerializeField] private float currentFuel;
    [SerializeField] private bool canBoost;
    [SerializeField] private float boostCooldown;
    [SerializeField] private float boostTimer;

    public float CurrentFuel { get { return currentFuel; } set { currentFuel = value; } }
    public float GetMaxFuel { get { return maxFuel; } }


    public delegate void GameEvent();
    public static event GameEvent PlayerCanBoost;


    private void Start()
    {      
        canBoost = true;
        //PlayerCanBoost();
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

    private void LoseFuelAmount(float amount)
    {
        currentFuel -= amount * Time.deltaTime;
        return;
    }

    public override void EveryFrame()
    {
        base.EveryFrame();
        fuelDisplay.UpdateBar(currentFuel, maxFuel);
        if (currentFuel <= 0) {
            LevelManager.Instance.EndGame();
        }
        if (canBoost == false)
        {
            boostTimer += 1 * Time.fixedDeltaTime;
            if(boostTimer >= boostCooldown)
            {
                PlayerCanBoost();
                canBoost = true;
                
                boostTimer = 0;
            }
        }
        return;
    }

    public void TakeDamage(float daño)
    {
        currentFuel -= daño;
        return;
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
            LoseFuelAmount(fuelLossPerSecond);
        }
    }

    public void ApplyBoost(Vector3 _direction)
    {
        if (currentFuel > 0 && canBoost)
        {
            ship.GetBodyComponent.AddForce(_direction * boost);
            LoseFuelAmount(fuelLossPerBoost);
           
            canBoost = false;
            PlayerCanBoost();
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
