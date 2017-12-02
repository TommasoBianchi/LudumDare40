using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollower : MonoBehaviour {

    private float acceleration;
    private float maxSpeed;
    private Vector3 oldMousePosition;
    private Vector3 velocity = Vector3.zero;
    
	void Start ()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.transform.position.z - transform.position.z;
        Vector3 startingPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        startingPosition.z = transform.position.z;
        transform.position = startingPosition;
        maxSpeed = 150;//GameManager.GetMinigameSetting("DrinkItAll", "Speed");
        acceleration = 150;

        oldMousePosition = Input.mousePosition;

        // Test
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        if(maxSpeed == 0)
        {
            smoothFollow();
        }
        else
        {
            fastFollow();
        }

        oldMousePosition = Input.mousePosition;
    }

    private void smoothFollow ()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.transform.position.z - transform.position.z;
        Vector3 delta = Camera.main.ScreenToWorldPoint(mousePosition) - transform.position;
        delta.z = 0;
        transform.Translate(delta);
    }

    private void fastFollow()
    {
        Vector3 delta = Input.mousePosition - oldMousePosition;
        delta.z = 0;
        delta.x = -delta.x;

        if(delta.sqrMagnitude > Mathf.Epsilon)
        {
            velocity += delta.normalized * acceleration * Time.deltaTime;
        }
        else
        {
            //velocity -= velocity.normalized * acceleration / 10 * Time.deltaTime;
        }
        Vector3.ClampMagnitude(velocity, maxSpeed);

        Vector3 nextPosition = transform.position + velocity * Time.deltaTime;
        Vector3 nextPositionOnScreen = Camera.main.WorldToScreenPoint(nextPosition);
        if (nextPositionOnScreen.x > 0 && nextPositionOnScreen.x < Screen.width &&
           nextPositionOnScreen.y > 0 && nextPositionOnScreen.y < Screen.height)
        {
            transform.position = nextPosition;
        }
    }
}
