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
    public Text currentSpeaker;
    //public Text timer;
    public Text clickToContinue;

    public Button option0;
    public Button option1;
    public Button option2;
    public Button option3;
    public Text option0Label;
    public Text option1Label;
    public Text option2Label;
    public Text option3Label;

    public TextAsset startScene;
    public TextAsset allCharacters;
    public TextAsset allRooms;
    public TextAsset startDialogue;
    //public TextAsset evidencetxt;

    private List<Room> fullRoomList = new List<Room>();
    private List<Character> fullCharacterList = new List<Character>();
    //private List<Evidence> evidenceList = new List<Evidence>();

    private List<Room> currentRoomList = new List<Room>();
    private int currentRoomIndex;
    private string currentCharacter;
    //private string currentDialogue;

    private Button[] buttons = new Button[4];
    private Text[] buttonLabels = new Text[4];

    private bool typing = false;
    private bool waiting = false;
    private string optionType;
    private string[] buttonOptions = new string[4];


	// Use this for initialization
	void Start () {

        buttons[0] = option0.GetComponent<Button>();
        buttons[1] = option1.GetComponent<Button>();
        buttons[2] = option2.GetComponent<Button>();
        buttons[3] = option3.GetComponent<Button>();
        buttonLabels[0] = option0Label;
        buttonLabels[1] = option1Label;
        buttonLabels[2] = option2Label;
        buttonLabels[3] = option3Label;

        SendMessage("readinCharacters", allCharacters);
        SendMessage("readinRooms", allRooms);
        SendMessage("readinScene", startScene);
        
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
            clickToContinue.gameObject.SetActive(false);
        }


	}

    public void updateRoomList (List<Room> r) {
        fullRoomList = r;
        //Debug.Log("All Rooms: "+r.Count);
    }

    public void updateCharacterList (List<Character> c) {
        
        fullCharacterList = c;
        
        //Debug.Log("All Characters: " + c.Count);
    }

    public void updateCurrentRooms (List<Room> r) {
        
        currentRoomList = r;
        //Debug.Log("Current rooms: "+r.Count);
    }

    public void updateCurrentCharacters (List<string> c) {
        //currentCharacterList = c;
        
        foreach (string s in c) {
            string[] curr = s.Split('~');
            for (int i = 0; i < fullCharacterList.Count; i++) {
                
                if (curr[0] == fullCharacterList[i].getName()) {
                    //Debug.Log("Adding Dialogue to " + fullCharacterList[i].getName());
                    for (int k = 1; k < curr.Length; k++) {
                        fullCharacterList[i].addLine(curr[k].Split(';')[0], curr[k].Split(';')[1]);
                        //Debug.Log("Successfully added dialogue");
                    }
                }
            }
            
        }
    }

    /*
    public void updateEvidence (List<Evidence> e) {
        evidenceList = e;
    }
    */

    /*
    public void changeRooms (string rName) {
        
        foreach (Room r in roomList) {
            if (r.name == rName) {
                room.SendMessage("changeSprite", r.getSprite());

                currentRoomIndex;
                return;
            }
        }

    }
    */

    public void changeBackground (string n) {
        room.SendMessage("changeSprite", n);
    }

    public void changeCharacter (string n) {
        Debug.Log("changing sprite to " + n);
        character.SendMessage("changeSprite", n);
        currentCharacter = n;
    }

    /*
    public void changeCharacters (string cName) {

        if (roomList[currentRoomIndex].getCharacters().Contains(cName)) {
            foreach (Character c in characterList) {
                if (c.name == cName) {
                    character.SendMessage("changeSprite", c.getCN());
                    currentCharacter = c;
                    return;
                }
            }
        }

    }
    */

    public void changeRoom (string n) {
        
        //Debug.Log("Checking for room " + n);
        for (int i = 0; i < currentRoomList.Count; i++) {
            //Debug.Log("Comparing to " + currentRoomList[i].getName());
            if (currentRoomList[i].getName() == n) {
                //Debug.Log("Found Room");
                character.SendMessage("changeSprite", "none");
                changeBackground(currentRoomList[i].getName());
                List<string> options = currentRoomList[i].getCharacters();
                currentRoomIndex = i;
                loadRoomOptions(options);
                return;
            }
        }
        //Debug.Log("Room not found");
    }

    
    public void showDialogue () {
        dialogue.gameObject.SetActive(true);
    }

    public void hideDialogue () {
        dialogue.gameObject.SetActive(false);
    }
    

    public void loadDialogue (string fileName) {
        SendMessage("readDialogue", fileName);
    }

    public void isTyping (bool b) {
         typing = b;
    }

    public void isWaiting () {
        waiting = true;
        clickToContinue.gameObject.SetActive(false);
    }

    public void skipDialogue () {
        SendMessage("finish");
    }

    

    public void showOptions (int o) {
        for (int i = 0; i < 4; i++) {
            buttons[i].gameObject.SetActive(i<o);
        }
    }

    public void hideOptions () {
        for (int i = 0; i < 4; i++) {
            buttons[i].gameObject.SetActive(false);
        }
    }

    public void loadRoomOptions ( List<string> options) {
        //Debug.Log("Loading room options");
        hideDialogue();
        currentSpeaker.text = currentRoomList[currentRoomIndex].getName();
        //string selectionType = "character";
        string prefix = "Talk to ";
        string exit = "Change Rooms";
        //Debug.Log("options.Count = "+options.Count);
        if (options.Count < 4) {
            showOptions(options.Count + 1);
            for (int i = 0; i < options.Count; i++) {
                buttonLabels[i].text = prefix + options[i];
                buttonOptions[i] = options[i];
                //buttons[i].onClick.AddListener(delegate { selectCharacter(options[i]); });
            }
            buttonLabels[options.Count].text = exit;
            buttonOptions[options.Count] = exit;
            optionType = "character";
            //buttons[options.Count].onClick.AddListener(delegate { selectCharacter(exit); });
            
        }
        //Debug.Log("Finished loading room options");
    }

    public void loadCharacterOptions (string n) {
        hideDialogue();
        currentSpeaker.text = n;
        List<Dialogue> d = new List<Dialogue>();

        foreach (Character c in fullCharacterList) {
            if (c.getName() == n) {
                d = c.getAvailableLines();
            }
        }

        if (d.Count < 4) {
            showOptions(d.Count+1);
            for (int i = 0; i < d.Count; i++) {
                buttonLabels[i].text = d[i].buttonText;
                buttonOptions[i] = d[i].fileName;
                //buttons[i].onClick.AddListener(delegate { selectDialogue(d[i].fileName); });
            }
        }
        buttonLabels[d.Count].text = "Goodbye";
        buttonOptions[d.Count] = "Goodbye";
        optionType = "dialogue";
        //buttons[d.Count].onClick.AddListener(delegate { selectDialogue("Goodbye"); });
    }

    public void loadRoomList () {
        hideDialogue();
        currentSpeaker.text = "Room Directory";
        if (currentRoomList.Count < 5) {
            showOptions(currentRoomList.Count);
            for (int i = 0; i < currentRoomList.Count; i++) {
                
                buttonLabels[i].text = currentRoomList[i].getName();
                buttonOptions[i] = currentRoomList[i].getName();
                //buttons[i].onClick.AddListener(delegate { changeRoom(currentRoomList[i].getName()); });

            }
            optionType = "room";
        }
    }

    public void selectCharacter (string n) {
        if (n == "Change Rooms") {
            loadRoomList();
        } else {

            //character.SetActive(true);
            currentCharacter = n;
            changeCharacter( n + "N");
            loadCharacterOptions(n);
        }
    }

    public void selectDialogue (string fileName) {
        if (fileName == "Goodbye") {
            changeRoom(currentRoomList[currentRoomIndex].getName());
        } else {
            hideOptions();
            showDialogue();
            loadDialogue(fileName);
        }
    }

    public void makeSelection (int bNum) {
        switch (optionType) {
            case "room":
                changeRoom(buttonOptions[bNum]);
                break;
            case "character":
                selectCharacter(buttonOptions[bNum]);
                break;
            case "dialogue":
                selectDialogue(buttonOptions[bNum]);
                break;
        }
    }

    public void addDialogue (string unparsed) {
        string cName = unparsed.Split('_')[0];
        string[] d = unparsed.Split('_')[1].Split(';');
        string b = d[0];
        string f = d[1];
        for (int i = 0; i < fullCharacterList.Count; i++) {
            if (fullCharacterList[i].getName() == cName) {
                fullCharacterList[i].addLine(b, f);
            }
        }
    }

    public void removeDialogue (string unparsed) {
        string cName = unparsed.Split('_')[0];
        string[] d = unparsed.Split('_')[1].Split(';');
        string b = d[0];
        string f = d[1];
        for (int i = 0; i<fullCharacterList.Count; i++) {
            if (fullCharacterList[i].getName() == cName) {
                fullCharacterList[i].removeLine(b, f);
            }
        }
    }

}
