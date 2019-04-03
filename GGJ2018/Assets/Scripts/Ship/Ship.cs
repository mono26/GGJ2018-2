using System.Collections;
using UnityEngine;

[System.Serializable]
public class ShipInput
{
    [SerializeField] private float horizontalInput, verticalInput;
    [SerializeField] private bool rayInput, boostInput, shieldInput;

    public float GetHorizontalInput { get { return horizontalInput; } }
    public float GetVerticalInput { get { return verticalInput; } }
    public bool GetRayInput { get { return rayInput; } }
    public bool GetBoostInput { get { return boostInput; } }
    public bool GetShieldInput { get { return shieldInput; } }

    public ShipInput(){}
    public ShipInput(float _horizontal, float _vertical, bool _ray, bool _boost, bool _shield)
    {
        horizontalInput = _horizontal;
        verticalInput = _vertical;
        rayInput = _ray;
        boostInput = _boost;
        shieldInput = _shield;
    }
}

// TODO Remove IAffectedByGravity
public class Ship : MonoBehaviour
{
    [Header("Ship components")]
    [SerializeField] BoxCollider2D hitBoxComponent = null;
    [SerializeField] SpriteRenderer spriteComponent = null;
    [SerializeField] Rigidbody2D bodyComponent = null;
    [SerializeField] ShipEngine engineComponent = null;
    [SerializeField] Radar radarComponent = null;
    [SerializeField] SignalOscilator oscilatorComponent = null;
    [SerializeField] AtractorRay atractorRayComponent = null;
    [SerializeField] Shield shieldComponent = null;
    [SerializeField] ParticleSystem trailParticles;

    [Header("Ship editor debugging.")]
    [SerializeField] ShipComponent[] components;
    [SerializeField] ShipInput currentInput = new ShipInput();    // TODO save input in exterinput component

    public Rigidbody2D GetBodyComponent { get { return bodyComponent; } }
    public AtractorRay GetRayComponent { get { return atractorRayComponent; } }
    public Shield GetShieldComponent { get { return shieldComponent; } }
    public ShipEngine GetEngineComponent { get { return engineComponent; } }

    //Interface EventHandler
    public delegate void UIAction();
    public static event UIAction UpdateButton;

    public delegate void GameEvent();
    public static event GameEvent PlayerInOrbit;

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

        if (oscilatorComponent == null)
        {
            oscilatorComponent = GetComponent<SignalOscilator>();
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
        UpdateButton();
    }

    private void Update ()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            ApplyBoost();
        }

        foreach (ShipComponent component in components)
        {
            component.EveryFrame();
        }

        if (currentInput.GetBoostInput)
        {
            ApplyBoost();
        }

        if (currentInput.GetRayInput)
        {
            ToggleShipRay();
        }

        if (currentInput.GetShieldInput)
        {
            ToggleShipShield();
        }
    }

    void ToggleShipRay()
    {
        atractorRayComponent.ToggleRay();
    }

    void ToggleShipShield()
    {
        shieldComponent.ToggleShield();
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
            if (trailParticles.isStopped && !atractorRayComponent.IsAlienRayOn)
            {
                trailParticles.Play();
            }
            
        }else
            {
                trailParticles.Stop();
            }
    }

    private void ApplyBoost()
    {
        if (engineComponent == null)
        {
            return;
        }

        if (currentInput.GetHorizontalInput != 0 || currentInput.GetVerticalInput != 0)
        {
            engineComponent.ApplyBoost((transform.right * currentInput.GetHorizontalInput) +
            (transform.up * currentInput.GetVerticalInput));
        }
    }

    private void OnTriggerEnter2D(Collider2D _collider)
    {
        Debug.LogError(_collider.gameObject.name);

        if (!_collider.CompareTag("GravitationField"))
        {
            return;
        }


        // Changes active buttons on GUI if it enters a planet
        PlayerInOrbit();
    }

    void OnTriggerStay2D(Collider2D _collider)
    {

        if (!_collider.CompareTag("GravitationField"))
        {
            return;
        }
        UpdateButton();


    }

    void OnTriggerExit2D(Collider2D _collider)
    {
        if (!_collider.CompareTag("GravitationField"))
        {
            return;
        }

        // Changes active buttons on GUI if it goes out of a planet
        PlayerInOrbit();
        UpdateButton();

        if(atractorRayComponent.IsAlienRayOn)
        {
            ToggleShipRay();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Planet"))
        {
            engineComponent.LoseFuelAmount(400);
        }
    }
    // public void ApplyGravity(Vector2 _normalizedGravityDirection, float _gravityForce, GravitySourceType _gravitySource)
    // {
    //     if(_gravitySource.Equals(GravitySourceType.Planet))
    //     {
    //         return;
    //     }

    //     // In case the user forgets to normalize the direction vector.
    //     Vector3 normalizedDirection = _normalizedGravityDirection.normalized;
    //     bodyComponent.AddForce(_normalizedGravityDirection * _gravityForce, ForceMode2D.Force);
    // }

    // public void ApplyRotation(Vector2 _normalizedRotationDirection, float _rotationForce)
    // {
    //     // In case the user forgets to normalize the direction vector.
    //     Vector3 normalizedDirection = _normalizedRotationDirection.normalized;
    //     bodyComponent.AddForce(_normalizedRotationDirection * _rotationForce, ForceMode2D.Force);
    // }

    // public void RotateTowardsGravitationCenter(Vector2 _gravitationCenterDirection)
    // {
    //     Debug.DrawRay(transform.position, -_gravitationCenterDirection, Color.yellow, 10);
    //     Debug.DrawRay(transform.position, -transform.up, Color.red, 10);
    //     transform.up = _gravitationCenterDirection;
    // }

    public void ReceiveInput(ShipInput _inputToRecieve)
    {
        currentInput = _inputToRecieve;
    }
}
