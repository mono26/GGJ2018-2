using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalEmitter : MonoBehaviour
{
    public enum SignalType { AlienPlanet, FuelPlanet}
    public enum SignalState { ON, OFF }

    [Header("Editor debugging")]
    [SerializeField] SignalState state = SignalState.OFF;
    [SerializeField] SignalType type;
    public SignalType Type { get { return type; } }
    public SignalState GetState { get {return state; } }

    public void TurnSignal(SignalState _state)
    {
        state = _state;
    }
}
