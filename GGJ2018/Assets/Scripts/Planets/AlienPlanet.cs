using System.Collections.Generic;
using UnityEngine;

public class AlienPlanet : Planet
{
    [Header("Alien Planet settings")]
    [SerializeField] Alien alien;
    [SerializeField] int numberOfAliens;

    [SerializeField] List<Alien> aliens;

    protected override void Awake()
    {
        base.Awake();

        aliens = new List<Alien>();
        SpawnAliensInPlanet();
        LocateAliens();
        ChangeAliensDirectionTowardsPlanet();
    }

    void SpawnAliensInPlanet()
    {
        if(alien != null)
        {
            for (int i = 0; i < numberOfAliens; i++)
            {
                Alien alien = Instantiate(this.alien, transform.position, transform.rotation, transform.Find("Aliens"));
                alien.transform.SetParent(transform);
                aliens.Add(alien);
            }
        }
    }

    protected void LocateAliens()
    {
        foreach (Alien alien in aliens)
        {
            int angle = Random.Range(0, 365);
            float x = Mathf.Cos(angle) * (planetRadius + 0.3f);
            float y = Mathf.Sin(angle) * (planetRadius + 0.3f);
            alien.transform.position = transform.position + new Vector3(x, y);
        }
    }

    private void ChangeAliensDirectionTowardsPlanet()
    {
        foreach (Alien alien in aliens)
        {
            alien.transform.up = (alien.transform.position - transform.position).normalized;
        }
    }

    protected void OnEnable()
    {
        signal.TurnSignal(SignalEmitter.SignalState.ON);
        return;
    }

    void StopAliensMovement()
    {
        for(int i = aliens.Count - 1; i > -1; i--)
        {
            if(aliens[i] == null)
            {
                aliens.RemoveAt(i);
                continue;
            }

            aliens[i].GetBodyComponent.velocity = Vector3.zero;
        }
    }

    protected override void OnTriggerEnter2D(Collider2D _collider)
    {
        base.OnTriggerEnter2D(_collider);

        if(_collider.CompareTag("Alien"))
        {
            IAffectedByGravity obj = _collider.GetComponent<IAffectedByGravity>();
            if (obj != null && !objsInGravitationField.Contains(obj))
            {
                objsInGravitationField.Add(obj);        
            }
        }
    }

    protected override void OnTriggerExit2D(Collider2D _collider)
    {
        base.OnTriggerExit2D(_collider);

        if(_collider.CompareTag("Alien"))
        {
            IAffectedByGravity obj = _collider.GetComponent<IAffectedByGravity>();
            if (obj != null && objsInGravitationField.Contains(obj) == true)
            {
                objsInGravitationField.Remove(obj);
            }
        }

        if(_collider.CompareTag("Player"))
        {
            StopAliensMovement();
        }
    }
}
