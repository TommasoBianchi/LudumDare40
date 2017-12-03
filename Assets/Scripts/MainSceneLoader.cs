using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainSceneLoader : MonoBehaviour {

    public Image loadingOverlay;

    private Player player;
    private float targetTimeToStart;
    private float delayInSeconds;

    void Start ()
    {
        player = FindObjectOfType<Player>();
        player.gameObject.SetActive(false);

        #if UNITY_WEBGL
        Time.timeScale = 8;
        delayInSeconds = 4;
        #else
        Time.timeScale = 50;
        delayInSeconds = 1;
        #endif

        targetTimeToStart = Time.realtimeSinceStartup + delayInSeconds;
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
