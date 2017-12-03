using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour {

    public float targetY;
    public float yTolerance;
    [HideInInspector]
    public KeyCode keyCode;
    [HideInInspector]
    public Karaoke karaoke;
    [HideInInspector]
    public Note nextNote;

    private static Note currentNoteToCatch;
    
	void Start ()
    {
		if(currentNoteToCatch == null)
        {
            currentNoteToCatch = this;
        }
	}
	
	void Update ()
    {
        if(this != currentNoteToCatch)
        {
            return;
        }

        foreach (KeyCode key in karaoke.keyCodes)
        {
            if(key != keyCode && Input.GetKeyDown(key))
            {
                karaoke.WrongClick();
            }
        }

        bool isClickable = transform.position.y < targetY + yTolerance && transform.position.y > targetY - yTolerance;

        if (Input.GetKeyDown(keyCode))
        {
            if (isClickable)
            {
                currentNoteToCatch = nextNote;
                karaoke.NoteCatched();
                Destroy(gameObject);
            }
            else
            {
                karaoke.WrongClick();
            }
        }
        else if(transform.position.y < targetY - yTolerance)
        {
            currentNoteToCatch = nextNote;
            karaoke.MissedNote();
            this.enabled = false;
        }
    }

    private IEnumerator changeNote()
    {
        yield return null;
        currentNoteToCatch = nextNote;
    }
}
