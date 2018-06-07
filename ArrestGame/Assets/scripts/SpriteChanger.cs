using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteChanger : MonoBehaviour {

    //public Sprite s;

    private SpriteRenderer sr;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void changeSprite (string n) {
        sr = GetComponent<SpriteRenderer>();
        Sprite s = Resources.Load<Sprite>("sprites\\" + n);// as Sprite;
        /*
        if (s == null) {
            Debug.Log("Sprite is null");
        } else {
            Debug.Log("Sprite is not null");
        }
        
        if (sr.sprite == null) {
            Debug.Log("sr.sprite is null");
        } else {
            Debug.Log("sr.sprite is not null");
        }
        */
        sr.sprite = s;
    }

}
