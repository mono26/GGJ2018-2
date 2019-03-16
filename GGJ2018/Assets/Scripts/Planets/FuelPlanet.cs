using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelPlanet : Planet
{
	[SerializeField] int rechargeValue = 100;
	[SerializeField] float rechargeCooldown = 0;

	[SerializeField] float cooldownTimer;


    /*private void Start()
    {
        
    }
    */
    void Update() 
	{
		cooldownTimer -= Time.deltaTime;
		rechargeCooldown = Mathf.Clamp(cooldownTimer, 0, rechargeCooldown);
	}





    public void RechargeShip(ShipEngine _engine)
	{
		if (rechargeCooldown > 0)
		{
			return;
		}

		_engine.RechargeFuel(rechargeValue);
		cooldownTimer = rechargeCooldown;
	}
}
