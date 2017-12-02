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
    public float maxWalkedDistance;

    private float nextSpawnTime;

    private GaussianDistribution speedGaussianDistribution;
    private GaussianDistribution cooldownGaussianDistribution;

    private GameObjectPooler[] gameObjectPools;

    void Start ()
    {
        speedGaussianDistribution = new GaussianDistribution(meanSpeed, speedVariance);
        cooldownGaussianDistribution = new GaussianDistribution(meanCooldown, cooldownVariance);
        nextSpawnTime = Time.time + cooldownGaussianDistribution.Generate();

        gameObjectPools = new GameObjectPooler[prefabs.Length];
        for (int i = 0; i < prefabs.Length; i++)
        {
            Transform poolParent = new GameObject(prefabs[i].name + " pool").transform;
            poolParent.parent = transform;
            gameObjectPools[i] = new GameObjectPooler(prefabs[i].gameObject, Vector3.one * 100, poolParent, 20);
        }
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
        GameObjectPooler pool = gameObjectPools[Random.Range(0, gameObjectPools.Length)];
        GameObject go = pool.Instantiate(spawnPoint.position, transform.rotation, transform);
        go.GetComponent<Mover>().Speed = speedGaussianDistribution.Generate();

        DestroyAfterDistance dad = go.GetComponent<DestroyAfterDistance>();
        if(dad == null)
        {
            dad = go.AddComponent<DestroyAfterDistance>();
        }
        dad.distance = maxWalkedDistance;
        dad.onDestroy = g => pool.Destroy(g);
    }
}
