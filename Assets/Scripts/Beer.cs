using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beer : MonoBehaviour {

    public Transform beer;
    
    public float beerLevel { get; private set; }

    private void Start()
    {
        beerLevel = beer.localScale.y;
    }

    public void DrinkBeer(float amount)
    {
        beerLevel -= amount;

        beer.localScale = new Vector3(1, beerLevel, 1);
    }

    public void SetBeerLevel(float level)
    {
        beerLevel = level;

        beer.localScale = new Vector3(1, beerLevel, 1);
    }
}
