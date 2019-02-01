using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MovingJoystick : TouchJoystick, IPointerDownHandler, IPointerUpHandler
{
	[SerializeField] float activeAlpha, inactiveAlpha;
	[SerializeField] CanvasGroup touchArea;
	[SerializeField] bool canMoveToClickPosition;

	Vector2 initialJoystickPosition, initialKnobPosition;

	void Awake() 
	{
		if(touchArea == null)
		{
			touchArea = GetComponent<CanvasGroup>();
		}
	}
	void Start()
	{
		SetAlpha(0.3f);

		initialJoystickPosition = joystick.position;
		initialKnobPosition = knob.position;
	}

	public void OnPointerDown(PointerEventData pointerData)
	{
		if(!canMoveToClickPosition)
		{
			return;
		}

		SetAlpha(1);

		joystick.position = pointerData.position;
		//knob.anchoredPosition = pointerData.position;
	}

	void SetAlpha(float _targetAlpha)
	{
		float targetAlpha = Mathf.Clamp(_targetAlpha, 0, 1);
		touchArea.alpha = targetAlpha;
	}

	public void OnPointerUp(PointerEventData pointerData)
	{
		SetAlpha(0.3f);

		joystick.position = initialJoystickPosition;
		knob.position = initialKnobPosition;
	}
}
