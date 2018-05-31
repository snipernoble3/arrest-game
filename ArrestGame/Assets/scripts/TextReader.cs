using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VNFramework;


public class TextReader : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void readinRooms (TextAsset t) {

        List<Room> r = new List<Room>();


        SendMessage("updateRooms", r);
    }

    public void readinCharacters (TextAsset t) {

        List<Character> c = new List<Character>();


        SendMessage("updateCharacters", c);
    }

    public void readinEvidence (TextAsset t) {

        List<Evidence> e = new List<Evidence>();


        SendMessage("updateEvidence", e);
    }

}
