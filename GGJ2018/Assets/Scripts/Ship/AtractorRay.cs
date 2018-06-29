using System.Collections;
using UnityEngine;

public class AtractorRay : MonoBehaviour
{
    [Header("Atractor Ray settings")]
    [SerializeField]
    protected float range;
    [SerializeField]
    protected float radius;
    [SerializeField]
    protected float strenght;
    [SerializeField]
    protected float rate;
    [SerializeField]
    protected LayerMask layerMask;

    [Header("Components")]
    [SerializeField]
    protected Ship ship;
    [SerializeField]
    protected GameObject spriteEffect;

    [Header("Edittor debugging")]
    [SerializeField]
    private RaycastHit2D[] aliensHit;
    [SerializeField]
    private bool isAlienRayOn;
    public bool IsAlienRayOn { get { return isAlienRayOn; } }

    Coroutine routine = null;

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
        yield return new WaitForSeconds(rate);
        routine = ship.StartCoroutine(FindAliens());
    }

    public void StartRay()
    {
        isAlienRayOn = true;
        routine = ship.StartCoroutine(FindAliens());
        spriteEffect.SetActive(true);
    }

    public void StopRay()
    {
        isAlienRayOn = false;
        ship.StopCoroutine(routine);
        spriteEffect.SetActive(false);
    }
}
