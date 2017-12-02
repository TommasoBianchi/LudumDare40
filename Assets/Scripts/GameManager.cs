using System;
using UnityEngine;

public static class GameManager {

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
}
