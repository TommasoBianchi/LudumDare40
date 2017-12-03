using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollower : MonoBehaviour {

    private float acceleration;
    private float maxSpeed;
    private float randomForce;
    private float sipAmount;

    private Vector3 oldMousePosition;
    private Vector3 velocity = Vector3.zero;

    private Beer beer;
    
	void Start ()
    {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = Camera.main.transform.position.z - transform.position.z;
        Vector3 startingPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        startingPosition.z = transform.position.z;
        transform.position = startingPosition;

        beer = FindObjectOfType<Beer>();

        maxSpeed = 150;//GameManager.GetMinigameSetting("DrinkItAll", "Speed");
        acceleration = 50;
        randomForce = 80;
        sipAmount = 3;

        oldMousePosition = Input.mousePosition;
    }

    private void Update()
    {
        move();

        if (Input.GetMouseButtonDown(0))
        {
            beer.DrinkBeer(1 / sipAmount);
        }
    }

    private void move()
    {
        if (maxSpeed == 0)
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
            Vector3 deltaRandom = new Vector3(Mathf.Sin(Time.frameCount), Mathf.Cos(Time.frameCount), 0);
            velocity += deltaRandom * randomForce * Time.deltaTime;
        }
        Vector3.ClampMagnitude(velocity, maxSpeed);

        Vector3 nextPosition = transform.position + velocity * Time.deltaTime;
        Vector3 nextPositionOnScreen = Camera.main.WorldToScreenPoint(nextPosition);
        if (nextPositionOnScreen.x > 0 && nextPositionOnScreen.x < Screen.width &&
           nextPositionOnScreen.y > 0 && nextPositionOnScreen.y < Screen.height)
        {
            transform.position = nextPosition;
        }
        else
        {
            velocity = Vector3.zero;
        }
    }
}
