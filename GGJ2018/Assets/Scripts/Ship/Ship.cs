﻿using System.Collections;
using UnityEngine;

[System.Serializable]
public class ShipInput
{
    [SerializeField] private float horizontal, vertical, radarFrequency;
    [SerializeField] private bool rayState, radarState;

    public float GetHorizontal { get { return horizontal; } }
    public float GetVertical { get { return vertical; } }
    public float GetRadarFrequency { get { return radarFrequency; } }
    public bool GetRayState { get { return rayState; } }
    public bool GetRadarState { get { return radarState; } }

    public ShipInput(){}
    public ShipInput(float _horizontal, float _vertical, float _radarFrequency, bool _ray, bool _radar)
    {
        horizontal = _horizontal;
        vertical = _vertical;
        radarFrequency = _radarFrequency;
        rayState = _ray;
        radarState = _radar;
        return;
    }
}

public class Ship : MonoBehaviour, IInfluencedByGravity
{
    [Header("Ship components")]
    [SerializeField] private BoxCollider2D hitBoxComponent = null;
    [SerializeField] private SpriteRenderer spriteComponent = null;
    [SerializeField] private Rigidbody2D bodyComponent = null;
    [SerializeField] private ShipEngine engineComponent = null;
    [SerializeField] private Radar radarComponent = null;
    [SerializeField] private AtractorRay atractorRayComponent = null;

    [Header("Ship editor debugging.")]
    [SerializeField] private ShipComponent[] components;
    [SerializeField] private ShipInput currentInput = new ShipInput();    // TODO save input in exterinput component

    bool isOnPlanetGravitationalField;

    public Rigidbody2D GetBodyComponent { get { return bodyComponent; } }

	private void Awake()
    {
        if (hitBoxComponent == null) 
        {
            hitBoxComponent = GetComponent<BoxCollider2D>();
        }
        if (spriteComponent == null) 
        {
            spriteComponent = GetComponent<SpriteRenderer>();
            if(spriteComponent == null) 
            {
                spriteComponent = GetComponentInChildren<SpriteRenderer>();
            }
        }
        if (bodyComponent == null) 
        {
            bodyComponent = GetComponent<Rigidbody2D>();
        }
        if (engineComponent == null) 
        {
            engineComponent = GetComponent<ShipEngine>();
        }
        if (radarComponent == null) 
        {
            radarComponent = GetComponent<Radar>();
        }
        if (atractorRayComponent == null) 
        {
            atractorRayComponent = GetComponent<AtractorRay>();
        }

        components = GetComponents<ShipComponent>();
    }

    private void Update ()
    {
        ActivateShipRay();
        ActivateShipRadar();
        HandleShipRadarFrequency();

        foreach (ShipComponent component in components){
            component.EveryFrame();
        }
    }

    private void ActivateShipRay()
    {
        if(atractorRayComponent != null && currentInput.GetRayState) 
        {
            atractorRayComponent.ActivateRay();
        }
    }

    private void ActivateShipRadar()
    {
        if(atractorRayComponent != null && currentInput.GetRadarState) 
        {
            radarComponent.ActivateRadar();
        }
    }

    private void HandleShipRadarFrequency()
    {
        if(radarComponent != null) 
        {
            radarComponent.ChangeFrecuency((int)currentInput.GetRadarFrequency);
        }
        return;
    }

    private void FixedUpdate()
    {
        DriveShip();
    }

    private void DriveShip()
    {
        if(engineComponent != null)
        {
            engineComponent.ApplyEngineThrust(transform.right * currentInput.GetHorizontal);
            engineComponent.ApplyEngineThrust(transform.up * currentInput.GetVertical);
        }
    }

    void OnTriggerEnter2D(Collider2D _collider)
    {
        if (!_collider.CompareTag("GravitationField"))
        {
            return;
        }

        var parentPlanet = _collider.transform.parent;
        if(parentPlanet == null)
        {
            return;
        }

        if(parentPlanet.CompareTag("Planet"))
        {
            isOnPlanetGravitationalField = true;
        }
    }

    void OnTriggerExit2D(Collider2D _collider)
    {
        if (!_collider.CompareTag("GravitationField"))
        {
            return;
        }

        var parentPlanet = _collider.transform.parent;
        if(parentPlanet == null)
        {
            return;
        }

        if(parentPlanet.CompareTag("Planet"))
        {
            isOnPlanetGravitationalField = false;
        }
    }

    public void ApplyGravity(Vector2 _direction, float _force)
    {
        if(isOnPlanetGravitationalField)
        {
            return;
        }

        // In case the user forgets to normalize the direction vector.
        Vector3 normalizedDirection = _direction.normalized;
        bodyComponent.AddForce(_direction * _force, ForceMode2D.Force);
    }

    public void ApplyRotationalForce(Vector2 _direction, float _force)
    {
        // In case the user forgets to normalize the direction vector.
        Vector3 normalizedDirection = _direction.normalized;
        bodyComponent.AddForce(_direction * _force, ForceMode2D.Force);
    }


    public void RotateTowardsGravitationCenter(Vector2 _gravitationCenterDirection)
    {
        transform.up = _gravitationCenterDirection;
    }

    public void ReceiveInput(ShipInput _inputToRecieve)
    {
        currentInput = _inputToRecieve;
    }
}
