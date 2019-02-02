using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [Header("Planet settings")]
    [SerializeField][Range(-1,1)] int gravitationalFieldDirection = 1; // +1 left, -1 right
    [SerializeField] float gravitationalRotation = 9.8f;
    [SerializeField] float gravity = 9.8f;
    [SerializeField] CircleCollider2D gravitationalField;
    [SerializeField]  bool playerInPlanet;
    [SerializeField] float rotationSpeed;
    [SerializeField] protected float planetRadius;

    [Header("Planet components")]
    [SerializeField]
    protected SignalEmitter signal;
    public SignalEmitter Signal { get { return signal; } }

    [Header("Planet editor debuggin")]
    [SerializeField] protected List<IInfluencedByGravity> objectsInsideGravitationField = new List<IInfluencedByGravity>();
    [SerializeField] protected float gravitationalFieldRadius;
    public float GetPlanetRadius { get { return planetRadius; } }
    public float GetGravitationalFieldStrenght { get { return gravitationalRotation; } }

    // Use this for initialization
    protected virtual void Awake()
    {
        CircleCollider2D collider = GetComponent<CircleCollider2D>();
        if (collider) {
            planetRadius = GetComponent<CircleCollider2D>().radius;
        }
        if (signal == false){
            signal = GetComponent<SignalEmitter>();
        }
        else return;
    }

    protected virtual void Start()
    {
        playerInPlanet = false;
        gravitationalFieldRadius = planetRadius + planetRadius;
        if(gravitationalField != null)
            gravitationalField.radius = gravitationalFieldRadius;

        return;
	}

    protected virtual void FixedUpdate()
    {
        if(!playerInPlanet)
        {
            return;
        }

        transform.Rotate(new Vector3(0, 0, rotationSpeed * gravitationalFieldDirection * Time.fixedDeltaTime));

        RotateObjectsInGravitationField();
        ApplyGravityOnObjects();
    }

    protected void RotateObjectsInGravitationField()
    {
        if(objectsInsideGravitationField.Count == 0)
        {
            return;
        }

        foreach (IInfluencedByGravity obj in objectsInsideGravitationField)
        {
            Vector2 directionFromObjectToCenter = obj.GetBodyComponent.position - (Vector2)transform.position;
            Vector2 tangentToDirectionToTheObject = new Vector2(-directionFromObjectToCenter.y, directionFromObjectToCenter.x).normalized * gravitationalFieldDirection;
            float rotationForce = obj.GetBodyComponent.mass * gravitationalRotation;

            obj.ApplyRotationalForce(tangentToDirectionToTheObject, rotationForce * Time.fixedDeltaTime);
            obj.RotateTowardsGravitationCenter(Vector2.Lerp(obj.GetBodyComponent.transform.up, directionFromObjectToCenter.normalized, 0.05f));
        }
    }

    protected virtual void ApplyGravityOnObjects()
    {
        if(objectsInsideGravitationField.Count == 0)
        {
            return;
        }

        foreach (IInfluencedByGravity obj in objectsInsideGravitationField)
        {
                Vector2 directionFromObjectToCenter = obj.GetBodyComponent.position - (Vector2)transform.position;
                float gravityForce = obj.GetBodyComponent.mass * gravity;

                obj.ApplyGravity(-directionFromObjectToCenter.normalized, gravityForce * Time.fixedDeltaTime);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D _collider)
    {
        IInfluencedByGravity obj = _collider.GetComponent<IInfluencedByGravity>();
        if (obj != null && objectsInsideGravitationField.Contains(obj) == false)
        {
            objectsInsideGravitationField.Add(obj);
            
        }

        if(_collider.tag == "Player")
            playerInPlanet = true;

    }

    protected virtual void OnTriggerExit2D(Collider2D _collider)
    {
        IInfluencedByGravity obj = _collider.GetComponent<IInfluencedByGravity>();
        if (obj != null && objectsInsideGravitationField.Contains(obj) == true)
        {
            objectsInsideGravitationField.Remove(obj);
        }

        if (_collider.tag == "Player")
            playerInPlanet = false;
    }
}
