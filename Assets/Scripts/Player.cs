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
    private KeyCode rightKey = KeyCode.D;
    private KeyCode leftKey = KeyCode.A;
    private UIManager uiManager;

    private float nextMaxSpeedRandomizationTime = 0;
    private float nextKeyRandomizationTime;

    private Dictionary<GameManager.Drunkness, DrunkMalus> drunkMaluses;
    
	void Start ()
    {
        deactivatePlayerCollisions();

        uiManager = FindObjectOfType<UIManager>();

        transform.position = GameManager.PlayerSpawnPosition;

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
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.layer == LayerMask.NameToLayer("Car"))
        {
            Vector3 forceDirection = transform.position - collision.contacts[0].point;
            GetComponent<Rigidbody>().AddForce(forceDirection.normalized * 1000, ForceMode.Impulse);
            GetComponent<Rigidbody>().AddForce(Vector3.up * 1000, ForceMode.Impulse);
            Die();
        }
    }

    public void Die()
    {
        activatePlayerCollisions();
        GetComponent<Rigidbody>().useGravity = true;
        this.enabled = false;
        uiManager.PlayGameOverAnimation();
    }

    private void move()
    {
        Vector3 direction = Vector3.zero;
        if (Input.GetKey(upKey)) direction.z++;
        if (Input.GetKey(downKey)) direction.z--;
        if (Input.GetKey(leftKey)) direction.x--;
        if (Input.GetKey(rightKey)) direction.x++;

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

        //for (int i = keyCodes.Length - 1; i >= 0; i--)
        //{
        //    int j = Random.Range(0, i + 1);
        //    KeyCode tmp = keyCodes[i];
        //    keyCodes[i] = keyCodes[j];
        //    keyCodes[j] = tmp;
        //}

        float randomChoice = Random.Range(0f, 1f);
        if(randomChoice <= 0.35f)
        {
            // Invert X axis
            KeyCode tmp = keyCodes[2];
            keyCodes[2] = keyCodes[3];
            keyCodes[3] = tmp;
        }
        else if(randomChoice <= 0.7f)
        {
            // Invert Y axis
            KeyCode tmp = keyCodes[0];
            keyCodes[0] = keyCodes[1];
            keyCodes[1] = tmp;
        }
        else
        {
            // Invert both axis
            KeyCode tmp = keyCodes[0];
            keyCodes[0] = keyCodes[1];
            keyCodes[1] = tmp;
            tmp = keyCodes[2];
            keyCodes[2] = keyCodes[3];
            keyCodes[3] = tmp;
        }

        upKey = keyCodes[0];
        downKey = keyCodes[1];
        rightKey = keyCodes[2];
        leftKey = keyCodes[3];

        uiManager.UpdateKeyBindings(keyCodes);
    }

    private void deactivatePlayerCollisions()
    {
        int mylayer = gameObject.layer;
        int carLayer = LayerMask.NameToLayer("Car");
        for (int i = 0; i < 32; i++)
        {
            if(i != carLayer)
            {
                Physics.IgnoreLayerCollision(mylayer, i, true);
            }
        }
    }

    private void activatePlayerCollisions()
    {
        int mylayer = gameObject.layer;
        for (int i = 0; i < 32; i++)
        {
            Physics.IgnoreLayerCollision(mylayer, i, false);
        }
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
