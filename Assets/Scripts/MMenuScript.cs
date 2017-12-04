using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MMenuScript : MonoBehaviour {

    public Slider musicSlider;

    void Start()
    {
        musicSlider.value = GameManager.musicVolume;
    }

    public void PlayGame ()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame ()
    {
        Application.Quit();
    }
}
