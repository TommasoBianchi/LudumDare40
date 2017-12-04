using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighscoreUI : MonoBehaviour
{

    public Text pointsText;
    public InputField nameInputField;

    private int points;

    void Start()
    {
        points = Mathf.FloorToInt(GameManager.RawDrunkLevel * 1000);
        pointsText.text = points.ToString();
    }

    public void SaveHighscore()
    {
        GameManager.SaveHighscore(nameInputField.text, points);
    }
}
