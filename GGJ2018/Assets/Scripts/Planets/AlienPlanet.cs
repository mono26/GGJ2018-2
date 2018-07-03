using System.Collections.Generic;
using UnityEngine;

public class AlienPlanet : Planet
{
    [Header("Alien Planet settings")]
    [SerializeField]
    private GameObject alien;
    [SerializeField]
    private int numberOfAliens;

    [Header("Editor debugging")]
    [SerializeField]
    private List<Transform> aliens;

    public override void Awake()
    {
        base.Awake();

        aliens = new List<Transform>();
        for (int alien = 0; alien < numberOfAliens; alien++)
        {
            var tempAlien = Instantiate(this.alien, transform.position, transform.rotation, transform.Find("Aliens"));
            aliens.Add(tempAlien.transform);
        }
        return;
    }
    // Use this for initialization
    public override void Start()
    {
        base.Start();

        foreach (Transform alien in aliens)
        {
            if (alien != null && alien.gameObject.activeInHierarchy)
            {
                LocateAliens(alien);
                ChangeUpDirectionTowardsPlanet(alien);
            }
        }
        return;
    }

    public void OnEnable()
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

    public void RemoveAlien(Transform _alien)
    {
        aliens.Remove(_alien);
        return;
    }

    protected override void OnTriggerEnter2D(Collider2D _collider)
    {
        base.OnTriggerEnter2D(_collider);

        if (_collider.CompareTag("Alien") == true)
        {
            if(aliens.Contains(_collider.transform) == false)
            {
                aliens.Add(_collider.transform);
            }
        }

        return;
    }

    protected override void OnTriggerExit2D(Collider2D _collider)
    {
        base.OnTriggerExit2D(_collider);

        if (_collider.CompareTag("Alien") == true)
        {
            if (aliens.Contains(_collider.transform) == true)
            {
                aliens.Remove(_collider.transform);
            }
        }

        return;
    }
}
