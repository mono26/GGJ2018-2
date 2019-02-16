using System.Collections;
using UnityEngine;

public class AtractorRay : ShipComponent
{
    [Header("Atractor Ray settings")]
    [SerializeField] protected LayerMask alienLayer;
    [SerializeField] protected float range = 1.5f;
    [SerializeField] protected float radius = 0.7f;
    [SerializeField] protected float ticksPerSecond = 10;
    [SerializeField] protected float strenght = 1000 ;

    [Header("Components")]
    [SerializeField] protected GameObject ufoRay;
    [SerializeField] protected GameObject door;

    [Header("Edittor debugging")]
    protected Coroutine atractAliensRoutine;
    [SerializeField] private bool isAlienRayOn = false;
    public bool IsAlienRayOn { get { return isAlienRayOn; } }

    protected override void Awake()
    {
        base.Awake();

        if(ufoRay == null)
        {
            ufoRay = transform.Find("UFORay").gameObject;
        }
    }

    void Start() 
    {
        ufoRay.SetActive(false);
        door.SetActive(true);
        isAlienRayOn = false;
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position - (transform.up * range), radius);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position - (transform.up * range));

        return;
    }

    public void ToggleRay()
    {
        isAlienRayOn = !isAlienRayOn;

        if (isAlienRayOn)
        {
            atractAliensRoutine = StartCoroutine(FindAliens());
        } 

        if (!isAlienRayOn && atractAliensRoutine != null)
        {
            StopCoroutine(atractAliensRoutine);
        }

        TurnRayOn();

        OpenDoor();
    }

    IEnumerator FindAliens()
    {
        RaycastHit2D[] aliensHit = Physics2D.CircleCastAll(ship.transform.position, radius, -ship.transform.up, range, alienLayer);
        if (aliensHit.Length > 0)
        {
            foreach (RaycastHit2D hit in aliensHit)
            {
                if (hit.collider.CompareTag("Alien"))
                {
                    hit.collider.GetComponent<Rigidbody2D>().AddForce(hit.transform.up * strenght);
                }
            }
        }

        yield return new WaitForSeconds(1 / ticksPerSecond);
        atractAliensRoutine = StartCoroutine(FindAliens());
        yield break;
    }

    void TurnRayOn()
    {
        if (ufoRay != null)
        {
            ufoRay.SetActive(isAlienRayOn);
        }
    }

    void OpenDoor()
    {
        door.SetActive(!isAlienRayOn);
        return;
    }
}
