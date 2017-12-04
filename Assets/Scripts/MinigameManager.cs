using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MinigameManager : MonoBehaviour {

    public Image winPanel;
    public Image losePanel;
    public Image startPanel;
    public GameObject countdown;
    public bool freezeTime = true;
    public MonoBehaviour[] scriptsToDisableAtStart;

    private float drunkAmountIfWin = 0.1f;
    private bool hasStarted = false;

    void Start()
    {
        foreach (MonoBehaviour script in scriptsToDisableAtStart)
        {
            script.enabled = false;
        }
    }

    void Update()
    {
        if (!hasStarted && Input.GetKeyDown(KeyCode.Space))
        {
            hasStarted = true;
            startPanel.gameObject.SetActive(false);
            countdown.SetActive(true);
            StartCoroutine(doAfterTime(3, () => startMinigame()));
        }
    }

    private void startMinigame()
    {
        countdown.SetActive(false);
        foreach (MonoBehaviour script in scriptsToDisableAtStart)
        {
            script.enabled = true;
        }
    }

    public void Win()
    {
        if (freezeTime)
        {
            Time.timeScale = 0;
        }
        
        winPanel.gameObject.SetActive(true);
        GameManager.Drink(drunkAmountIfWin);
        StartCoroutine(doAfterTime(2, () => SceneManager.LoadScene("MainScene")));
    }

    public void Lose()
    {
        if (freezeTime)
        {
            Time.timeScale = 0;
        }

        GameManager.TakeDamage(GameManager.GetMinigameSetting("GameLost", "DamageTaken"));
        losePanel.gameObject.SetActive(true);
        StartCoroutine(doAfterTime(2, () => SceneManager.LoadScene("MainScene")));
    }

    private IEnumerator doAfterTime(float time, System.Action callback)
    {
        float endTime = Time.time + time;

        while (Time.time < endTime)
        {
            yield return null;
        }

        callback.Invoke();
    }
}
