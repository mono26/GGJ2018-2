using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Asteroide : MonoBehaviour, EventHandler<BlackHoleCenterReachEvent>
{
    [SerializeField]
    protected GameObject deadParticle;
    [SerializeField]
    protected float lifeTime = 10;

    [SerializeField]
    protected Rigidbody2D body;
    protected Rigidbody2D Body { get { return body; } }

    // Use this for initialization
    protected void Start ()
    {
        if(body == null)
            body = GetComponent<Rigidbody2D>();

        StartCoroutine(DeadTimer());

        return;
	}

    protected void OnEnable()
    {
        EventManager.AddListener<BlackHoleCenterReachEvent>(this);
        return;
    }

    protected void OnDisable()
    {
        EventManager.RemoveListener<BlackHoleCenterReachEvent>(this);
        return;
    }

    protected IEnumerator DeadTimer()
    {
        yield return new WaitForSeconds(lifeTime);

        AsteroidPool.Instance.ReleaseAsteroide(this.GetComponent<Rigidbody2D>());

        yield break;
    }

    public void ReleaseAsteroid()
    {
        // TODO pass direction of movement from the asteroid to the rotation of the particle
        var particle = Instantiate(deadParticle, transform.position, transform.rotation);
        particle.transform.rotation = Quaternion.FromToRotation(particle.transform.up, body.velocity);
        AsteroidPool.Instance.ReleaseAsteroide(this.GetComponent<Rigidbody2D>());

        return;
    }

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        ReleaseAsteroid();

        return;
    }

    public void OnEvent(BlackHoleCenterReachEvent _blackHoleEvent)
    {
        if (_blackHoleEvent.affectedObject.Equals(body) == true)
        {
            ReleaseAsteroid();
        }

        return;
    }
}
