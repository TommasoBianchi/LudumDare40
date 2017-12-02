using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {

    public float speed = 0;
    
	void Start () {
		
	}
	
	void FixedUpdate () {
        transform.Translate(transform.forward * speed * Time.fixedDeltaTime);
	}
}
