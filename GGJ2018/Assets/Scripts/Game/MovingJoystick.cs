using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MovingJoystick : TouchJoystick, IPointerDownHandler, IPointerUpHandler
{
	[SerializeField] RectTransform touchArea;
	[SerializeField] bool canMoveToClickPosition;

	Vector2 initialJoystickPosition, initialKnobPosition;

	// Use this for initialization
	void Start()
	{
		initialJoystickPosition = joystick.position;
		initialKnobPosition = knob.position;
	}

	public void OnPointerDown(PointerEventData pointerData)
	{
		if(!canMoveToClickPosition)
		{
			return;
		}

		Debug.LogError(pointerData.position);
		joystick.position = pointerData.position;
		//knob.anchoredPosition = pointerData.position;
	}

	public void OnPointerUp(PointerEventData pointerData)
	{
		joystick.position = initialJoystickPosition;
		knob.position = initialKnobPosition;
	}
}
