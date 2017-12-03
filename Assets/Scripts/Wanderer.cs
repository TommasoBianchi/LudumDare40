using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wanderer : MonoBehaviour {

    public float meanSpeed;

    private float speed;
    private float turningAngle;
    
	void Start ()
    {
        speed = GaussianDistribution.Generate(meanSpeed, 0);
	}
	
	void Update ()
    {
        if(Time.frameCount % 50 == 0)
        {
            turningAngle = GaussianDistribution.Generate(0, 1);
        }

        transform.Rotate(0, turningAngle, 0, Space.Self);
        transform.Translate(transform.forward * speed * Time.deltaTime, Space.World);
	}
}
