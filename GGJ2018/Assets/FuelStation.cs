using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuelStation : MonoBehaviour {

    [SerializeField] private float reloadTime;
    [SerializeField] private float amountPerSecond;


    [SerializeField] private ShipEngine engine;


    // Use this for initialization
    void Awake () {
        engine = FindObjectOfType<ShipEngine>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(ReloadFuel());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            StartCoroutine(ReloadFuel());
        }
    }

    private IEnumerator ReloadFuel()
    {
        engine.CurrentFuel += amountPerSecond;
        yield return new WaitForSeconds(reloadTime);
    }
}
