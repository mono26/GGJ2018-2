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
    public TouchButtonStates CurrentState { get; protected set; }

    [SerializeField] private bool canPress = true;

	void Update ()
    {
        if(CurrentState == TouchButtonStates.ButtonPressed && canPress) {
			OnPointerPressed();
		}
		return;
    }

    protected virtual void LateUpdate()
    {
        if (CurrentState == TouchButtonStates.ButtonUp) {
            CurrentState = TouchButtonStates.Off;
            
        }
        if (CurrentState == TouchButtonStates.ButtonDown) {
            CurrentState = TouchButtonStates.ButtonPressed;
        }
		return;
    }

    public virtual void OnPointerPressed()
    {
        canPress = false;
        CurrentState = TouchButtonStates.ButtonPressed;
		InputManager.InteractWithButton(representedButtonID, InputButtonStates.Pressed);
		return;
    }

    public virtual void OnPointerDown(PointerEventData data)
    {
        if (CurrentState == TouchButtonStates.Off) 
		{
            canPress = false;
        	CurrentState = TouchButtonStates.ButtonDown;
			InputManager.InteractWithButton(representedButtonID, InputButtonStates.Down);
        }
		return;
    }

    public virtual void OnPointerUp(PointerEventData data)
    {
        if (CurrentState == TouchButtonStates.ButtonPressed || CurrentState == TouchButtonStates.ButtonDown) 
		{
            canPress = true;
        	CurrentState = TouchButtonStates.ButtonUp;
			InputManager.InteractWithButton(representedButtonID, InputButtonStates.Up);
        }
		return;
    }

    public virtual void DisableButton()
    {
        CurrentState = TouchButtonStates.Disabled;
		return;
    }

    public virtual void EnableButton()
    {
        if (CurrentState == TouchButtonStates.Disabled) {
            CurrentState = TouchButtonStates.Off;
        }
		return;
    }
}
