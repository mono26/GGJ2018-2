using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    // Assigned by editor or taken by the radius of the collider
    [SerializeField]
    float planetRadius;
    float gravitationalFieldRadius;
    [SerializeField]
    float gravitationalFieldStrenght;
    [SerializeField]
    private CircleCollider2D gravitationalField;

    public float PlanetRadius { get { return planetRadius; } }
    public float GravitationalFieldRadius { get { return gravitationalFieldRadius; } }
    public float GravitationalFieldStrenght { get { return gravitationalFieldStrenght; } }
    public CircleCollider2D GravitationalField{ get { return gravitationalField; } }

    // Use this for initialization
    public virtual void Start()
    {
        gravitationalFieldRadius = planetRadius * 2;
        gravitationalField.radius = gravitationalFieldRadius;
	}
}
