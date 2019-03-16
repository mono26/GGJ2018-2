using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipGravity : Gravity
{
    public override void ApplyGravity(Planet _planet)
    {
        if(_planet.GetGravitySource == GravitySourceType.Planet)
        {
            return;
        }

        base.ApplyGravity(_planet);
    }
}
