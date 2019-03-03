using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroyEffect : AutoDestroyEffect 
{
	[SerializeField] GameObject particleEffect = null;
	public override void SpawnEffect()
	{
		GameObject particle = Instantiate(particleEffect, transform.position, transform.rotation);
	}
}
