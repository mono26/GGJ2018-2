using System.Collections;
using UnityEngine;

public class ParticleKiller : MonoBehaviour
{
    [SerializeField]
    public float lifeTime;

	// Use this for initialization
	void Start ()
    {
        StartCoroutine(DeadTimer());
    }

    public IEnumerator DeadTimer()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
