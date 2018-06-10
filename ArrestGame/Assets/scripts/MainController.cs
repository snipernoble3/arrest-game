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
    //public Text timer;
    
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
    private int currentCharacterIndex;
    private string currentDialogue;

    private Button[] buttons = new Button[4];
    private Text[] buttonLabels = new Text[4];

    private bool typing = false;
    private bool waiting = false;

	// Use this for initialization
	void Start () {

        buttons[0] = option0;
        buttons[1] = option1;
        buttons[2] = option2;
        buttons[3] = option3;
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
        }


	}

    public void updateRoomList (List<Room> r) {
        fullRoomList = r;
    }

    public void updateCharacterList (List<Character> c) {
        fullCharacterList = c;
    }

    public void updateCurrentRooms (List<Room> r) {
        currentRoomList = r;
        
    }

    public void updateCurrentCharacters (List<string> c) {
        //currentCharacterList = c;
        foreach (string s in c) {
            string[] curr = s.Split('~');
            for (int i = 0; i < fullCharacterList.Count; i++) {
                if (curr[0] == fullCharacterList[i].name) {
                    for (int k = 1; k < curr.Length; k++) {
                        fullCharacterList[i].addLine(curr[k].Split(';')[0], curr[k].Split(';')[1]);
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
        for (int i =0; i < currentRoomList.Count; i++) {
            if (currentRoomList[i].name == n) {
                character.SetActive(false);
                changeBackground(currentRoomList[i].name);
                List<string> options = currentRoomList[i].getCharacters();
                currentRoomIndex = i;
                loadRoomOptions(options);
            }
        }
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
        hideDialogue();
        
        //string selectionType = "character";
        string prefix = "Talk to ";
        string exit = "Change Rooms";
        if (options.Count < 4) {
            for (int i = 0; i < options.Count; i++) {
                buttonLabels[i].text = prefix + options[i];
                buttons[i].onClick.AddListener(delegate { selectCharacter(options[i]); });
            }
            buttonLabels[options.Count].text = exit;
            buttons[options.Count].onClick.AddListener(delegate { selectCharacter(exit); });
            showOptions(options.Count+1);
        }
        
    }

    public void loadCharacterOptions (string n) {
        hideDialogue();
        List<Dialogue> d = new List<Dialogue>();

        foreach (Character c in fullCharacterList) {
            if (c.name == n) {
                d = c.getAvailableLines();
            }
        }

        if (d.Count < 4) {
            for (int i = 0; i < d.Count; i++) {
                buttonLabels[i].text = d[i].buttonText;
                buttons[i].onClick.AddListener(delegate { selectDialogue(d[i].fileName); });
            }
        }
        buttonLabels[d.Count].text = "Goodbye";
        buttons[d.Count].onClick.AddListener(delegate { selectDialogue("Goodbye"); });
    }

    public void loadRoomList () {
        hideDialogue();
        if (currentRoomList.Count < 4) {
            for (int i = 0; i < currentRoomList.Count; i++) {
                buttonLabels[i].text = currentRoomList[i].name;
                buttons[i].onClick.AddListener(delegate { changeRoom(currentRoomList[i].name); });
            }
            
            showOptions(currentRoomList.Count);
        }
    }

    public void selectCharacter (string n) {
        if (n == "Change Rooms") {
            loadRoomList();
        } else {
            
            character.SetActive(true);
            character.SendMessage("changeSprite", n + "N");
            loadCharacterOptions(n);
        }
    }

    public void selectDialogue (string fileName) {
        if (fileName == "Goodbye") {
            changeRoom(currentRoomList[currentRoomIndex].name);
        } else {
            hideOptions();
            showDialogue();
            loadDialogue(fileName);
        }
    }

}
