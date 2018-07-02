﻿using System.Collections;
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
    protected float strenght = 10;

    [Header("Components")]
    [SerializeField]
    protected GameObject ufoRay;

    [Header("Edittor debugging")]
    [SerializeField]
    private RaycastHit2D[] aliensHit;
    [SerializeField]
    private bool isAlienRayOn = false;
    public bool IsAlienRayOn { get { return isAlienRayOn; } }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + (Vector3.down * range), radius);
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, -transform.up);

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
