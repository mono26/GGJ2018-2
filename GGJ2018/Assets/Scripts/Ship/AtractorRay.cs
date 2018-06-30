using System.Collections;
using UnityEngine;

public class AtractorRay : ShipComponent
{
    [Header("Atractor Ray settings")]
    [SerializeField]
    protected LayerMask layerMask;
    [SerializeField]
    protected float range;
    [SerializeField]
    protected float radius = 1;
    [SerializeField]
    protected float ticksPerSecond = 10;
    [SerializeField]
    protected float strenght;

    [Header("Components")]
    [SerializeField]
    protected GameObject ufoRay;

    [Header("Edittor debugging")]
    [SerializeField]
    private RaycastHit2D[] aliensHit;
    [SerializeField]
    private bool isAlienRayOn;
    public bool IsAlienRayOn { get { return isAlienRayOn; } }

    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
        Gizmos.DrawRay(transform.position, -transform.up);
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
