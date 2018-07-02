using UnityEngine;

public class Planet : MonoBehaviour
{
    [Header("Planet settings")]
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

    // Use this for initialization
    public virtual void Awake()
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

    public virtual void Start()
    {
        gravitationalFieldRadius = planetRadius + planetRadius;
        if(gravitationalField != null)
            gravitationalField.radius = gravitationalFieldRadius;

        return;
	}

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Asteroide"))
        {
            // TODO Release asteroid and spawn particles
            collision.gameObject.GetComponent<Asteroide>().ReleaseAsteroid();
        }
    }
}
