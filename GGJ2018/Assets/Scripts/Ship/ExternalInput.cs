// Copyright (c) What a Box Creative Studio. All rights reserved.

using UnityEngine;
//using UnityStandardAssets.CrossPlatformInput;

public class ExternalInput : ShipComponent
{
    [Header("External Input settings")]
    [SerializeField] private string playerID;
    [SerializeField] private string horizontalID, verticalID, radarfrequencyID, rayID, radarID;

	[Header("External Input editor debugging")]
    [SerializeField] private float horizontal;
    [SerializeField] private float vertical, radarFrequency;
    [SerializeField] bool rayState = false;
    [SerializeField] bool radarState = false;

    public float GetHorizontal { get { return horizontal; } }
    public float GetVertical { get { return vertical; } }
    public float GetFrequency { get { return radarFrequency; } }
    public bool GetRayState { get { return rayState; } }
    public bool GetRadarState { get { return radarState; } }

    private void OnEnable() 
    {
        RegisterShipInput();
        return;
    }

        private void RegisterShipInput()
    {
        InputManager.RegisterAxis(verticalID);
        InputManager.RegisterAxis(horizontalID);
        InputManager.RegisterAxis(radarfrequencyID);
        InputManager.RegisterButton(rayID);
        InputManager.RegisterButton(radarID);
        return;
    }

    private void OnDisable()
    {
        UnRegisterShipInput();
        return;
    }

    private void UnRegisterShipInput()
    {
        InputManager.UnRegisterAxis(verticalID);
        InputManager.UnRegisterAxis(horizontalID);
        InputManager.UnRegisterAxis(radarfrequencyID);
        InputManager.UnRegisterButton(rayID);
        InputManager.UnRegisterButton(radarID);
        return;
    }

    public override void EveryFrame()
    {
        GetTheInput();
        ship.ReceiveInput(new ShipInput(horizontal, vertical, radarFrequency, rayState, radarState));
        return;
    }

    protected virtual void GetTheInput()
    {
        horizontal = InputManager.GetAxisValue(horizontalID);
        vertical = InputManager.GetAxisValue(verticalID);
        // TODO check if its better to dot with a button.
        radarFrequency = InputManager.GetAxisValue(radarfrequencyID);
        radarState = (InputManager.GetButtonState(radarID) == InputButtonStates.Down) ? true : false;
        rayState = (InputManager.GetButtonState(rayID) == InputButtonStates.Down) ? true : false;
        return;
    }
}

