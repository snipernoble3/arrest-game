using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour {

    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void changeScene (string s) {
        if (s == "Settings") {
            SceneManager.LoadScene(s, LoadSceneMode.Additive);
        } else {
            SceneManager.LoadScene(s);
        }
        
    }

}
