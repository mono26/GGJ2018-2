using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipComponent : MonoBehaviour
{
    [Header("Ship Component settings")]
    [SerializeField]
    protected Ship ship;

    protected virtual void Awake()
    {
        if(ship == null)
            ship = GetComponent<Ship>();

        return;
    }

    public virtual void EveryFrame()
    {
        HandleInput();
    }

    public virtual void FixedFrame()
    {

    }

    public virtual void LateFrame()
    {

    }

    protected virtual void HandleInput()
    {

    }
}
