using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour {

    public Transform entrancePosition;
    public GameObject pressToEnterTip;
    
    public Transform player;

    private static Bar barThatEnabledTip;
    
	void Start ()
    {
	}
	
	void Update ()
    {
		if((entrancePosition.position - player.position).sqrMagnitude < 30)
        {
            barThatEnabledTip = this;
            pressToEnterTip.SetActive(true);
            if (Input.GetKeyDown(KeyCode.Space))
            {
                // Enter the bar
                GameManager.EnterBar(this, player);
            }
        }
        else
        {
            if(this == barThatEnabledTip)
            {
                pressToEnterTip.SetActive(false);
                barThatEnabledTip = null;
            }
        }
	}
}
