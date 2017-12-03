﻿using UnityEngine;
    
public class UIController : MonoBehaviour {

    public GameObject pausePanel;
    public GameObject settingsPanel;
    public int PauseMenuLocation;
    private bool IsPaused;
    

	// Use this for initialization
	void Start () {
        IsPaused = false;
        PauseMenuLocation = 0;
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKeyDown("escape") && !IsPaused)
        {
            Time.timeScale = 0;
            pausePanel.gameObject.SetActive(true);
            IsPaused = true;
            PauseMenuLocation++;
        }

        else if(Input.GetKeyDown("escape") && IsPaused)
        {
            if (PauseMenuLocation == 1)
            {
                pausePanel.gameObject.SetActive(false);
                Time.timeScale = 1;
                PauseMenuLocation--;
                IsPaused = false;
            }

            else if(PauseMenuLocation == 2)
            {
                settingsPanel.gameObject.SetActive(false);
                pausePanel.gameObject.SetActive(true);
                PauseMenuLocation--;
            }
        }
		
	}

    public void PauseLocationUP ()
    {
        PauseMenuLocation++;
    }

    public void PauseLocationDown ()
    {
        PauseMenuLocation--;
    }

    public void Resume ()
    {
        Time.timeScale = 1;
        IsPaused = false;
        PauseMenuLocation = 0;
    }

    /*public void BackMainMenu ()
    {
        Application.LoadLevel(0);
    }*/
}
