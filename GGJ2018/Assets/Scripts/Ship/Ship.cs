using System.Collections;
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

public class Ship : MonoBehaviour, IAffectedByGravity
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

    void Start() 
    {
        LevelUIManager.Instance.DisplayInputButton("RadarButton", true);
        LevelUIManager.Instance.DisplayInputButton("RayButton", false);
    }

    private void Update ()
    {
        foreach (ShipComponent component in components)
        {
            component.EveryFrame();
        }

        if (currentInput.GetRadarState)
        {
            ActivateShipRadar();
        }

        if (currentInput.GetRayState)
        {
            ActivateShipRay();
        }

        HandleShipRadarFrequency();
    }

    private void ActivateShipRay()
    {
        atractorRayComponent.ActivateRay();
    }

    private void ActivateShipRadar()
    {
        radarComponent.ActivateRadar();
    }

    private void HandleShipRadarFrequency()
    {
        if(radarComponent == null) 
        {
            return;
        }

        radarComponent.ChangeFrecuency((int)currentInput.GetRadarFrequency);
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

    void OnTriggerStay2D(Collider2D _collider)
    {
        if (!_collider.CompareTag("GravitationField"))
        {
            return;
        }

        LevelUIManager.Instance.DisplayInputButton("RadarButton", false);
        LevelUIManager.Instance.DisplayInputButton("RayButton", true);

        if(radarComponent.IsRadarOn)
        {   
            ActivateShipRadar();
        }
    }

    void OnTriggerExit2D(Collider2D _collider)
    {
        if (!_collider.CompareTag("GravitationField"))
        {
            return;
        }

        LevelUIManager.Instance.DisplayInputButton("RadarButton", true);
        LevelUIManager.Instance.DisplayInputButton("RayButton", false);
        if(atractorRayComponent.IsAlienRayOn)
        {
            ActivateShipRadar();
        }
    }

    public void ApplyGravity(Vector2 _normalizedGravityDirection, float _gravityForce, GravitySourceType _gravitySource)
    {
        if(_gravitySource.Equals(GravitySourceType.Planet))
        {
            return;
        }

        // In case the user forgets to normalize the direction vector.
        Vector3 normalizedDirection = _normalizedGravityDirection.normalized;
        bodyComponent.AddForce(_normalizedGravityDirection * _gravityForce, ForceMode2D.Force);
    }

    public void ApplyRotationSpeed(Vector2 _normalizedRotationDirection, float _rotationForce)
    {
        // In case the user forgets to normalize the direction vector.
        Vector3 normalizedDirection = _normalizedRotationDirection.normalized;
        bodyComponent.AddForce(_normalizedRotationDirection * _rotationForce, ForceMode2D.Force);
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
