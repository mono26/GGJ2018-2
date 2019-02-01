// Copyright (c) What a Box Creative Studio. All rights reserved.

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using System;

public enum AxisOptions{ BothAxis, XAxis, YAxis }

[System.Serializable]
public class JoystickEvent : UnityEvent<Vector2> { }

[RequireComponent(typeof(Rect))]
[RequireComponent(typeof(CanvasGroup))]
public class TouchJoystick : MonoBehaviour, IDragHandler, IEndDragHandler
{
    [Header("Touch Joystick settings")]
    [SerializeField] private float maxRange = 1.0f;
    [SerializeField] private string representedHorizontalAxisID;
    [SerializeField] private string representedVerticalAxisID;
    [SerializeField] private AxisOptions axisOptions;

    [Header("Touch Joystick components")]
    [SerializeField] protected Camera targetCamera;
    [SerializeField] protected RectTransform joystick;
    [SerializeField] protected RectTransform knob;

    [SerializeField] private RenderMode ParentCanvasRenderMode { get; set; }
    protected Vector2 joystickValue;

    public virtual void Initialize()
    {
        if (targetCamera == null) {
            throw new Exception("TouchJoystick : you have to set a target camera");
        }
        ParentCanvasRenderMode = GetComponentInParent<Canvas>().renderMode;
        return;
    }

    protected virtual void OnEnable()
    {
        Initialize();
        return;
    }

    protected virtual void Update()
    {
        if(axisOptions == AxisOptions.BothAxis || axisOptions == AxisOptions.XAxis){
            InputManager.InteractWithAxis(representedHorizontalAxisID, joystickValue.x);
        }
        if(axisOptions == AxisOptions.BothAxis || axisOptions == AxisOptions.YAxis){
            InputManager.InteractWithAxis(representedVerticalAxisID, joystickValue.y);
        }
        return;
    }

    public virtual void OnDrag(PointerEventData eventData)
    {
        Vector3 newTargetPosition = Vector3.zero;
        if (ParentCanvasRenderMode == RenderMode.ScreenSpaceCamera)
        {
            //newTargetPosition = targetCamera.ScreenToWorldPoint(eventData.position);
            newTargetPosition = targetCamera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, targetCamera.nearClipPlane));
        }
        else 
        {
            newTargetPosition = eventData.position;
        }

        newTargetPosition.z = joystick.position.z;

        if(axisOptions == AxisOptions.YAxis)
        {
            newTargetPosition.x = joystick.position.x;
        }
        if(axisOptions == AxisOptions.XAxis)
        {
            newTargetPosition.y = joystick.position.y;
        }

        newTargetPosition = Vector3.ClampMagnitude(newTargetPosition - joystick.position, maxRange);
        joystickValue.x = EvaluateInputValue(newTargetPosition.x);
        joystickValue.y = EvaluateInputValue(newTargetPosition.y);
        Vector3 newJoystickPosition = joystick.position + newTargetPosition;
        knob.position = newJoystickPosition;
        return;
    }

    public virtual void OnEndDrag(PointerEventData eventData)
    {
        Vector3 newJoystickPosition = joystick.position;
        newJoystickPosition.z = joystick.position.z;
        knob.position = newJoystickPosition;
        joystickValue.x = 0f;
        joystickValue.y = 0f;
        return;
    }

    protected virtual float EvaluateInputValue(float vectorPosition)
    {
        return Mathf.InverseLerp(0, 1, Mathf.Abs(vectorPosition)) * Mathf.Sign(vectorPosition);
    }
}

