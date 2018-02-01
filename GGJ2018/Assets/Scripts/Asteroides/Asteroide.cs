using System.Collections;
using UnityEngine;

public class Asteroide : MonoBehaviour
{
    [SerializeField]
    private GameObject deadParticle;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(DeadTimer());
	}

    public IEnumerator DeadTimer()
    {
        yield return new WaitForSeconds(5);
        AsteroidPool.Instance.ReleaseAsteroide(this.GetComponent<Rigidbody2D>());
    }

    public void ReleaseAsteroid()
    {
        Instantiate(deadParticle, transform.position, transform.rotation);
        AsteroidPool.Instance.ReleaseAsteroide(this.GetComponent<Rigidbody2D>());
    }
}
