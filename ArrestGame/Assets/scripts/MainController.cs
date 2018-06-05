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
    public Text timer;

    public TextAsset roomstxt;
    public TextAsset characterstxt;
    public TextAsset evidencetxt;

    private List<Room> roomList = new List<Room>();
    private List<Character> characterList = new List<Character>();
    private List<Evidence> evidenceList = new List<Evidence>();

    private Room currentRoom;
    private Character currentCharacter;
    private string currentDialogue;

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

    public void showDialogue () {
        dialogue.enabled = true;
    }

    public void hideDialogue () {
        dialogue.enabled = false;
    }

    public void loadDialogue (string k) {
        //open dialogue
        //check dialogue length
        //type one section at a time
        //wait for player input to continue
        //reset scene after dialogue
    }

    public void typeDialogue () {
        //add one character to the text at a time at a certain speed
    }

    public void skipDialogue () {
        //add the full remainder of the dialogue to the text
    }

    public void showOptions () {
        //enable options
    }

    public void hideOptions () {
        //disable options
    }

    public void loadOptions (/*Room r*/) {
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
