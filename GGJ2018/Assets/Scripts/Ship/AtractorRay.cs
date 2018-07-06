using System.Collections;
using UnityEngine;

public class AtractorRay : ShipComponent
{
    [Header("Atractor Ray settings")]
    [SerializeField]
    protected LayerMask layerMask;
    [SerializeField]
    protected float range = 1.5f;
    [SerializeField]
    protected float radius = 0.7f;
    [SerializeField]
    protected float ticksPerSecond = 10;
    [SerializeField]
    protected float strenght = 1000 ;

    [Header("Components")]
    [SerializeField]
    protected GameObject ufoRay;

    [Header("Edittor debugging")]
    [SerializeField]
    private RaycastHit2D[] aliensHit;
    [SerializeField]
    private bool isAlienRayOn = false;
    public bool IsAlienRayOn { get { return isAlienRayOn; } }

    protected override void Awake()
    {
        base.Awake();

        if(ufoRay == null)
        {
            ufoRay = transform.Find("UFORay").gameObject;
        }
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position - (transform.up * range), radius);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position - (transform.up * range));

        return;
    }

    protected void OnEnable()
    {
        ufoRay.SetActive(false);
        isAlienRayOn = false;
        return;
    }

    protected override void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            if (isAlienRayOn == false)
            {
                StartRay();
            }
            else if (isAlienRayOn == true)
            {
                StopRay();
            }
        }
    }

    private IEnumerator FindAliens()
    {
        aliensHit = Physics2D.CircleCastAll(ship.transform.position, radius, -ship.transform.up, range, layerMask);
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
        else yield return null;
        yield return new WaitForSeconds(1 / ticksPerSecond);
        StartCoroutine(FindAliens());
    }

    public void StartRay()
    {
        isAlienRayOn = true;
        StartCoroutine(FindAliens());
        ufoRay.SetActive(true);
    }

    public void StopRay()
    {
        isAlienRayOn = false;
        ship.StopCoroutine(FindAliens());
        ufoRay.SetActive(false);
    }
}
