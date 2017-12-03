using System.Collections.Generic;
using UnityEngine;

public static class GameManager
{

    static GameManager()
    {
        //minigameSettings = new Dictionary<string, Dictionary<string, float[]>>
        //{
        //    { "DrinkItAll", new Dictionary<string, float[]>() },
        //    { "GrabTheBeer", new Dictionary<string, float[]>() },
        //    { "Karaoke", new Dictionary<string, float[]>() },
        //    { "Darts", new Dictionary<string, float[]>() },
        //    { "DontFallDown", new Dictionary<string, float[]>() }
        //};

        //minigameSettings["DrinkItAll"]["SipAmount"] = new float[] { 1, 2, 3, 4, 5 };

        //foreach (var item in minigameSettings)
        //{
        //    JSONManager.Save("Minigames/" + item.Key, item.Value);
        //}
        //JSONManager.Save("MinigameSettings", minigameSettings);
        //minigameSettings = JSONManager.Load<Dictionary<string, Dictionary<string, float[]>>>("MinigameSettings");

        minigameSettings = JSONManager.LoadDirectory<Dictionary<string, float[]>>("Minigames");
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

    private static Dictionary<string, Dictionary<string, float[]>> minigameSettings;

    public static float GetMinigameSetting(string minigameName, string settingKey)
    {
        return minigameSettings[minigameName][settingKey][(int)DrunkLevel];
    }

    #endregion
}
