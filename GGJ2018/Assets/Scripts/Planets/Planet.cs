using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [Header("Planet settings")]
    [SerializeField][Range(-1,1)]   // +1 right, -1 left
    protected int gravitationalFieldDirection = 1;
    [SerializeField] protected float gravitationalRotation = 9.8f;
    [SerializeField] protected float gravity = 9.8f;
    [SerializeField] protected CircleCollider2D gravitationalField;
    [SerializeField] protected float planetRadius;

    [Header("Planet components")]
    [SerializeField]
    protected SignalEmitter signal;
    public SignalEmitter Signal { get { return signal; } }

    [Header("Planet editor debuggin")]
    [SerializeField] protected List<Rigidbody2D> objectsInsideGravitationField;
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
        gravitationalFieldRadius = planetRadius + planetRadius;
        if(gravitationalField != null)
            gravitationalField.radius = gravitationalFieldRadius;

        return;
	}

    protected virtual void FixedUpdate()
    {
        RotateObjectsInGravitationField();
        ApplyGravityOnObjects();
        return;
    }

    protected void RotateObjectsInGravitationField()
    {
        if(objectsInsideGravitationField.Count > 0)
        {
            foreach (Rigidbody2D obj in objectsInsideGravitationField)
            {
                Vector2 directionFromObjectToCenter = obj.position - (Vector2)transform.position;
                Vector2 tangentToDirectionToTheObject = new Vector2(directionFromObjectToCenter.y, -directionFromObjectToCenter.x).normalized * gravitationalFieldDirection;
                float rotationForce = obj.mass * gravitationalRotation;
                obj.AddForce(tangentToDirectionToTheObject * rotationForce * Time.fixedDeltaTime, ForceMode2D.Force);
                obj.transform.up = Vector2.Lerp(obj.transform.up, directionFromObjectToCenter.normalized, 0.05f);
            }
        }
        return;
    }

    protected void ApplyGravityOnObjects()
    {
        if (objectsInsideGravitationField.Count > 0)
        {
            foreach (Rigidbody2D obj in objectsInsideGravitationField)
            {
                Vector2 directionFromObjectToCenter = obj.position - (Vector2)transform.position;
                float gravityForce = obj.mass * gravity;
                obj.AddForce(-directionFromObjectToCenter.normalized * gravityForce * Time.fixedDeltaTime, ForceMode2D.Force);
            }
        }
        return;
    }

    protected virtual void OnTriggerEnter2D(Collider2D _collider)
    {
        Rigidbody2D objectsBody = _collider.GetComponent<Rigidbody2D>();
        if (objectsBody != null && objectsInsideGravitationField.Contains(objectsBody) == false)
        {
            objectsInsideGravitationField.Add(objectsBody);
        }
        return;
    }

    protected virtual void OnTriggerExit2D(Collider2D _collider)
    {
        Rigidbody2D objectsBody = _collider.GetComponent<Rigidbody2D>();
        if (objectsBody != null && objectsInsideGravitationField.Contains(objectsBody) == true)
        {
            objectsInsideGravitationField.Remove(objectsBody);
        }
        return;
    }
}
