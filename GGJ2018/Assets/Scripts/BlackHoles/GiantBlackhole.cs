using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantBlackhole : Blackhole 
{
	float currentSize = 0;
    void OnEnable() 
    {
        currentSize = 0;
    }
    void Update()
    {
		transform.localScale = new Vector3 (currentSize, currentSize, 1);
		currentSize += Time.deltaTime/2;
		currentSize = Mathf.Clamp(currentSize, 0, 1);

		transform.localScale = new Vector3 (currentSize, currentSize, 1);
    }
}
