﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public static class GameManager
{

    static GameManager()
    {
        PlayerSpawnPosition = Vector3.zero;
        minigameSettings = JSONManager.LoadDirectory<Dictionary<string, float[]>>("Minigames");
        highscores = JSONManager.Load<List<Highscore>>("Highscores");

        minigames = JSONManager.Load<string[]>("Minigames");
        bars = JSONManager.Load<string[]>("Bars");
        string[] minigamesToAssignArray = minigames.Clone() as string[];
        for (int i = minigames.Length - 1; i >= 0; i--)
        {
            int j = Random.Range(0, i + 1);
            string tmp = minigamesToAssignArray[j];
            minigamesToAssignArray[j] = minigamesToAssignArray[i];
            minigamesToAssignArray[i] = tmp;
        }
        minigamesToAssign = minigamesToAssignArray.ToList();

        SelectNewBar();

        musicVolume = 1;
    }

    #region Player

    public enum Drunkness
    {
        None,
        Low,
        Medium,
        High,
        Dead
    }

    private static PlayerStats playerStats = new PlayerStats(0, 1, 0);

    public static float RawDrunkLevel { get { return playerStats.drunkLevel; } }
    public static Drunkness DrunkLevel { get { return (Drunkness)Mathf.Clamp(Mathf.FloorToInt(playerStats.drunkLevel / 0.25f), 0, 4); } }
    public static float Health { get { return Mathf.Clamp(playerStats.health, 0, 1); } }
    public static float PoliceLevel { get { return playerStats.policeLevel; } }

    public static Vector3 PlayerSpawnPosition { get; private set; }

    public static List<Highscore> highscores { get; private set; }

    public static void Drink(float amount)
    {
        playerStats.drunkLevel += amount;
    }

    public static void TakeDamage(float amount)
    {
        playerStats.health -= amount;
    }

    public static void GameOver()
    {        
        // Load the highscore scene
        SceneManager.LoadScene("Highscore");
    }

    public static void SaveHighscore(string name, int points)
    {
        // Print the highscore
        highscores.Add(new Highscore(points, name));
        highscores.Sort(new HighscoreComparer());
        JSONManager.Save("Highscores", highscores);

        // Reset variables
        PlayerSpawnPosition = Vector3.zero;
        playerStats = new PlayerStats(0, 1, 0);

        // Load main menu
        SceneManager.LoadScene("MainMenu");
    }

    private struct PlayerStats
    {
        public float drunkLevel;
        public float health;
        public float policeLevel;

        public PlayerStats(float drunkLevel, float health, float policeLevel)
        {
            this.drunkLevel = drunkLevel;
            this.health = health;
            this.policeLevel = policeLevel;
        }
    }

    public struct Highscore
    {
        public int points;
        public string name;

        public Highscore(int points, string name)
        {
            this.points = points;
            this.name = name;
        }
    }

    public class HighscoreComparer : IComparer<Highscore>
    {
        public int Compare(Highscore x, Highscore y)
        {
            return y.points - x.points;
        }
    }

    #endregion

    #region Minigames

private static Dictionary<string, Dictionary<string, float[]>> minigameSettings;

    public static float GetMinigameSetting(string minigameName, string settingKey)
    {
        return minigameSettings[minigameName][settingKey][(int)DrunkLevel];
    }

    #endregion

    #region Bars

    private static string[] minigames;
    private static string[] bars;   // TODO: manage bar names better
    private static List<string> minigamesToAssign;
    private static Dictionary<string, string> barToMinigamesDictionary = new Dictionary<string, string>();
    public static string SelectedBarName { get; private set; }

    private static string getMinigame()
    {
        if(minigamesToAssign.Count > 0)
        {
            string minigame = minigamesToAssign[0];
            minigamesToAssign.RemoveAt(0);
            return minigame;
        }
        else
        {
            return minigames[Random.Range(0, minigames.Length)];
        }
    }

    public static void EnterBar(Bar bar, Transform player)
    {
        if(barToMinigamesDictionary.ContainsKey(bar.name) == false)
        {
            barToMinigamesDictionary.Add(bar.name, getMinigame());
        }
        PlayerSpawnPosition = player.position;
        SceneManager.LoadScene(barToMinigamesDictionary[bar.name]);
        GameManager.SelectNewBar();
    }

    public static void SelectNewBar()
    {
        List<string> names = new List<string>();

        foreach (string name in bars)
        {
            if(name != SelectedBarName)
            {
                names.Add(name);
            }
        }

        SelectedBarName = names[Random.Range(0, names.Count)];
    }

    #endregion

    public static float musicVolume { get; private set; }
    public static event System.Action<float> onMusicVolumeChange;

    public static void SetMusicVolume(float amount)
    {
        musicVolume = amount;
        if (onMusicVolumeChange != null)
        {
            onMusicVolumeChange.Invoke(musicVolume);
        }
    }
}
