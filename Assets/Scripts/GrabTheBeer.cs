using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabTheBeer : MonoBehaviour {

    public Transform spawnpoint;

    public GameObject beerPrefab;
    public float beerStatusMean;
    public float beerStatusVariance;

    private int beerDelay;
    private int status;
    private float spawnBeerTimer;
    private bool beerLaunched;


	void Start () {

        spawnBeerTimer = Time.time + 3;
        beerLaunched = false;
		
	}
	
	
	void Update () {

        if (Time.time > spawnBeerTimer)
        {
            SpawnBeer();

        }

    }

    private void SpawnBeer ()
    {
        GameObject go = Instantiate(beerPrefab, spawnpoint.position, beerPrefab.transform.rotation, transform);
        LaunchedBeer newBeer = go.GetComponent<LaunchedBeer>();
        newBeer.status = Random.Range(0, 101);

        float myBeerChance = GameManager.GetMinigameSetting("GrabTheBeer", "MyBeer");
        
        newBeer.GetComponent<Mover>().Speed = GameManager.GetMinigameSetting("GrabTheBeer", "Speed");

        if (newBeer.status > myBeerChance)
        {
            float randomCooldown = GaussianDistribution.Generate(beerStatusMean, beerStatusVariance);
            randomCooldown = Mathf.Clamp(randomCooldown, 3f, float.MaxValue);
            spawnBeerTimer = Time.time + randomCooldown;
        }

        else
        {
            spawnBeerTimer = float.MaxValue;
        }
    }
}
