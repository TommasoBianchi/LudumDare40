using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour {

    [SerializeField]
    private float speed = 0;
    public float Speed
    {
        get
        {
            return speed;
        }

        set
        {
            speed = value;
            if (speed <= 0)
                speed = 0.01f;
        }
    }

    void Start () {
		
	}
	
	void FixedUpdate () {
        transform.Translate((transform.rotation * transform.forward) * Speed * Time.fixedDeltaTime);
	}
}
