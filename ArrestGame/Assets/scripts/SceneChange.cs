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
            /*
            GameObject[] curr = SceneManager.GetActiveScene().GetRootGameObjects();
            for (int i = 0; i < curr.Length; i++) {
                if (curr[i].name == "Canvas") {
                    curr[i].SetActive(false);
                }
            }
            */
            SceneManager.LoadScene(s, LoadSceneMode.Additive);
        } else if (s == "return") {
            SceneManager.UnloadSceneAsync("Settings");
        } else {
            SceneManager.LoadScene(s);
        }
        
    }

}
