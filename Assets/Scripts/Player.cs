using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public Bounds bounds;

    private Vector3 velocity = Vector3.zero;
    private float acceleration = 25;
    private float maxSpeed;
    private float driftAngle;
    private float driftSpeed = 0.5f;
    private float maxDriftAngle = 30;

    private KeyCode upKey = KeyCode.W;
    private KeyCode downKey = KeyCode.S;
    private KeyCode rightKey = KeyCode.A;
    private KeyCode leftKey = KeyCode.D;

    private float nextMaxSpeedRandomizationTime = 0;
    private float nextKeyRandomizationTime;

    private Dictionary<GameManager.Drunkness, DrunkMalus> drunkMaluses;
    
	void Start ()
    {
        drunkMaluses = new Dictionary<GameManager.Drunkness, DrunkMalus>();
        drunkMaluses.Add(GameManager.Drunkness.None, new DrunkMalus(10, 0, float.MaxValue, 0));
        drunkMaluses.Add(GameManager.Drunkness.Low, new DrunkMalus(9, 1, 12, 4));
        drunkMaluses.Add(GameManager.Drunkness.Medium, new DrunkMalus(8, 2, 9, 3));
        drunkMaluses.Add(GameManager.Drunkness.High, new DrunkMalus(7, 3, 6, 2));
        drunkMaluses.Add(GameManager.Drunkness.Dead, new DrunkMalus(6, 8, 3, 1));

        JSONManager.Save("DrunkMaluses", drunkMaluses);
        drunkMaluses = JSONManager.Load<Dictionary<GameManager.Drunkness, DrunkMalus>>("DrunkMaluses");

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
        respectBounds();

        //GameManager.Drink(0.001f);
    }

    private void move()
    {
        Vector3 direction = Vector3.zero;
        if (Input.GetKey(upKey)) direction.z++;
        if (Input.GetKey(downKey)) direction.z--;
        if (Input.GetKey(leftKey)) direction.x++;
        if (Input.GetKey(rightKey)) direction.x--;

        if (Time.time > nextMaxSpeedRandomizationTime)
        {
            DrunkMalus currentDrunkMalus = drunkMaluses[GameManager.DrunkLevel];
            maxSpeed = GaussianDistribution.Generate(currentDrunkMalus.speedMean, currentDrunkMalus.speedVariance);
            nextMaxSpeedRandomizationTime = Time.time + GaussianDistribution.Generate(4, 1);
        }

        if (direction == Vector3.zero)
        {
            velocity -= acceleration * velocity.normalized * Time.deltaTime / 2;

            if(driftAngle < driftSpeed)
            {
                driftAngle = 0;
            }
            else
            {
                driftAngle = driftAngle - Mathf.Sign(driftAngle) * driftSpeed;
            }
        }
        else
        {
            if (velocity.sqrMagnitude > maxSpeed * maxSpeed)
            {
                velocity -= acceleration * velocity.normalized * Time.deltaTime / 2;
            }
            else
            {
                velocity += acceleration * direction.normalized * Time.deltaTime;
            }

            transform.rotation = Quaternion.LookRotation(velocity, Vector3.up);

            // Calculate drifting
            float angle = Vector3.SignedAngle(velocity, direction, Vector3.up);
            if (Mathf.Abs(angle) < maxDriftAngle) angle = 0; // Because it will never be exactly zero
            angle = Mathf.Clamp(Mathf.Abs(angle), 0, driftSpeed) * Mathf.Sign(angle);            
            if (Mathf.Abs(angle) < Mathf.Epsilon && Mathf.Abs(driftAngle) > Mathf.Epsilon)
            {
                angle = -Mathf.Clamp(Mathf.Abs(driftAngle), 0, driftSpeed) * Mathf.Sign(driftAngle);
            }
            driftAngle += angle;
        }
        velocity = Vector3.ClampMagnitude(velocity, 15);
        driftAngle = Mathf.Clamp(Mathf.Abs(driftAngle), 0, maxDriftAngle) * Mathf.Sign(driftAngle);

        // Drift
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, driftAngle);

        // Translate
        transform.Translate(velocity * Time.fixedDeltaTime, Space.World);
    }

    private void respectBounds()
    {
        if (!bounds.Contains(transform.position))
        {
            transform.position = bounds.ClosestPoint(transform.position);
        }
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
