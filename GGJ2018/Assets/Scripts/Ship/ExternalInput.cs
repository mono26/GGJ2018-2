// Copyright (c) What a Box Creative Studio. All rights reserved.

using UnityEngine;
//using UnityStandardAssets.CrossPlatformInput;

public class ExternalInput : ShipComponent
{
    [Header("External Input settings")]
    [SerializeField] private string playerID;
    [SerializeField] private string horizontalID, verticalID, radarfrequencyID, rayID, radarID, shieldID;

	[Header("External Input editor debugging")]
    [SerializeField] private float horizontalInput, verticalInput, radarFrequencyInput;
    [SerializeField] bool rayInput = false;
    [SerializeField] bool radarInput = false;
    [SerializeField] bool shieldInput = false;

    private void OnEnable() 
    {
        RegisterShipInput();
    }

    void RegisterShipInput()
    {
        InputManager.RegisterAxis(verticalID);
        InputManager.RegisterAxis(horizontalID);
        InputManager.RegisterAxis(radarfrequencyID);
        InputManager.RegisterButton(rayID);
        InputManager.RegisterButton(radarID);
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
        InputManager.UnRegisterAxis(radarfrequencyID);
        InputManager.UnRegisterButton(rayID);
        InputManager.UnRegisterButton(radarID);
        InputManager.UnRegisterButton(shieldID);
    }

    public override void EveryFrame()
    {
        GetTheInput();
        ship.ReceiveInput(new ShipInput(horizontalInput, verticalInput, radarFrequencyInput, rayInput, radarInput, shieldInput));
    }

    protected virtual void GetTheInput()
    {
        horizontalInput = InputManager.GetAxisValue(horizontalID);
        verticalInput = InputManager.GetAxisValue(verticalID);
        // TODO check if its better to dot with a button.
        radarFrequencyInput = InputManager.GetAxisValue(radarfrequencyID);
        radarInput = InputManager.GetButtonState(radarID).Equals(InputButtonStates.Down) ? true : false;
        rayInput = InputManager.GetButtonState(rayID).Equals(InputButtonStates.Down) ? true : false;
        shieldInput = InputManager.GetButtonState(shieldID).Equals(InputButtonStates.Down) ? true : false;
    }
}

