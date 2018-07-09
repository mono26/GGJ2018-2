using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoleCenterReachEvent : SpaceEvent
{
    public Rigidbody2D affectedObject;

    public BlackHoleCenterReachEvent(Rigidbody2D _affectedObject)
    {
        affectedObject = _affectedObject;
    }
}

public class BlackHole : Planet
{
    [Header("Black Hole settings")]
    [SerializeField]
    private float lifeTime;
    protected float minimumDistanceToCenter;
    [SerializeField]
    private float lifeTimeCounter;

    protected void OnEnable ()
    {
        lifeTimeCounter = lifeTime;

        return;
	}
	
	// Update is called once per frame
	protected override void FixedUpdate ()
    {
        if (lifeTimeCounter <= 0)
        {
            BlackholePool.Instance.ReleaseBlackholes(this.GetComponent<Rigidbody2D>());
            return;
        }

        //base.FixedUpdate();
        ApplyGravityOnObjects();

        lifeTimeCounter -= Time.deltaTime;

        return;
    }

    protected void CheckObjectsDistanceToCenter()
    {
        foreach(Rigidbody2D obj in objectsInsideGravitationField)
        {
            if(Vector3.Distance(obj.position, transform.position) < minimumDistanceToCenter)
            {
                // TODO destroy de object.
                // TODO apply damage to engine.
            }
        }
    }
}
