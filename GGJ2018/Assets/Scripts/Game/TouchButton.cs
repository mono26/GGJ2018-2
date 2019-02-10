// Copyright (c) What a Box Creative Studio. All rights reserved.

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public enum TouchButtonStates { Off, ButtonDown, ButtonPressed, ButtonUp, Disabled }

public class TouchButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Touch Button settings")]
	[SerializeField] private string representedButtonID;
    [SerializeField] TouchButtonStates currentState;
    public TouchButtonStates CurrentState { get { return currentState; } }

	void Update ()
    {
        if(currentState == TouchButtonStates.ButtonPressed) 
        {
			ButtonPressed();
        }
    }

    protected virtual void LateUpdate()
    {
        if (currentState == TouchButtonStates.ButtonUp) 
        {
            currentState = TouchButtonStates.Off;      
        }
        if (currentState == TouchButtonStates.ButtonDown) 
        {
            currentState = TouchButtonStates.ButtonPressed;
        }
    }

    public virtual void ButtonPressed()
    {
        currentState = TouchButtonStates.ButtonPressed;
		InputManager.InteractWithButton(representedButtonID, InputButtonStates.Pressed);
    }

    public virtual void OnPointerDown(PointerEventData data)
    {
        if (currentState != TouchButtonStates.Off) 
		{
            return;
        }

        ButtonDownFirstTime();
    }

    void ButtonDownFirstTime()
    {
        currentState = TouchButtonStates.ButtonDown;
        InputManager.InteractWithButton(representedButtonID, InputButtonStates.Down);
    }

    public virtual void OnPointerUp(PointerEventData data)
    {
        if (currentState != TouchButtonStates.ButtonPressed && currentState != TouchButtonStates.ButtonDown) 
		{
            return;
        }

        ButtonUp();
    }

    void ButtonUp()
    {
        currentState = TouchButtonStates.ButtonUp;
        InputManager.InteractWithButton(representedButtonID, InputButtonStates.Up);
    }

    public virtual void DisableButton()
    {
        currentState = TouchButtonStates.Disabled;
    }

    public virtual void EnableButton()
    {
        if (CurrentState == TouchButtonStates.Disabled) 
        {
            currentState = TouchButtonStates.Off;
        }
    }
}
