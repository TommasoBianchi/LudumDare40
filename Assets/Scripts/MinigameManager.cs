using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MinigameManager : MonoBehaviour {

    public Image winPanel;
    public Image losePanel;
    public bool freezeTime = true;

    private float drunkAmountIfWin = 0.1f;

    public void Win()
    {
        if (freezeTime)
        {
            Time.timeScale = 0;
        }
        
        winPanel.gameObject.SetActive(true);
        GameManager.Drink(drunkAmountIfWin);
        StartCoroutine(returnToMainScene());
    }

    public void Lose()
    {
        if (freezeTime)
        {
            Time.timeScale = 0;
        }

        losePanel.gameObject.SetActive(true);
        StartCoroutine(returnToMainScene());
    }

    IEnumerator returnToMainScene()
    {
        for (int i = 0; i < 100; i++)
        {
            yield return null;
        }
        
        SceneManager.LoadScene("MainScene");
    }
}
