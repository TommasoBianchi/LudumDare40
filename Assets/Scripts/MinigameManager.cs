using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameManager : MonoBehaviour {

    public Image winPanel;
    public Image losePanel;

    public void Win()
    {
        Time.timeScale = 0;
        winPanel.gameObject.SetActive(true);
        StartCoroutine(returnToMainScene());
    }

    public void Lose()
    {
        Time.timeScale = 0;
        losePanel.gameObject.SetActive(true);
        StartCoroutine(returnToMainScene());
    }

    IEnumerator returnToMainScene()
    {
        for (int i = 0; i < 1000; i++)
        {
            yield return null;
        }

        Debug.Log("Return to main scene");
    }
}
