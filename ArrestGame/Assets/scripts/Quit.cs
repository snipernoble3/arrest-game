using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quit : MonoBehaviour {

    //quit the game
    public void quitGame () {
        Debug.Log("Quit Game");
        Application.Quit();
    }
}
