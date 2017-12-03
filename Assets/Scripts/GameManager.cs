using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class GameManager
{

    static GameManager()
    {
        PlayerSpawnPosition = Vector3.zero;
        minigameSettings = JSONManager.LoadDirectory<Dictionary<string, float[]>>("Minigames");
        barMinigames = JSONManager.Load<Dictionary<string, string>>("BarMinigames");
        SelectNewBar();
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
    public static float Health { get { return playerStats.health; } }
    public static float PoliceLevel { get { return playerStats.policeLevel; } }

    public static Vector3 PlayerSpawnPosition { get; private set; }

    public static void Drink(float amount)
    {
        playerStats.drunkLevel += amount;
    }

    public static void GameOver()
    {
        // Print the highscore

        // Load the main menu
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

    #endregion

    #region Minigames

    private static Dictionary<string, Dictionary<string, float[]>> minigameSettings;

    public static float GetMinigameSetting(string minigameName, string settingKey)
    {
        return minigameSettings[minigameName][settingKey][(int)DrunkLevel];
    }

    #endregion

    #region Bars

    private static Dictionary<string, string> barMinigames;
    public static string SelectedBarName { get; private set; }

    public static string GetMinigame(string barName)
    {
        return barMinigames[barName];
    }

    public static void EnterBar(Bar bar, Transform player)
    {
        PlayerSpawnPosition = player.position;
        SceneManager.LoadScene(barMinigames[bar.name]);
        GameManager.SelectNewBar();
    }

    public static void SelectNewBar()
    {
        List<string> names = new List<string>();

        foreach (string name in barMinigames.Keys)
        {
            if(name != SelectedBarName)
            {
                names.Add(name);
            }
        }

        SelectedBarName = names[Random.Range(0, names.Count)];
    }

    #endregion
}
