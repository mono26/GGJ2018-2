using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [Header("Planet settings")]
    [SerializeField][Range(-1,1)] int gravitationalFieldDirection = 1; // +1 left, -1 right
    [SerializeField] float gravitationalRotation = 9.8f;
    [SerializeField] float gravity = 9.8f;
    [SerializeField] CircleCollider2D gravitationalField;
    [SerializeField]  bool playerInGravitationalField;
    [SerializeField] float rotationSpeed;
    [SerializeField] protected float planetRadius;

    [Header("Planet components")]
    [SerializeField]
    protected SignalEmitter signal;

    [Header("Planet editor debuggin")]
    [SerializeField] protected List<IAffectedByGravity> objsInGravitationField = new List<IAffectedByGravity>();

    public float GetPlanetRadius { get { return planetRadius; } }
    public float GetGravitationalFieldStrenght { get { return gravitationalRotation; } }
    public SignalEmitter Signal { get { return signal; } }

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
        playerInGravitationalField = false;
        float gravitationalFieldRadius = planetRadius + planetRadius;
        if(gravitationalField != null)
            gravitationalField.radius = gravitationalFieldRadius;

        return;
	}

    protected virtual void FixedUpdate()
    {
        if(!playerInGravitationalField)
        {
            return;
        }

        transform.Rotate(new Vector3(0, 0, rotationSpeed * gravitationalFieldDirection * Time.fixedDeltaTime));

        RotateObjectsInGravitationField();
        ApplyGravityOnObjects();
    }

    protected void RotateObjectsInGravitationField()
    {
        if(objsInGravitationField.Count == 0)
        {
            return;
        }

        for (int i =  objsInGravitationField.Count - 1; i > -1; i--)
        {
            if(objsInGravitationField[i] == null)
            {
                objsInGravitationField.RemoveAt(i);
                continue;
            }

            Vector2 directionFromObjectToCenter = objsInGravitationField[i].GetBodyComponent.position - (Vector2)transform.position;
            Vector2 tangentToDirectionToTheObject = new Vector2(-directionFromObjectToCenter.y, directionFromObjectToCenter.x).normalized * gravitationalFieldDirection;
            float rotationForce = objsInGravitationField[i].GetBodyComponent.mass * gravitationalRotation;

            objsInGravitationField[i].ApplyRotationalForce(tangentToDirectionToTheObject, rotationForce * Time.fixedDeltaTime);
            objsInGravitationField[i].RotateTowardsGravitationCenter(Vector2.Lerp(objsInGravitationField[i].GetBodyComponent.transform.up, directionFromObjectToCenter.normalized, 0.05f));
        }
    }

    protected virtual void ApplyGravityOnObjects()
    {
        if(objsInGravitationField.Count == 0)
        {
            return;
        }

        for (int i =  objsInGravitationField.Count - 1; i > -1; i--)
        {
            if(objsInGravitationField[i] == null)
            {
                objsInGravitationField.RemoveAt(i);
                continue;
            }

            Vector2 directionFromObjectToCenter = objsInGravitationField[i].GetBodyComponent.position - (Vector2)transform.position;
            float gravityForce = objsInGravitationField[i].GetBodyComponent.mass * gravity;

            objsInGravitationField[i].ApplyGravity(-directionFromObjectToCenter.normalized, gravityForce * Time.fixedDeltaTime);
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D _collider)
    {
        IAffectedByGravity obj = _collider.GetComponent<IAffectedByGravity>();
        if (obj != null && !objsInGravitationField.Contains(obj))
        {
            objsInGravitationField.Add(obj);
        }

        if(_collider.tag == "Player")
        {
            playerInGravitationalField = true;
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D _collider)
    {
        IAffectedByGravity obj = _collider.GetComponent<IAffectedByGravity>();
        if (obj != null && objsInGravitationField.Contains(obj) == true)
        {
            objsInGravitationField.Remove(obj);
        }

        if (_collider.tag == "Player")
        {
            playerInGravitationalField = false;
        }
    }
}
