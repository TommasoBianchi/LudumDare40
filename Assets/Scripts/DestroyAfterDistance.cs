using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterDistance : MonoBehaviour {

    public float distance;
    [HideInInspector]
    public Action<GameObject> onDestroy;

    private Vector3 startPosition;
    
	void Start ()
    {
        startPosition = transform.position;
	}
	
	void Update ()
    {
		if((transform.position - startPosition).sqrMagnitude > distance * distance)
        {
            if(onDestroy != null)
            {
                onDestroy.Invoke(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
	}
}
