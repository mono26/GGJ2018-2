using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [Header("Planet settings")]
    [SerializeField][Range(-1,1)]   // +1 right, -1 left
    protected int gravitationalFieldDirection = 1;
    [SerializeField]
    protected float gravitationalFieldStrenght = 1000f;
    [SerializeField]
    protected float gravityStrenght = 900f;
    public float GravitationalFieldStrenght { get { return gravitationalFieldStrenght; } }
    [SerializeField]
    protected CircleCollider2D gravitationalField;
    [SerializeField]
    protected float planetRadius;
    public float PlanetRadius { get { return planetRadius; } }

    [Header("Components")]
    [SerializeField]
    protected SignalEmitter signal;
    public SignalEmitter Signal { get { return signal; } }

    [Header("Editor debuggin")]
    [SerializeField]
    protected List<Rigidbody2D> objectsInsideGravitationField;
    [SerializeField]
    protected float gravitationalFieldRadius;

    // Use this for initialization
    protected virtual void Awake()
    {
        var collider = GetComponent<CircleCollider2D>();
        if (collider)
        {
            planetRadius = GetComponent<CircleCollider2D>().radius;
        }

        if (signal == false)
            signal = GetComponent<SignalEmitter>();

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
                Vector2 directionFromCenterToObject = obj.position - (Vector2)transform.position;
                Vector2 tangentToDirectionToTheObject = new Vector2(directionFromCenterToObject.y, -directionFromCenterToObject.x).normalized * gravitationalFieldDirection;
                obj.AddForce(tangentToDirectionToTheObject * gravitationalFieldStrenght * Time.fixedDeltaTime, ForceMode2D.Force);
                obj.transform.up = Vector2.Lerp(obj.transform.up, directionFromCenterToObject.normalized, 0.05f);
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
                Vector2 directionFromCenterToObject = obj.position - (Vector2)transform.position;
                obj.AddForce(-directionFromCenterToObject.normalized * gravityStrenght * Time.fixedDeltaTime, ForceMode2D.Force);
            }
        }
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
