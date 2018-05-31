using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VNFramework;

public class MainController : MonoBehaviour {

    public GameObject room;
    public GameObject character;
    public GameObject evidence;

    public TextAsset roomstxt;
    public TextAsset characterstxt;
    public TextAsset evidencetxt;

    private List<Room> roomList = new List<Room>();
    private List<Character> characterList = new List<Character>();
    private List<Evidence> evidenceList = new List<Evidence>();


	// Use this for initialization
	void Start () {
        SendMessage("readinRooms", roomstxt);
        SendMessage("readinCharacters", characterstxt);
        SendMessage("readinEvidence", evidencetxt);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void updateRooms (List<Room> r) {
        roomList = r;
    }

    public void updateCharacters (List<Character> c) {
        characterList = c;
    }

    public void updateEvidence (List<Evidence> e) {
        evidenceList = e;
    }

}
