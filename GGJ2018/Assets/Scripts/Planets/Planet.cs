using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [Header("Planet settings")]
    [SerializeField][Range(-1,1)]
    protected int gravitationalFieldDirection;
    [SerializeField]
    protected float gravitationalFieldRadius;
    [SerializeField]
    protected float gravitationalFieldStrenght;
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
    protected List<GameObject> objectsInsideGravitationField;

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

    protected virtual void Update()
    {
        RotateObjectsInGravitationField();
    }

    protected void RotateObjectsInGravitationField()
    {
        if(objectsInsideGravitationField.Count > 0)
        {
            foreach (GameObject obj in objectsInsideGravitationField)
            {
                obj.transform.RotateAround(transform.position, transform.forward, gravitationalFieldStrenght * gravitationalFieldDirection * Time.deltaTime);
                var direction = (obj.transform.position - transform.position).normalized;
                obj.transform.up = Vector2.Lerp(obj.transform.up, direction, 0.05f);
            }
        }

        return;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Asteroide"))
        {
            // TODO Release asteroid and spawn particles
            collision.gameObject.GetComponent<Asteroide>().ReleaseAsteroid();
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D _collider)
    {
        if(objectsInsideGravitationField.Contains(_collider.gameObject) == false)
        {
            objectsInsideGravitationField.Add(_collider.gameObject);
        }
        return;
    }

    protected virtual void OnTriggerExit2D(Collider2D _collider)
    {
        if (objectsInsideGravitationField.Contains(_collider.gameObject) == true)
        {
            objectsInsideGravitationField.Remove(_collider.gameObject);
        }
        return;
    }
}
