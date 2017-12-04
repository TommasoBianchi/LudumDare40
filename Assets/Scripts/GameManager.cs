using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public static class GameManager
{

    static GameManager()
    {
        PlayerSpawnPosition = Vector3.zero;
        minigameSettings = JSONManager.LoadDirectory<Dictionary<string, float[]>>("Minigames");
        barMinigames = JSONManager.Load<Dictionary<string, string>>("BarMinigames");
        highscores = JSONManager.Load<List<Highscore>>("Highscores");
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
        // Print the highscore
        highscores.Add(new Highscore(Mathf.FloorToInt(RawDrunkLevel * 1000), "name", "tag"));
        highscores.Sort(new HighscoreComparer());
        JSONManager.Save("Highscores", highscores);

        // Load the main menu
        SceneManager.LoadScene("MainMenu");

        PlayerSpawnPosition = Vector3.zero;
        playerStats = new PlayerStats(0, 1, 0);
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
        public string tag;

        public Highscore(int points, string name, string tag)
        {
            this.points = points;
            this.name = name;
            this.tag = tag;
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
