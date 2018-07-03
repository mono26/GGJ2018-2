using System.Collections.Generic;
using UnityEngine;

public class AlienPlanet : Planet
{
    [Header("Alien Planet settings")]
    [SerializeField]
    private Alien alien;
    [SerializeField]
    private int numberOfAliens;

    [Header("Editor debugging")]
    [SerializeField]
    private List<Transform> aliensInPlanet;

    protected override void Awake()
    {
        base.Awake();

        aliensInPlanet = new List<Transform>();
        if(alien != null)
        {
            for (int i = 0; i < numberOfAliens; i++)
            {
                Alien tempAlien = Instantiate(alien, transform.position, transform.rotation, transform.Find("Aliens"));
                tempAlien.transform.SetParent(transform);
                aliensInPlanet.Add(tempAlien.transform);
            }
        }

        return;
    }

    protected override void Start()
    {
        base.Start();

        foreach (Transform alien in aliensInPlanet)
        {
            if (alien != null && alien.gameObject.activeInHierarchy)
            {
                LocateAliens(alien);
                ChangeUpDirectionTowardsPlanet(alien);
            }
        }
        return;
    }

    protected void OnEnable()
    {
        signal.TurnSignal(SignalEmitter.SignalState.ON);
        return;
    }

    private void LocateAliens(Transform _alien)
    {
        var angle = Random.Range(0f, 360f);
        var x = Mathf.Cos(angle) * planetRadius;
        var y = Mathf.Sin(angle) * planetRadius;
        _alien.position = transform.position + new Vector3(x, y);
        return;
    }

    private void ChangeUpDirectionTowardsPlanet(Transform _alien)
    {
        _alien.up = (_alien.position - transform.position).normalized;
        return;
    }

    protected override void OnTriggerEnter2D(Collider2D _collider)
    {
        base.OnTriggerEnter2D(_collider);

        if (_collider.CompareTag("Alien") == true)
        {
            if(aliensInPlanet.Contains(_collider.transform) == false)
            {
                aliensInPlanet.Add(_collider.transform);
            }
        }

        return;
    }

    protected override void OnTriggerExit2D(Collider2D _collider)
    {
        base.OnTriggerExit2D(_collider);

        if (_collider.CompareTag("Alien") == true)
        {
            if (aliensInPlanet.Contains(_collider.transform) == true)
            {
                aliensInPlanet.Remove(_collider.transform);
            }
        }

        return;
    }
}
