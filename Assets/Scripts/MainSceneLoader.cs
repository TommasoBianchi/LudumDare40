using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneLoader : MonoBehaviour {

    public float delayInSeconds = 1;
    public Image loadingOverlay;

    private Player player;
    private float targetTimeToStart;
    
	void Start ()
    {
        player = FindObjectOfType<Player>();
        player.gameObject.SetActive(false);
        targetTimeToStart = Time.realtimeSinceStartup + delayInSeconds;
        Time.timeScale = 50;
	}
	
	void Update ()
    {
		if(Time.realtimeSinceStartup > targetTimeToStart)
        {
            Time.timeScale = 1;
            player.gameObject.SetActive(true);
            loadingOverlay.gameObject.SetActive(false);
            Destroy(gameObject);
        }
	}
}
