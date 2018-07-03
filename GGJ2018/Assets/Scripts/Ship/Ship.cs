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

    protected void Update ()
    {
        foreach (ShipComponent component in shipComponents)
            component.EveryFrame();

        return;
    }
}
