using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beer : MonoBehaviour {

    public Transform beer;
    public Transform glass;

    public float beerLevel;

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            drinkBeer(0.001f);
        }
    }

    void drinkBeer(float amount)
    {
        beerLevel -= amount;

        beer.transform.localScale = new Vector3(1, beerLevel, 1);
        beer.transform.Translate(0, -0.5f * amount, 0);

        glass.transform.localScale = new Vector3(1, 1 - beerLevel, 1);
        glass.transform.Translate(0, -0.4f * amount, 0);

        if (beerLevel <= 0)
        {

        }
    }
}
