using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchedBeer: MonoBehaviour {

    public float status;
    public GameObject MyBeer;
    public GameObject NotMyBeer;
    private float myBeerChance;
    private float vanishDistance;
    private bool vanished;

    // Use this for initialization
    void Start () {

        myBeerChance = GameManager.GetMinigameSetting("GrabTheBeer", "MyBeer");

        if (status > myBeerChance)
        {
            MyBeer.SetActive(false);
            NotMyBeer.SetActive(true);
        }

        vanishDistance = GameManager.GetMinigameSetting("GrabTheBeer", "Vanish");

	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKey("space"))
        {
            Grab();
        }

        if (gameObject.transform.position.x < -18)
        {
            if (status <= myBeerChance)
            {
                FindObjectOfType<MinigameManager>().Lose();
            }

            Destroy(gameObject);
        }

        if (gameObject.transform.position.x < vanishDistance)
        {
            Vanish();
        }

    }

    private void Grab()
    {
        if ((gameObject.transform.position.x < -8 && gameObject.transform.position.x > -11) && status <= myBeerChance)
        {
            FindObjectOfType<MinigameManager>().Win();
        }

        else if ((gameObject.transform.position.x < -8 && gameObject.transform.position.x > -11) && status > myBeerChance)
        {
            FindObjectOfType<MinigameManager>().Lose();
        }
    }

    private void Vanish ()
    {
        Renderer[] rs = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rs)
        {
            r.enabled = false;
        }
          
        vanished = true;
    }
}
