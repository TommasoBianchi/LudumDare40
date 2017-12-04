using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscorePanel : MonoBehaviour {

    public GameObject prefab;
    
	void Start ()
    {
        List<GameManager.Highscore> highscores = GameManager.highscores;

        foreach (var highscore in highscores)
        {
            GameObject go = Instantiate(prefab, transform);
            Text[] texts = go.GetComponentsInChildren<Text>();
            texts[0].text = highscore.name;
            texts[1].text = highscore.points.ToString();
        }
	}
}
