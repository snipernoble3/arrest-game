using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VNFramework;
using UnityEngine.UI;

public class MainController : MonoBehaviour {

    public GameObject room;
    public GameObject character;
    public GameObject evidence;
    public Text dialogue;

    public TextAsset roomstxt;
    public TextAsset characterstxt;
    public TextAsset evidencetxt;

    private List<Room> roomList = new List<Room>();
    private List<Character> characterList = new List<Character>();
    private List<Evidence> evidenceList = new List<Evidence>();

    private Room currentRoom;
    private Character currentCharacter;


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

    public void changeRooms (string rName) {
        
        foreach (Room r in roomList) {
            if (r.name == rName) {
                room.SendMessage("changeSprite", r.getSprite());
                currentRoom = r;
                return;
            }
        }

    }

    public void changeCharacters (string cName) {

        if (currentRoom.getCharacters().Contains(cName)) {
            foreach (Character c in characterList) {
                if (c.name == cName) {
                    character.SendMessage("changeSprite", c.getCN());
                    currentCharacter = c;
                    return;
                }
            }
        }

    }

}
