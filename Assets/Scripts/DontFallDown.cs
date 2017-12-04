using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DontFallDown : MonoBehaviour {

    public Transform sliderCenter;
    public Transform sliderSX;
    public Transform sliderDX;
    public Transform sliderHandle;
    public float sliderLength;

    public Transform player;
    public Text timerText;

    private float handleVelocity;
    private float force;
    private float inertia;
    private float losePercentage;
    private float remainingTime;

    void Start ()
    {
        force = GameManager.GetMinigameSetting("DontFallDown", "Force");
        inertia = GameManager.GetMinigameSetting("DontFallDown", "Inertia");
        handleVelocity = GaussianDistribution.Generate(0, GameManager.GetMinigameSetting("DontFallDown", "StartingVelocityVariance"));
        losePercentage = GameManager.GetMinigameSetting("DontFallDown", "LosePercentage");
        remainingTime = GameManager.GetMinigameSetting("DontFallDown", "Time");

        sliderCenter.localScale = new Vector3(1 - losePercentage, 1, 1);
        sliderSX.localScale = new Vector3(losePercentage / 2, 1, 1);
        sliderDX.localScale = new Vector3(losePercentage / 2, 1, 1);
    }
	
	void Update ()
    {
        remainingTime -= Time.deltaTime;
        timerText.text = remainingTime.ToString("0.0");
        if(remainingTime <= 0)
        {
            // Win
            timerText.text = "0";
            FindObjectOfType<MinigameManager>().Win();
            this.enabled = false;
        }

        // User add velocity to handle
        if (Input.GetKey(KeyCode.D))
        {
            handleVelocity += force * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            handleVelocity -= force * Time.deltaTime;
        }

        // Handle add velocity to itself
        float unbalancePercentage = sliderHandle.localPosition.x / (sliderLength / 2);
        handleVelocity -= inertia * unbalancePercentage * Time.deltaTime;

        sliderHandle.Translate(Vector3.left * handleVelocity * Time.deltaTime);
        player.rotation = Quaternion.Euler(0, 0, -unbalancePercentage * 90);

        if(Mathf.Abs(unbalancePercentage) > 1 - losePercentage)
        {
            // Lose
            Rigidbody playerRigidbody = player.GetComponent<Rigidbody>();
            playerRigidbody.useGravity = true;
            playerRigidbody.AddForce(Vector3.down * 6, ForceMode.Impulse);
            FindObjectOfType<MinigameManager>().Lose();
            this.enabled = false;
        }
	}
}
