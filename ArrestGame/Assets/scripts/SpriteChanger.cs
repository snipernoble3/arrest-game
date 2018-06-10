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
        if (s == null) {
            Debug.Log("Setting sprite to none");
            s = Resources.Load<Sprite>("sprites\\none");
        }
        sr.sprite = s;
    }

}
