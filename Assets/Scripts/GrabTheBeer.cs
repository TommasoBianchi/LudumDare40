using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabTheBeer : MonoBehaviour {

    public Transform spawnpoint;

    public GameObject beerPrefab;
    public float beerStatusMean;
    public float beerStatusVariance;
    public GameObject Arm;

    private int beerDelay;
    private int status;
    private float spawnBeerTimer;
    private bool beerLaunched;
    

    void Start ()
    {
        spawnBeerTimer = Time.time + 3;
        beerLaunched = false;		
	}
	
	void Update ()
    {
        if (Time.time > spawnBeerTimer)
        {
            SpawnBeer();
        }

        if (Input.GetKeyDown("space"))
        {
            grabNearestBeer();
        }
    }

    private void SpawnBeer ()
    {
        GameObject go = Instantiate(beerPrefab, spawnpoint.position, beerPrefab.transform.rotation, transform);
        LaunchedBeer newBeer = go.GetComponent<LaunchedBeer>();
        newBeer.status = Random.Range(0, 101);

        float myBeerChance = GameManager.GetMinigameSetting("GrabTheBeer", "MyBeer");

        newBeer.Arm = Arm;
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

    private void grabNearestBeer()
    {
        Arm.GetComponent<Animator>().SetTrigger("Grab");

        LaunchedBeer[] allBeers = FindObjectsOfType<LaunchedBeer>();

        if(allBeers.Length == 0)
        {
            return;
        }

        LaunchedBeer nearestBeer = allBeers[0];
        float nearestDistance = (transform.position - nearestBeer.transform.position).sqrMagnitude;
        foreach (LaunchedBeer beer in allBeers)
        {
            float distance = (transform.position - beer.transform.position).sqrMagnitude;
            if(distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestBeer = beer;
            }
        }
        nearestBeer.Grab();
    }
}
