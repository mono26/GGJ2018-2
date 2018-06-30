using System.Collections;
using UnityEngine;

public class Ship : MonoBehaviour
{
    [SerializeField]
    protected BoxCollider2D shipCollider;
    public BoxCollider2D ShipCollider { get { return shipCollider; } }
    [SerializeField]
    protected SpriteRenderer shipSprite;
    public SpriteRenderer ShipSprite { get { return shipSprite; } }
    [SerializeField]
    protected Rigidbody2D shipBody;
    public Rigidbody2D ShipBody { get { return shipBody; } }

    protected ShipComponent[] shipComponents;
    protected bool isOnGravitationalField = false;

	void Awake()
    {
        if (shipCollider == null)
            shipCollider = GetComponent<BoxCollider2D>();
        if (shipSprite == null)
            shipSprite = GetComponent<SpriteRenderer>();
        if (shipBody == null)
            shipBody = GetComponent<Rigidbody2D>();

        shipComponents = GetComponents<ShipComponent>();
    }

    void Update ()
    {
        foreach (ShipComponent component in shipComponents)
            component.EveryFrame();

        return;
    }

    private IEnumerator RotateArroundPlanet(Planet _planet)
    {
        while (isOnGravitationalField)
        {
            transform.RotateAround(_planet.transform.position, _planet.transform.forward, _planet.GravitationalFieldStrenght * Time.deltaTime);
            var direction = (transform.position - _planet.transform.position).normalized;
            transform.up = Vector2.Lerp(transform.up, direction, 0.05f);
            yield return null;
        }
        yield return null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Gravitation Field"))
        {
            Debug.Log("Entrando en la orbita del planeta");
            isOnGravitationalField = true;
            StartCoroutine(RotateArroundPlanet(collision.GetComponentInParent<Planet>()));
        }

        if (collision.gameObject.CompareTag("Alien"))
        {
            Destroy(collision.gameObject);
            GameController.Instance.IncreaseScore();
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Gravitation Field"))
        {
            isOnGravitationalField = false;
        }
    }
}
