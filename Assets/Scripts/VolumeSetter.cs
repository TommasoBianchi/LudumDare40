using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSetter : MonoBehaviour
{

    private System.Action<float> action;

    void Start()
    {
        GetComponent<AudioSource>().volume = GameManager.musicVolume;
        action = (f) =>
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.volume = f;
            }
        };
        GameManager.onMusicVolumeChange += action;
    }

    public void SetVolume(Slider volumeSlider)
    {
        GameManager.SetMusicVolume(volumeSlider.value);
    }

    void OnDestroy()
    {
        GameManager.onMusicVolumeChange -= action;
    }
}
