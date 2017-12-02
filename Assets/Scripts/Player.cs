using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    private float speed = 10;

    private KeyCode upKey = KeyCode.W;
    private KeyCode downKey = KeyCode.S;
    private KeyCode rightKey = KeyCode.A;
    private KeyCode leftKey = KeyCode.D;

    private float nextKeyRandomizationTime;

    private Dictionary<GameManager.Drunkness, DrunkMalus> drunkMaluses;
    
	void Start ()
    {
        drunkMaluses = new Dictionary<GameManager.Drunkness, DrunkMalus>();
        drunkMaluses.Add(GameManager.Drunkness.None, new DrunkMalus(10, 0, float.MaxValue, 0));
        drunkMaluses.Add(GameManager.Drunkness.Low, new DrunkMalus(9, 1, 12, 4));
        drunkMaluses.Add(GameManager.Drunkness.Medium, new DrunkMalus(8, 2, 9, 3));
        drunkMaluses.Add(GameManager.Drunkness.High, new DrunkMalus(7, 3, 6, 2));
        drunkMaluses.Add(GameManager.Drunkness.Dead, new DrunkMalus(6, 4, 3, 1));

        DrunkMalus currentDrunkMalus = drunkMaluses[GameManager.DrunkLevel];
        nextKeyRandomizationTime = Time.time
                + GaussianDistribution.Generate(currentDrunkMalus.keyRandomizationMeanTime, currentDrunkMalus.keyRandomizationVarianceTime);
    }
	
	void FixedUpdate ()
    {
        if(Time.time > nextKeyRandomizationTime)
        {
            randomizeKeys();
            DrunkMalus currentDrunkMalus = drunkMaluses[GameManager.DrunkLevel];
            nextKeyRandomizationTime = Time.time
                + GaussianDistribution.Generate(currentDrunkMalus.keyRandomizationMeanTime, currentDrunkMalus.keyRandomizationVarianceTime);
        }

        move();

        GameManager.Drink(0.001f);
	}

    private void move()
    {
        Vector3 direction = Vector3.zero;
        if (Input.GetKey(upKey)) direction.z++;
        if (Input.GetKey(downKey)) direction.z--;
        if (Input.GetKey(leftKey)) direction.x++;
        if (Input.GetKey(rightKey)) direction.x--;

        DrunkMalus currentDrunkMalus = drunkMaluses[GameManager.DrunkLevel];
        float speed = GaussianDistribution.Generate(currentDrunkMalus.speedMean, currentDrunkMalus.speedVariance);
        transform.Translate(direction.normalized * speed * Time.fixedDeltaTime);
    }

    private void randomizeKeys()
    {
        KeyCode[] keyCodes = { upKey, downKey, rightKey, leftKey };

        for (int i = keyCodes.Length - 1; i >= 0; i--)
        {
            int j = Random.Range(0, i + 1);
            KeyCode tmp = keyCodes[i];
            keyCodes[i] = keyCodes[j];
            keyCodes[j] = tmp;
        }

        upKey = keyCodes[0];
        downKey = keyCodes[1];
        rightKey = keyCodes[2];
        leftKey = keyCodes[3];
    }

    private struct DrunkMalus
    {
        public float speedMean;
        public float speedVariance;
        public float keyRandomizationMeanTime;
        public float keyRandomizationVarianceTime;

        public DrunkMalus(float speedMean, float speedVariance, float keyRandomizationMeanTime, float keyRandomizationVarianceTime)
        {
            this.speedMean = speedMean;
            this.speedVariance = speedVariance;
            this.keyRandomizationMeanTime = keyRandomizationMeanTime;
            this.keyRandomizationVarianceTime = keyRandomizationVarianceTime;
        }
    }
}
