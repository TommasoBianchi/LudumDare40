using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Darts : MonoBehaviour {

    public WaypointWanderer target;
    public WaypointWanderer aim;
    public Animator handAnimator;
    public LayerMask targetLayerMask;
    
    void Start ()
    {
		
	}
	
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Shoot!
            handAnimator.SetTrigger("Throw");
            target.speed = 0;
            aim.speed = 0;

            if(Physics.Raycast(aim.transform.position, Vector3.back, 20, targetLayerMask))
            {
                FindObjectOfType<MinigameManager>().Win();
            }
            else
            {
                FindObjectOfType<MinigameManager>().Lose();
            }
        }
	}
}
