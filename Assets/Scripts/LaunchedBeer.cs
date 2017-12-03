using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchedBeer: MonoBehaviour {

    public float status;
    public GameObject MyBeer;
    public GameObject NotMyBeer;
    public GameObject Arm;
    private float myBeerChance;
    private float vanishDistance;
    private bool vanished;
    
    void Start ()
    {
        myBeerChance = GameManager.GetMinigameSetting("GrabTheBeer", "MyBeer");

        vanished = false;

        if (status > myBeerChance)
        {
            MyBeer.SetActive(false);
            NotMyBeer.SetActive(true);
        }

        vanishDistance = GameManager.GetMinigameSetting("GrabTheBeer", "Vanish");
	}
	
	void Update ()
    {
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

    public void Grab()
    {
        if ((gameObject.transform.position.x <= -6.5 && gameObject.transform.position.x >= -10.5) && status <= myBeerChance)
        {
            if (vanished)
            {
                Renderer[] rs = GetComponentsInChildren<Renderer>();
                foreach (Renderer r in rs)
                {
                    r.enabled = true;
                }
            }
            FindObjectOfType<MinigameManager>().Win();
            gameObject.GetComponent<Mover>().enabled = false;
        }

        else if ((gameObject.transform.position.x <= -6.5 && gameObject.transform.position.x >= -10.5) && status > myBeerChance)
        {
            if (vanished)
            {
                Renderer[] rs = GetComponentsInChildren<Renderer>();
                foreach (Renderer r in rs)
                {
                    r.enabled = true;
                }
            }
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
