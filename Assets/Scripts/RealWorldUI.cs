using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealWorldUI : MonoBehaviour {

    public Vector2 onScreenPosition;
    public Camera uiCamera;
    
	void Start ()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        Vector3 readOnScreenPosition = new Vector3(onScreenPosition.x * screenWidth, onScreenPosition.y * screenHeight, 1);
        Vector3 worldPosition = uiCamera.ScreenToWorldPoint(readOnScreenPosition);
        transform.position = worldPosition;
    }
}
