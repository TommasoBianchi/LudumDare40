using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour {

    public Beer beerSlider;
    public Transform healthBar;
    public TextMesh drunkTagText;

    private string[] drunkTags;
    
	void Start ()
    {
        drunkTags = JSONManager.Load<string[]>("DrunkTags");
        updateDrunknessIndicator();
        updateHealthIndicator();
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
    }
}
