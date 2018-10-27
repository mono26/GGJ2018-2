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

public class Ship : MonoBehaviour
{
    [Header("Ship components")]
    [SerializeField] private BoxCollider2D hitBox;
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private ShipEngine engine;
    [SerializeField] private Radar radar;
    [SerializeField] private AtractorRay ray;

    [Header("Ship editor debugging.")]
    [SerializeField] private ShipComponent[] components;
    [SerializeField] private ShipInput currentInput = new ShipInput();    // TODO save input in exterinput component

    public Rigidbody2D GetShipBody { get { return body; } }

	private void Awake()
    {
        if (hitBox == null) {
            hitBox = GetComponent<BoxCollider2D>();
        }
        if (sprite == null) {
            sprite = GetComponent<SpriteRenderer>();
            if(sprite == null) {
                sprite = GetComponentInChildren<SpriteRenderer>();
            }
        }
        if (body == null) {
            body = GetComponent<Rigidbody2D>();
        }
        components = GetComponents<ShipComponent>();
        return;
    }

    private void Update ()
    {
        ActivateShipRay();
        ActivateShipRadar();
        HandleShipRadarFrequency();
        foreach (ShipComponent component in components){
            component.EveryFrame();
        }
        return;
    }

    private void ActivateShipRay()
    {
        if(ray != null && currentInput.GetRayState) {
            ray.ActivateRay();
        }
        return;
    }

    private void ActivateShipRadar()
    {
        if(ray != null && currentInput.GetRadarState) {
            radar.ActivateRadar();
        }
    }

    private void HandleShipRadarFrequency()
    {
        if(radar != null) {   
            radar.ChangeFrecuency((int)currentInput.GetRadarFrequency);
        }
        return;
    }

    private void FixedUpdate()
    {
        DriveShip();
        return;
    }

    private void DriveShip()
    {
        if(engine != null)
        {
            engine.ApplyEngineThrust(transform.right * currentInput.GetHorizontal);
            engine.ApplyEngineThrust(transform.up * currentInput.GetVertical);
        }
        return;
    }

    public void ReceiveInput(ShipInput _inputToRecieve)
    {
        currentInput = _inputToRecieve;
        return;
    }
}
