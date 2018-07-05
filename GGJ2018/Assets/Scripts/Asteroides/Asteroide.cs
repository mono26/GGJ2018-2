using System.Collections;
using UnityEngine;

public class Asteroide : MonoBehaviour
{
    [SerializeField]
    protected GameObject deadParticle;
    [SerializeField]
    protected float lifeTime;

    [SerializeField]
    protected Rigidbody2D body;
    protected Rigidbody2D Body { get { return body; } }

    // Use this for initialization
    void Start ()
    {
        if(body == null)
            body = GetComponent<Rigidbody2D>();

        StartCoroutine(DeadTimer());

        return;
	}

    public IEnumerator DeadTimer()
    {
        yield return new WaitForSeconds(lifeTime);
        AsteroidPool.Instance.ReleaseAsteroide(this.GetComponent<Rigidbody2D>());
    }

    public void ReleaseAsteroid()
    {
        // TODO pass direction of movement from the asteroid to the rotation of the particle
        var particle = Instantiate(deadParticle, transform.position, transform.rotation);
        particle.transform.rotation = Quaternion.FromToRotation(particle.transform.up, body.velocity);
        AsteroidPool.Instance.ReleaseAsteroide(this.GetComponent<Rigidbody2D>());
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        ReleaseAsteroid();
        return;
    }
}
