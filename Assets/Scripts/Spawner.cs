using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public Mover[] prefabs;
    public Transform[] spawnPoints;
    public float meanCooldown;
    public float cooldownVariance;
    public float meanSpeed;
    public float speedVariance;

    private float nextSpawnTime;

    private GaussianDistribution speedGaussianDistribution;
    private GaussianDistribution cooldownGaussianDistribution;

    void Start ()
    {
        speedGaussianDistribution = new GaussianDistribution(meanSpeed, speedVariance);
        cooldownGaussianDistribution = new GaussianDistribution(meanCooldown, cooldownVariance);
        nextSpawnTime = Time.time + cooldownGaussianDistribution.Generate();
    }
	
	void Update ()
    {
		if(Time.time > nextSpawnTime)
        {
            // Spawn something
            spawn();

            // Update timer
            nextSpawnTime = Time.time + cooldownGaussianDistribution.Generate();
        }
	}

    private void spawn()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Mover prefab = prefabs[Random.Range(0, prefabs.Length)];
        GameObject go = Instantiate(prefab.gameObject, spawnPoint.position, transform.rotation, transform);
        go.GetComponent<Mover>().Speed = speedGaussianDistribution.Generate();
    }
}
