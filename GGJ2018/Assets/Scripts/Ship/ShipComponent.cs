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
        if(ship == null){
            ship = GetComponent<Ship>();
        }
        return;
    }

    public virtual void EveryFrame()
    {

    }

    public virtual void FixedFrame()
    {

    }

    public virtual void LateFrame()
    {

    }
}
