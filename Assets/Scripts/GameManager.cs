using System;
using System.Collections.Generic;
using UnityEngine;

public static class GameManager
{

    static GameManager()
    {
        JSONManager.Save("MinigameSettings", minigameSettings);
        minigameSettings = JSONManager.Load<Dictionary<string, Dictionary<string, float>>>("MinigameSettings");
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

    //public static float DrunkLevel { get { return playerStats.drunkLevel; } }
    public static Drunkness DrunkLevel { get { return (Drunkness)Mathf.Clamp(Mathf.FloorToInt(playerStats.drunkLevel / 0.25f), 0, 4); } }
    public static float Health { get { return playerStats.health; } }
    public static float PoliceLevel { get { return playerStats.policeLevel; } }

    public static void Drink(float amount)
    {
        playerStats.drunkLevel += amount;
        Debug.Log(DrunkLevel);
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

    private static Dictionary<string, Dictionary<string, float>> minigameSettings = new Dictionary<string, Dictionary<string, float>>
    {
        { "DrinkItAll", new Dictionary<string, float>{ { "Speed", 1 } } },
        { "GrabTheBeer", new Dictionary<string, float>{ { "", 0 } } },
        { "Karaoke", new Dictionary<string, float>{ { "", 0 } } },
        { "Darts", new Dictionary<string, float>{ { "", 0 } } },
        { "DontFallDown", new Dictionary<string, float>{ { "", 0 } } }
    };

    public static float GetMinigameSetting(string minigameName, string settingKey)
    {
        return minigameSettings[minigameName][settingKey];
    }

    #endregion
}
