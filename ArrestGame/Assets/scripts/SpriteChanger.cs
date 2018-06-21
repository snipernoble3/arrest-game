using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteChanger : MonoBehaviour {

    private SpriteRenderer sr;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void changeSprite (string n) {
        sr = GetComponent<SpriteRenderer>();
        
        Sprite s = Resources.Load<Sprite>("sprites\\" + n); //load the sprite from the sprites folder

        //if the sprite is null after attempting to load, set it to the blank sprite and send a debug.log message
        if (s == null) {
            Debug.Log("Setting sprite to none");
            s = Resources.Load<Sprite>("sprites\\none");
        }

        sr.sprite = s; //set the new sprite with the sprite renderer
    }

}
