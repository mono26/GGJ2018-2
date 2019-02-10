using System.Collections;
using UnityEngine;

[System.Serializable]
public class ShipInput
{
    [SerializeField] private float horizontalInput, verticalInput, radarFrequencyInput;
    [SerializeField] private bool rayInput, radarInput, shieldInput;

    public float GetHorizontalInput { get { return horizontalInput; } }
    public float GetVerticalInput { get { return verticalInput; } }
    public float GetRadarFrequencyInput { get { return radarFrequencyInput; } }
    public bool GetRayInput { get { return rayInput; } }
    public bool GetRadarInput { get { return radarInput; } }
    public bool GetShieldInput { get { return shieldInput; } }

    public ShipInput(){}
    public ShipInput(float _horizontal, float _vertical, float _radarFrequency, bool _ray, bool _radar, bool _shield)
    {
        horizontalInput = _horizontal;
        verticalInput = _vertical;
        radarFrequencyInput = _radarFrequency;
        rayInput = _ray;
        radarInput = _radar;
        shieldInput = _shield;
    }
}

public class Ship : MonoBehaviour, IAffectedByGravity
{
    [Header("Ship components")]
    [SerializeField] BoxCollider2D hitBoxComponent = null;
    [SerializeField] SpriteRenderer spriteComponent = null;
    [SerializeField] Rigidbody2D bodyComponent = null;
    [SerializeField] ShipEngine engineComponent = null;
    [SerializeField] Radar radarComponent = null;
    [SerializeField] AtractorRay atractorRayComponent = null;
    [SerializeField] Shield shieldComponent = null;

    [Header("Ship editor debugging.")]
    [SerializeField] ShipComponent[] components;
    [SerializeField] ShipInput currentInput = new ShipInput();    // TODO save input in exterinput component

    public Rigidbody2D GetBodyComponent { get { return bodyComponent; } }

	private void Awake()
    {
        components = GetComponents<ShipComponent>();

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
        if (shieldComponent == null) 
        {
            shieldComponent = GetComponent<Shield>();
        }
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

        if (currentInput.GetRadarInput)
        {
            ToggleShipRadar();
        }

        if (currentInput.GetRayInput)
        {
            ToggleShipRay();
        }

        if (currentInput.GetShieldInput)
        {
            ToggleShipShield();
        }

        HandleShipRadarFrequency();
    }

    void ToggleShipRay()
    {
        atractorRayComponent.ToggleRay();
    }

    void ToggleShipRadar()
    {
        radarComponent.ToggleRadar();
    }

    void ToggleShipShield()
    {
        shieldComponent.ToggleShield();
    }

    private void HandleShipRadarFrequency()
    {
        if(radarComponent == null) 
        {
            return;
        }

        radarComponent.ChangeFrecuency((int)currentInput.GetRadarFrequencyInput);
    }

    private void FixedUpdate()
    {
        DriveShip();
    }

    private void DriveShip()
    {
        if(engineComponent == null)
        {
            return;
        }

        if (currentInput.GetHorizontalInput != 0 || currentInput.GetVerticalInput != 0)
        {
            engineComponent.ApplyEngineThrust( (transform.right * currentInput.GetHorizontalInput) + 
            (transform.up * currentInput.GetVerticalInput) );
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
            ToggleShipRadar();
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
            ToggleShipRadar();
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
