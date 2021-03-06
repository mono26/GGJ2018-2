﻿// Copyright (c) What a Box Creative Studio. All rights reserved.

using UnityEngine;

public class ExternalInput : ShipComponent
{
    [Header("External Input settings")]
    [SerializeField] string playerID;
    [SerializeField] string horizontalID = "";
    [SerializeField] string verticalID = "";
    [SerializeField] string rayID = "";
    [SerializeField] string boostID = "";
    [SerializeField] string shieldID = "";

	[Header("External Input editor debugging")]
    [SerializeField] float horizontalInput; 
    [SerializeField] float verticalInput;
    [SerializeField] bool rayInput = false;
    [SerializeField] bool boostInput = false;
    [SerializeField] bool shieldInput = false;

    private void OnEnable() 
    {
        RegisterShipInput();
    }

    void RegisterShipInput()
    {
        InputManager.RegisterAxis(verticalID);
        InputManager.RegisterAxis(horizontalID);
        InputManager.RegisterButton(rayID);
        InputManager.RegisterButton(boostID);
        InputManager.RegisterButton(shieldID);
    }

    private void OnDisable()
    {
        UnRegisterShipInput();
    }

    void UnRegisterShipInput()
    {
        InputManager.UnRegisterAxis(verticalID);
        InputManager.UnRegisterAxis(horizontalID);
        InputManager.UnRegisterButton(rayID);
        InputManager.UnRegisterButton(boostID);
        InputManager.UnRegisterButton(shieldID);
    }

    public override void EveryFrame()
    {
        GetTheInput();
        ship.ReceiveInput(new ShipInput(horizontalInput, verticalInput, rayInput, boostInput, shieldInput));
    }

    protected virtual void GetTheInput()
    {
        horizontalInput = InputManager.GetAxisValue(horizontalID);
        verticalInput = InputManager.GetAxisValue(verticalID);
        // TODO check if its better to dot with a button.
        boostInput = InputManager.GetButtonState(boostID).Equals(InputButtonStates.Down) ? true : false;
        rayInput = InputManager.GetButtonState(rayID).Equals(InputButtonStates.Down) ? true : false;
        shieldInput = InputManager.GetButtonState(shieldID).Equals(InputButtonStates.Down) ? true : false;
    }
}

