using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointWanderer : MonoBehaviour {

    public float speed;
    public Bounds bounds;
    public bool rotateAroundY;

    private Vector3 waypoint;
    
	void Start ()
    {
        updateWaypoint();
    }
	
	void Update ()
    {
        if((waypoint - transform.position).sqrMagnitude < 0.01f)
        {
            updateWaypoint();
        }

        Vector3 direction = waypoint - transform.position;

        transform.Translate(direction.normalized * speed * Time.deltaTime);

        if (rotateAroundY)
        {
            Vector3 lookPosition = waypoint;
            lookPosition.y = transform.position.y;
            transform.LookAt(lookPosition);
        }
	}

    private void updateWaypoint()
    {
        waypoint.x = Random.Range(bounds.min.x, bounds.max.x);
        waypoint.y = Random.Range(bounds.min.y, bounds.max.y);
        waypoint.z = Random.Range(bounds.min.z, bounds.max.z);
    }
}
