using UnityEngine;

public class Planet : MonoBehaviour
{
    // Assigned by editor or taken by the radius of the collider
    [SerializeField]
    float planetRadius;
    float gravitationalFieldRadius;
    [SerializeField]
    float gravitationalFieldStrenght;
    [SerializeField]
    private CircleCollider2D gravitationalField;

    public float PlanetRadius { get { return planetRadius; } }
    public float GravitationalFieldRadius { get { return gravitationalFieldRadius; } }
    public float GravitationalFieldStrenght { get { return gravitationalFieldStrenght; } }
    public CircleCollider2D GravitationalField{ get { return gravitationalField; } }

    // Use this for initialization
    public void Awake()
    {
        var collider = GetComponent<CircleCollider2D>();
        if (collider)
        {
            planetRadius = GetComponent<CircleCollider2D>().radius;
        }
        else return;
    }

    public virtual void Start()
    {
        gravitationalFieldRadius = planetRadius + planetRadius;
        gravitationalField.radius = gravitationalFieldRadius;
	}

    public virtual void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("Asteroid"))
        {
            // TODO Release asteroid and spawn particles
            collision.gameObject.GetComponent<Asteroide>().ReleaseAsteroid();
        }
    }
}
