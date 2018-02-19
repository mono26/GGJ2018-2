using System.Collections;
using UnityEngine;

public class Alien : MonoBehaviour
{
    [SerializeField]
    float lifeTime = 5.0f;

    public IEnumerator KillAlien()
    {
        yield return new WaitForSeconds(lifeTime);
        DestroyObject(gameObject);
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Gravitation Field"))
        {
            StartCoroutine(KillAlien());
            GetComponentInParent<AlienPlanet>().RemoveAlien(transform);
        }
    }
}
