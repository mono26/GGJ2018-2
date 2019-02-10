// Copyright (c) What a Box Creative Studio. All rights reserved.

using System.Collections.Generic;
using UnityEngine;

public static class InputManager
{
    private static bool forceMobileMode = true;
    private static bool IsOnMobile { get;  set; }

    private static Dictionary<string, InputAxis> activeAxiss = new Dictionary<string, InputAxis>();
    private static Dictionary<string, InputButton> activeButtons = new Dictionary<string, InputButton>();  

    static InputManager()
    {
        DetectTypeOfInput();
        return;
	}

    private static void DetectTypeOfInput()
    {
        if(LevelUIManager.Instance)
        {
            if(forceMobileMode)  
            {
                LevelUIManager.Instance.ActivatePlayerControls(true);
                IsOnMobile = true;
            }
            else
            {
#if UNITY_ANDROID || UNITY_IPHONE
                LevelUIManager.Instance.ActivatePlayerControls(true);
                IsOnMobile = true;
#endif
#if UNITY_EDITOR
                LevelUIManager.Instance.ActivatePlayerControls(false);
                IsOnMobile = false;
#endif
            }    
        }
        return;
    }

    private static float GetDesktopAxisValue(string _axisID)
    {
        float axisValue = 0;
        InputAxis axis = RetrieveAxisByID(_axisID);
        if(axis != null) {
            axisValue = Input.GetAxis(_axisID);
        }
        return axisValue;
    }

    private static InputAxis RetrieveAxisByID(string _axisID)
    {
        // TODO implement exception for no registered axis with id.
        InputAxis axisToReturn = null;
        InputAxis axis = null;
        if(activeAxiss.TryGetValue(_axisID, out axis)) {
            axisToReturn = axis;
        }
        return axisToReturn;
    }

    private static InputButtonStates GetDesktopButtonState(string _buttonID)
    {
        InputButton button = RetrieveButtonByID(_buttonID);
        InputButtonStates buttonState = InputButtonStates.Off;
        if(button != null)
        {
            if (Input.GetButtonDown(_buttonID)) {
                buttonState = InputButtonStates.Down;
            }
            if (Input.GetButton(_buttonID)) {
                buttonState = InputButtonStates.Pressed;
            }
            if (Input.GetButtonUp(_buttonID)) {
                buttonState = InputButtonStates.Up;
            }
        }
        return buttonState;
    }

    private static InputButton RetrieveButtonByID(string _buttonID)
    {
        // TODO implement exception for no registered button with id.
        InputButton buttonToReturn = null;
        InputButton button = null;
        if(activeButtons.TryGetValue(_buttonID, out button)) 
        {
            buttonToReturn = button;
        }
        return buttonToReturn;
    }

    private static float GetVirtualAxisValue(string _axisID)
    {
        InputAxis axis = RetrieveAxisByID(_axisID);
        return axis.CurrentValue;
    }

    private static InputButtonStates GetVirtualButtonState(string _buttonID)
    {
        InputButton button = RetrieveButtonByID(_buttonID);
        return button.CurrentState;
    }

    public static float GetAxisValue(string _axisID)
    {
        float axisValue = 0;
        if(IsOnMobile){
            axisValue = GetVirtualAxisValue(_axisID);
        }
        else{
            axisValue = GetDesktopAxisValue(_axisID);
        }
        return axisValue;
    }

    public static InputButtonStates GetButtonState(string _buttonID)
    {
        InputButtonStates buttonState = InputButtonStates.Off;
        if (IsOnMobile)
        {
            buttonState = GetVirtualButtonState(_buttonID);
        }
        else
        {
            buttonState = GetDesktopButtonState(_buttonID);
        }
        
        return buttonState;
    }

    public static void InteractWithButton(string _buttonID, InputButtonStates _newbuttonState) 
    {
        InputButton button = RetrieveButtonByID(_buttonID);
        if(button != null){
            button.ChangeButtonState(_newbuttonState);
        }
        return;
    }

    public static void InteractWithAxis(string _axisID, float _axisValue)
    {
        InputAxis axis = RetrieveAxisByID(_axisID);
        if(axis != null) {
            axis.ChangeAxisValue(_axisValue);
        }
        return;
    }

    public static void RegisterButton(string _buttonIDToRegister)
    {
        if(!activeButtons.ContainsKey(_buttonIDToRegister))
        {
            InputButton buttonToAdd = new InputButton(_buttonIDToRegister);
            activeButtons.Add(_buttonIDToRegister, buttonToAdd);
        }
        return;
    }

    public static void UnRegisterButton(string _buttonIDToUnRegister)
    {
        if(activeButtons.ContainsKey(_buttonIDToUnRegister)) {
            activeButtons.Remove(_buttonIDToUnRegister);
        }
        return;
    }

    
    public static void RegisterAxis(string _axisIDToRegister)
    {
        if(!activeAxiss.ContainsKey(_axisIDToRegister))
        {
            InputAxis buttonToAdd = new InputAxis(_axisIDToRegister);
            activeAxiss.Add(_axisIDToRegister, buttonToAdd);
        }
        return;
    }

    public static void UnRegisterAxis(string _axisIDToUnRegister)
    {
        if(activeAxiss.ContainsKey(_axisIDToUnRegister)) {
            activeButtons.Remove(_axisIDToUnRegister);
        }
        return;
    }
}

public enum InputButtonStates { Off, Down, Pressed, Up }
public class InputButton
{
    public string ButtonID { get; protected set; }
    public InputButtonStates CurrentState { get; protected set; }

    public InputButton(string _buttonID)
    {
        ButtonID = _buttonID;
        return;
    }
    public void ChangeButtonState(InputButtonStates _newState)
    {
        CurrentState = _newState;
        return;
    }
}

public class InputAxis
{
    public string AxisID { get; private set; }
    public float CurrentValue { get; private set; }

    public InputAxis(string _axisID)
    {
        AxisID = _axisID;
        return;
    }
    public void ChangeAxisValue(float _valueToSet)
    {
        CurrentValue = _valueToSet;
        return;
    }
}