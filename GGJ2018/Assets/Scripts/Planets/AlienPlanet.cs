using System.Collections.Generic;
using UnityEngine;

public class AlienPlanet : Planet
{
    [SerializeField]
    private int numberOfAliens;

    [SerializeField]
    private GameObject alien;

    [SerializeField]
    private List<Transform> aliens;

    public override void Awake()
    {
        base.Start();
        aliens = new List<Transform>();
        for (int alien = 0; alien < numberOfAliens; alien++)
        {
            var tempAlien = Instantiate(this.alien, transform.position, transform.rotation, transform.Find("Aliens"));
            aliens.Add(tempAlien.transform);
        }
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
    }

    // Update is called once per frame
    public void Update ()
    {
        for (int alien = 0; alien < aliens.Count; alien++)
        {
            if (aliens[alien] != null && aliens[alien].gameObject.activeInHierarchy)
            {
                aliens[alien].RotateAround(transform.position, transform.forward, GravitationalFieldStrenght * Time.deltaTime);
                ChangeUpDirectionTowardsPlanet(aliens[alien]);
            }
            else
            {
                aliens[alien] = null;
            }
        }
        return;
    }

    private void LocateAliens(Transform _alien)
    {
        var angle = Random.Range(0f, 360f);
        var x = Mathf.Cos(angle) * PlanetRadius;
        var y = Mathf.Sin(angle) * PlanetRadius;
        _alien.position = transform.position + new Vector3(x, y);
    }

    private void ChangeUpDirectionTowardsPlanet(Transform _alien)
    {
        _alien.up = (_alien.position - transform.position).normalized;
    }

    public void RemoveAlien(Transform _alien)
    {
        aliens.Remove(_alien);
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        base.OnCollisionEnter2D(collision);
    }

}
