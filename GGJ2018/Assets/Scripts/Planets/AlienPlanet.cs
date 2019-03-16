using System.Collections.Generic;
using UnityEngine;

public class AlienPlanet : Planet
{
    [Header("Alien Planet settings")]
    [SerializeField] int numberOfAliens;

    [SerializeField] List<Alien> aliens;
    [SerializeField] Transform aliensContainer;

    void OnEnable() 
    {
        signal.TurnSignal(SignalEmitter.SignalState.ON);
    }

    void Update()
    {
        if (aliens.Count == 0)
        {
            signal.TurnSignal(SignalEmitter.SignalState.OFF);
        }
    }

    void SpawnAliensInPlanet()
    {
        for (int i = 0; i < numberOfAliens; i++)
        {
            Alien alien = PoolsManager.Instance.GetObjectFromPool<Alien>();
            alien.transform.SetParent(aliensContainer);
            aliens.Add(alien);
        }
    }

    protected void LocateAliens()
    {
        foreach (Alien alien in aliens)
        {
            int angle = Random.Range(0, 360);
            float x = Mathf.Cos(angle) * (GetRadius + 0.3f);
            float y = Mathf.Sin(angle) * (GetRadius + 0.3f);
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

    public override void Release()
    {
        foreach (Alien alien in aliens)
        {
            if (alien == null)
            {
                continue;
            }

            alien.transform.SetParent(null);
        }

        aliens.Clear();
        PoolsManager.Instance.ReleaseObjectToPool(this);
    }

    public void SpawnAliens()
    {
        aliens = new List<Alien>();
        SpawnAliensInPlanet();
        LocateAliens();
        ChangeAliensDirectionTowardsPlanet();
    }
}
