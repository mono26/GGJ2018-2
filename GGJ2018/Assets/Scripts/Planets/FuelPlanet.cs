using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelPlanet : Planet
{
	[SerializeField] int rechargeValue = 100;
	[SerializeField] float rechargeCooldown = 0;
	[SerializeField] GameObject refillEffect;

	Coroutine refillRoutine;

	protected override void OnTriggerEnter2D(Collider2D collision)
    {
		base.OnTriggerEnter2D(collision);

        if (collision.gameObject.tag == "Player")
        {
			ShipEngine engine = collision.gameObject.GetComponent<ShipEngine>();
            refillRoutine = StartCoroutine(ReloadFuel(engine));
        }
    }
    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StopCoroutine(refillRoutine);
        }
    }

	private IEnumerator ReloadFuel(ShipEngine _engine)
    {
        _engine.RechargeFuel(rechargeValue);

		if (refillEffect != null)
		{
			Instantiate(refillEffect, _engine.transform.position, _engine.transform.rotation);
		}

        yield return new WaitForSeconds(rechargeCooldown);

		refillRoutine = StartCoroutine(ReloadFuel(_engine));
    }
}
