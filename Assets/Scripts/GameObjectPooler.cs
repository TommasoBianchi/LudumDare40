using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectPooler {

    private GameObject prefab;
    private Vector3 storagePosition;
    private Transform storageParent;
    private Queue<GameObject> pool;

    public GameObjectPooler(GameObject prefab, Vector3 storagePosition, Transform storageParent, int pregenerateCount = 0)
    {
        this.prefab = prefab;
        this.storagePosition = storagePosition;
        this.storageParent = storageParent;
        pool = new Queue<GameObject>();

        for (int i = 0; i < pregenerateCount; i++)
        {
            GameObject go = GameObject.Instantiate(prefab, storagePosition, Quaternion.identity, storageParent);
            go.SetActive(false);
            pool.Enqueue(go);
        }
    }

	public GameObject Instantiate(Vector3 position, Quaternion rotation, Transform parent)
    {
        if(pool.Count == 0)
        {
            pool.Enqueue(GameObject.Instantiate(prefab, storagePosition, Quaternion.identity));
        }

        GameObject go = pool.Dequeue();
        go.transform.position = position;
        go.transform.parent = parent;
        go.transform.rotation = rotation;
        go.SetActive(true);

        return go;
    }

    public void Destroy(GameObject go)
    {
        go.SetActive(false);
        go.transform.position = storagePosition;
        go.transform.parent = storageParent;
        pool.Enqueue(go);
    }
}
