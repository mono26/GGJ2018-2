using System.Collections.Generic;
using UnityEngine;

public enum GravitySourceType { Planet, Blackhole }

public class Planet : SpawnableObject
{
    [Header("Planet settings")]
    [SerializeField][Range(-1,1)] int gravitationalFieldDirection = 1; // +1 left, -1 right
    [SerializeField] float rotationSpeed = 4.4f;
    [SerializeField] float rotationForce = 9.8f;
    [SerializeField] float gravityForce = 9.8f;
    [SerializeField] protected GravitySourceType gravitySource;
    [SerializeField] bool playerInGravitationalField;
    [SerializeField] float minGravFieldDistFromPlanet = 10.0f;

    [Header("Planet components")]
    [SerializeField]
    protected SignalEmitter signal;
    [SerializeField] CircleCollider2D gravitationalField;

    [SerializeField] AutoDestroyComponent autoDestroyComponent = null;

    [Header("Planet editor debuggin")]
    [SerializeField] protected List<IAffectedByGravity> objsInGravitationField = new List<IAffectedByGravity>();

    public float GetGravFieldRadius { get { return GetRadius + minGravFieldDistFromPlanet; } }
    public SignalEmitter Signal { get { return signal; } }

    bool alreadyAwaked = false;

    // Use this for initialization
    public override void Awake()
    {
        if (alreadyAwaked)
        {
            return;
        }

        base.Awake();

        CircleCollider2D planetCollider = GetComponent<CircleCollider2D>();
        if (planetCollider) 
        {
            GetComponent<CircleCollider2D>().radius = GetRadius;
        }

        if (gravitationalField == null)
        {
            GetComponentInChildren<CircleCollider2D>();
        }

        if(gravitationalField != null)
        {
            gravitationalField.radius = GetGravFieldRadius;
        }

        if (signal == null)
        {
            signal = GetComponent<SignalEmitter>();
        }

        if (autoDestroyComponent == null)
        {
            autoDestroyComponent = GetComponent<AutoDestroyComponent>();
        }
    }

    protected virtual void Start()
    {
        gravitySource = GravitySourceType.Planet;

        playerInGravitationalField = false;
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

            Vector2 directionFromObjectToCenter = (objsInGravitationField[i].GetBodyComponent.position - (Vector2)transform.position).normalized;
            Vector2 tangentToDirectionToTheObject = new Vector2(-directionFromObjectToCenter.y, directionFromObjectToCenter.x).normalized * gravitationalFieldDirection;
            float rotationForceToApply = rotationForce * objsInGravitationField[i].GetBodyComponent.mass;

            objsInGravitationField[i].ApplyRotationSpeed(tangentToDirectionToTheObject, rotationForceToApply * Time.fixedDeltaTime);
            // Becuse we are using the opposite of the transform.up for the lerp. We must use the opposite of the lerp.
            objsInGravitationField[i].RotateTowardsGravitationCenter(Vector2.Lerp(objsInGravitationField[i].GetBodyComponent.transform.up, directionFromObjectToCenter, 0.05f));
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
            float gravityForceToApply = gravityForce * objsInGravitationField[i].GetBodyComponent.mass;

            objsInGravitationField[i].ApplyGravity(-directionFromObjectToCenter.normalized, gravityForceToApply * Time.fixedDeltaTime, gravitySource);
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

    public override void Release()
    {
        PoolsManager.Instance.ReleaseObjectToPool(this);
    }

    public void SetLifeTimeAccordingToDistanceFromPlayer(float _distanceFromPlayer)
    {
        if (autoDestroyComponent == null)
        {
            return;
        }
        
        int minLifeTime = autoDestroyComponent.GetMinLifeTime;
        int maxLifeTime = autoDestroyComponent.GetMaxLifeTime;
        int lifeTime = Random.Range(minLifeTime, maxLifeTime);
        autoDestroyComponent.SetLifeTime(lifeTime + (_distanceFromPlayer * 2));
    }
}
