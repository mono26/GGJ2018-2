using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SignalEmitter : MonoBehaviour
{
    public enum SignalType { AlienPlanet, FuelPlanet}
    public enum SignalState { ON, OFF }

    [Header("Editor debugging")]
    [SerializeField]
    protected SignalState state = SignalState.OFF;
    [SerializeField]
    protected SignalType type;
    public SignalType Type { get { return type; } }

    public void TurnSignal(SignalState _state)
    {
        state = _state;
        return;
    }
}
