using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MinigameManager : MonoBehaviour {

    public Image winPanel;
    public Image losePanel;

    private float drunkAmountIfWin = 0.1f;

    public void Win()
    {
        Time.timeScale = 0;
        winPanel.gameObject.SetActive(true);
        GameManager.Drink(drunkAmountIfWin);
        GameManager.SelectNewBar();
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
        for (int i = 0; i < 400; i++)
        {
            yield return null;
        }
        
        SceneManager.LoadScene("MainScene");
    }
}
