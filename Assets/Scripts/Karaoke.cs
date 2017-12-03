using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Karaoke : MonoBehaviour
{

    public TextMesh[] textMeshes;
    public Transform[] spawnPoints;

    public GameObject notePrefab;

    public Text catchedNotesText;
    public Text missedNotesText;
    public Text pointsText;
    public Text targetPointsText;

    private Note lastSpawnedNote;
    private int songLength;
    private int spawnedNotesAmount;
    private float nextNoteSpawnTime;
    private float noteSpawnCooldownMean;
    private float noteSpawnCooldownVariance;

    private int correctNotePoints = 10;
    private int missclickMalusPoints = 2;

    public KeyCode[] keyCodes { get; private set; }

    private List<KeyCode>[] keyCodeClusters = new List<KeyCode>[] {
        new List<KeyCode>(){ KeyCode.Q, KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.Z, KeyCode.X },
        new List<KeyCode>(){ KeyCode.E, KeyCode.R, KeyCode.D, KeyCode.F, KeyCode.C, KeyCode.V },
        new List<KeyCode>(){ KeyCode.T, KeyCode.Y, KeyCode.U, KeyCode.G, KeyCode.H, KeyCode.B, KeyCode.N },
        new List<KeyCode>(){ KeyCode.O, KeyCode.P, KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.I, KeyCode.J }
    };

    void Start()
    {
        songLength = Mathf.FloorToInt(Mathf.Clamp(GaussianDistribution.Generate(
            GameManager.GetMinigameSetting("Karaoke", "SongLengthMean"), 
            GameManager.GetMinigameSetting("Karaoke", "SongLengthVariance")), 5, float.MaxValue));
        spawnedNotesAmount = 0;
        noteSpawnCooldownMean = GameManager.GetMinigameSetting("Karaoke", "NoteSpawnCooldownMean");
        noteSpawnCooldownVariance = GameManager.GetMinigameSetting("Karaoke", "NoteSpawnCooldownVariance");
        nextNoteSpawnTime = Time.time + 3;
        targetPointsText.text = (GameManager.GetMinigameSetting("Karaoke", "TargetPercentage") * correctNotePoints * songLength).ToString();

        randomizeKeys();
    }

    private void randomizeKeys()
    {
        int keyClusters = (int)GameManager.GetMinigameSetting("Karaoke", "KeyClusters");
        int[] clusterIndices = new int[keyCodeClusters.Length];
        for (int i = 0; i < clusterIndices.Length; i++)
        {
            clusterIndices[i] = i;
        }
        for (int i = clusterIndices.Length - 1; i >= 0; i--)
        {
            int j = Random.Range(0, i + 1);
            int tmp = clusterIndices[i];
            clusterIndices[i] = clusterIndices[j];
            clusterIndices[j] = tmp;
        }
        int[] chosenKeyClusters = new int[keyClusters];
        for (int i = 0; i < chosenKeyClusters.Length; i++)
        {
            chosenKeyClusters[i] = clusterIndices[i];
        }

        keyCodes = new KeyCode[textMeshes.Length];
        int index = 0;
        foreach (int i in chosenKeyClusters)
        {
            keyCodes[index] = keyCodeClusters[i][Random.Range(0, keyCodeClusters[i].Count)];
            textMeshes[index].text = keyCodes[index].ToString();
            index++;
        }
        while (index < keyCodes.Length)
        {
            int randomCluster = chosenKeyClusters[Random.Range(0, chosenKeyClusters.Length)];
            KeyCode keyCode = keyCodeClusters[randomCluster][Random.Range(0, keyCodeClusters[randomCluster].Count)];

            if (keyCodes.Contains(keyCode))
            {
                continue;
            }

            keyCodes[index] = keyCode;
            textMeshes[index].text = keyCode.ToString();
            index++;
        }
    }

    void Update()
    {
        if (Time.time > nextNoteSpawnTime)
        {
            spawnNote();
        }
    }

    public void NoteCatched()
    {
        catchedNotesText.text = (int.Parse(catchedNotesText.text) + 1).ToString();
        pointsText.text = (int.Parse(pointsText.text) + (correctNotePoints + missclickMalusPoints)).ToString();
    }

    public void WrongClick()
    {
        pointsText.text = (int.Parse(pointsText.text) - missclickMalusPoints).ToString();
    }

    public void MissedNote()
    {
        missedNotesText.text = (int.Parse(missedNotesText.text) + 1).ToString();
        pointsText.text = (int.Parse(pointsText.text) - correctNotePoints).ToString();
    }

    private void spawnNote()
    {
        int position = Random.Range(0, spawnPoints.Length);
        GameObject go = Instantiate(notePrefab, spawnPoints[position].position, notePrefab.transform.rotation, transform);
        Note newNote = go.GetComponent<Note>();
        newNote.keyCode = keyCodes[position];
        newNote.karaoke = this;
        if (lastSpawnedNote != null)
        {
            lastSpawnedNote.nextNote = newNote;
        }
        lastSpawnedNote = newNote;
        spawnedNotesAmount++;

        if (spawnedNotesAmount < songLength)
        {
            float randomCooldown = GaussianDistribution.Generate(noteSpawnCooldownMean, noteSpawnCooldownVariance);
            randomCooldown = Mathf.Clamp(randomCooldown, 0.15f, float.MaxValue);
            nextNoteSpawnTime = Time.time + randomCooldown;
        }
        else
        {
            nextNoteSpawnTime = float.MaxValue;
        }
    }
}
