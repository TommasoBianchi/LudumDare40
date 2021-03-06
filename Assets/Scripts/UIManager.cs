﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    public Beer beerSlider;
    public Transform healthBar;
    public TextMesh drunkTagText;
    public TextMesh[] keyBindings;
    public GameObject gameOverOverlay;
    public float gameOverOverlayDuration;
    public Player player;

    private float gameOverFinishTime;

    private string[] drunkTags;
    
	void Start ()
    {
        drunkTags = JSONManager.Load<string[]>("DrunkTags");
        updateDrunknessIndicator();
        updateHealthIndicator();
    }

    public void UpdateKeyBindings(KeyCode[] keyCodes)
    {
        for (int i = 0; i < keyCodes.Length; i++)
        {
            keyBindings[i].text = keyCodes[i].ToString();
        }
    }

    public void PlayGameOverAnimation()
    {
        if (gameOverOverlay.activeSelf == false)
        {
            gameOverOverlay.SetActive(true);
            gameOverFinishTime = Time.time + gameOverOverlayDuration;
            StartCoroutine(gameOver());
        }
    }

    private IEnumerator gameOver()
    {
        while(Time.time < gameOverFinishTime)
        {
            yield return null;
        }

        GameManager.GameOver();
    }

    private void updateDrunknessIndicator()
    {
        float drunkLevel = GameManager.RawDrunkLevel;
        int drunkTagLevel = Mathf.FloorToInt(drunkLevel / 0.25f);
        float beerAmount = drunkLevel / 0.25f - drunkTagLevel;

        beerSlider.SetBeerLevel(beerAmount);
        if (drunkTagLevel < drunkTags.Length)
        {
            drunkTagText.text = drunkTags[drunkTagLevel];
        }
        else
        {
            drunkTagText.text = drunkTags[drunkTags.Length - 1];
        }
    }

    private void updateHealthIndicator()
    {
        float health = GameManager.Health;
        Vector3 scale = healthBar.localScale;
        scale.y = health;
        healthBar.localScale = scale;

        if(health <= 0)
        {
            player.Die();
        }
    }
}
