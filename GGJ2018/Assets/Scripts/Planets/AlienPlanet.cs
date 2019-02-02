using System.Collections.Generic;
using UnityEngine;

public class AlienPlanet : Planet
{
    [Header("Alien Planet settings")]
    [SerializeField]
    private Alien alien;
    [SerializeField]
    private int numberOfAliens;

    protected override void Awake()
    {
        base.Awake();

        Transform[] aliensInPlanet = new Transform[numberOfAliens];
        SpawnAliensInPlanet(ref aliensInPlanet);
        LocateAliens(aliensInPlanet);
        ChangeAliensDirectionTowardsPlanet(aliensInPlanet);
    }

    void SpawnAliensInPlanet(ref Transform[] aliens)
    {
        if(alien != null)
        {
            for (int i = 0; i < numberOfAliens; i++)
            {
                Alien tempAlien = Instantiate(alien, transform.position, transform.rotation, transform.Find("Aliens"));
                tempAlien.transform.SetParent(transform);
                aliens[i] = tempAlien.transform;
            }
        }
    }

    protected void LocateAliens(Transform[] _aliens)
    {
        foreach (Transform alien in _aliens)
        {
            int angle = Random.Range(0, 365);
            float x = Mathf.Cos(angle) * (planetRadius + 0.3f);
            float y = Mathf.Sin(angle) * (planetRadius + 0.3f);
            alien.position = transform.position + new Vector3(x, y);
        }
    }

    private void ChangeAliensDirectionTowardsPlanet(Transform[] _aliens)
    {
        foreach (Transform alien in _aliens)
        {
            alien.up = (alien.position - transform.position).normalized;
        }
    }

    protected void OnEnable()
    {
        signal.TurnSignal(SignalEmitter.SignalState.ON);
        return;
    }
}
