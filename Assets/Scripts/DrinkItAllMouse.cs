using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrinkItAllMouse : MonoBehaviour {

    public Bounds face;
    public Transform drinkPoint;

    private float acceleration;
    private float maxSpeed;
    private float randomForce;
    private float sipAmount;

    private Vector3 oldMousePosition;
    private Vector3 velocity = Vector3.zero;

    private Beer beer;
    
	void Start ()
    {
        beer = FindObjectOfType<Beer>();

        maxSpeed = GameManager.GetMinigameSetting("DrinkItAll", "MaxSpeed");
        acceleration = GameManager.GetMinigameSetting("DrinkItAll", "Acceleration");
        randomForce = GameManager.GetMinigameSetting("DrinkItAll", "RandomForce");
        sipAmount = GameManager.GetMinigameSetting("DrinkItAll", "SipAmount");

        oldMousePosition = Input.mousePosition;
    }

    private void Update()
    {
        if(beer.beerLevel <= 0)
        {
            FindObjectOfType<MinigameManager>().Win();
            this.enabled = false;
        }

        move();

        if (Input.GetMouseButtonDown(0))
        {
            if (face.Contains(drinkPoint.position))
            {
                beer.DrinkBeer(1 / sipAmount);
            }
            else
            {
                FindObjectOfType<MinigameManager>().Lose();
                this.enabled = false;
            }
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
