using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Darts : MonoBehaviour {

    public WaypointWanderer target;
    public WaypointWanderer aim;
    public Animator handAnimator;
    public LayerMask targetLayerMask;
    public GameObject Target;
    
    void Start ()
    {
        Target.transform.localScale -= new Vector3(GameManager.GetMinigameSetting("Darts", "TargetScale"), GameManager.GetMinigameSetting("Darts", "TargetScale"), 0);

        float targetSpeed = GaussianDistribution.Generate(
            GameManager.GetMinigameSetting("Darts", "TargetSpeedMean"),
            GameManager.GetMinigameSetting("Darts", "TargetSpeedVariance"));
        targetSpeed = Mathf.Clamp(targetSpeed, 2, float.MaxValue);
        target.speed = targetSpeed;

        float aimSpeed = GaussianDistribution.Generate(
            GameManager.GetMinigameSetting("Darts", "AimSpeedMean"),
            GameManager.GetMinigameSetting("Darts", "AimSpeedVariance"));
        aimSpeed = Mathf.Clamp(aimSpeed, 2, float.MaxValue);
        aim.speed = aimSpeed;
    }
	
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // Shoot!
            handAnimator.SetTrigger("Throw");
            target.speed = 0;
            aim.speed = 0;

            if(Physics.Raycast(aim.transform.position, Vector3.back, 20, targetLayerMask))
            {
                FindObjectOfType<MinigameManager>().Win();
            }
            else
            {
                FindObjectOfType<MinigameManager>().Lose();
            }
        }
	}
}
