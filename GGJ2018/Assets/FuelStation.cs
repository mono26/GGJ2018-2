using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelStation : MonoBehaviour {
    [SerializeField] int rechargeValue = 100;
    [SerializeField] float rechargeCooldown = 5;
    [SerializeField] private ParticleSystem reloadingParticles;
    //[SerializeField] private bool canReload;

    Coroutine refillRoutine;

    protected void Awake()
    {
        //canReload = true;
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" )
        {
            ShipEngine engine = collision.gameObject.GetComponent<ShipEngine>();
            refillRoutine = StartCoroutine(ReloadFuel(engine));
            reloadingParticles.Play();
        }
    }
    protected void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            reloadingParticles.Stop();
            StopCoroutine(refillRoutine);
            
        }
    }

    private IEnumerator ReloadFuel(ShipEngine _engine)
    {
        if(_engine.CurrentFuel < _engine.GetMaxFuel)
        {
            _engine.RechargeFuel(rechargeValue);
            Debug.Log("Reloading " + rechargeValue);

            yield return new WaitForSeconds(rechargeCooldown);

            refillRoutine = StartCoroutine(ReloadFuel(_engine));
        }
        
    }
}
