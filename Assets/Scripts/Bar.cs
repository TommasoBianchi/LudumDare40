using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bar : MonoBehaviour {
    
    public Transform entrancePosition;
    public GameObject pressToEnterTip;
    public GameObject selectedBarLight;
    
    public Transform player;

    private static Bar barThatEnabledTip;
    
	void Start ()
    {
        if(name == GameManager.SelectedBarName)
        {
            selectedBarLight.SetActive(true);
        }
        else
        {
            selectedBarLight.SetActive(false);
        }

        setupPeopleOnRoof();
	}
	
	void Update ()
    {
		if((entrancePosition.position - player.position).sqrMagnitude < 30 && name == GameManager.SelectedBarName)
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

    private void setupPeopleOnRoof()
    {
        foreach (BoxBounder bounder in gameObject.GetComponentsInChildren<BoxBounder>())
        {
            bounder.bounds.center = transform.position;
        }
    }
}
