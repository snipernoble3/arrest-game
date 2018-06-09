using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VNFramework;
using UnityEngine.UI;

public class MainController : MonoBehaviour {

    //public TextAsset t;
    //public Sprite s;

    public GameObject room;
    public GameObject character;
    //public GameObject evidence;
    public Text dialogue;
    public Text timer;

    public TextAsset startRooms;
    public TextAsset startCharacters;
    public TextAsset startDialogue;
    public TextAsset evidencetxt;

    private List<Room> roomList = new List<Room>();
    private List<Character> characterList = new List<Character>();
    private List<Evidence> evidenceList = new List<Evidence>();

    private Room currentRoom;
    private Character currentCharacter;
    private string currentDialogue;

    private bool typing = false;
    private bool waiting = false;

	// Use this for initialization
	void Start () {
        //SendMessage("readinRooms", roomstxt);
        //SendMessage("readinCharacters", characterstxt);
        //SendMessage("readinEvidence", evidencetxt);
        SendMessage("readDialogue", startDialogue);
	}
	
	// Update is called once per frame
	void Update () {
		

        if (typing && Input.GetKeyDown(KeyCode.RightArrow)) {
            skipDialogue();
        }

        if (waiting && Input.GetKeyDown(KeyCode.RightArrow)) {
            SendMessage("next");
            waiting = false;
        }


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

    public void changeBackground (string n) {
        //Sprite s = Resources.Load(n) as Sprite;
        //Debug.Log("Sending " + n + " to sprite changer");
        /*
        if (s == null) {
            Debug.Log("Sprite s is null");
        } else {
            Debug.Log("Sending sprite s");
        }
        */
        room.SendMessage("changeSprite", n);
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

    public void showDialogue () {
        dialogue.enabled = true;
    }

    public void hideDialogue () {
        dialogue.enabled = false;
    }

    public void loadDialogue (string fileName) {
        SendMessage("readDialogue", fileName);
    }

    public void isTyping (bool b) {
         typing = b;
    }

    public void isWaiting () {
        waiting = true;
    }

    public void skipDialogue () {
        SendMessage("finish");
    }

    public void showOptions () {
        //enable options
    }

    public void hideOptions () {
        //disable options
    }

    public void loadOptions (string[] options) {
        //find all options in the room/character
        //assign all options to buttons
    }

    public void selectOption () {
        //when an option is clicked, do its action
        //character - change option list to list of dialogue
        //evidence - show new hint and advance step, then return to room options
        //dialogue - show response
    }

}
