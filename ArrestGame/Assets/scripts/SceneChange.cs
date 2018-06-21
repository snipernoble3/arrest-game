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
        if (s == "Settings") { //if the scene being loaded is the settings tab, open it on top instead of replacing the current scene
            SceneManager.LoadScene(s, LoadSceneMode.Additive);
        } else if (s == "return") { //if the scene name is return, it means it is recieving the message to close the settings tab
            SceneManager.UnloadSceneAsync("Settings");
        } else { //if anything else, replace the current scene
            SceneManager.LoadScene(s);
        }
        
    }

}
