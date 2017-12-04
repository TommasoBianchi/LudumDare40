using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxBounder : MonoBehaviour {

    public Bounds bounds;
	
	void Update ()
    {
        if (!bounds.Contains(transform.position))
        {
            transform.position = bounds.ClosestPoint(transform.position);
        }
    }
}
