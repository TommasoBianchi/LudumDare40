using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeSetter : MonoBehaviour
{

    void Start()
    {
        GetComponent<AudioSource>().volume = GameManager.musicVolume;
        GameManager.onMusicVolumeChange += (f) =>
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource != null)
            {
                audioSource.volume = f;
            }
        };
    }

    public void SetVolume(Slider volumeSlider)
    {
        GameManager.SetMusicVolume(volumeSlider.value);
    }
}
