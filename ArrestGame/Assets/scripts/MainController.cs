using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VNFramework;
using UnityEngine.UI;

public class MainController : MonoBehaviour {
    

    public GameObject room; //Object used for displaying backgrounds
    public GameObject character; //Object used for displaying characters
    public Text dialogue; //The textbox for dialogue
    public Text currentSpeaker; //The textbox for the current speaker
    public Text clickToContinue; //The text informing the player when to continue


    //The buttons for options, as well as their corresponding labels
    public Button option0;
    public Button option1;
    public Button option2;
    public Button option3;
    public Text option0Label;
    public Text option1Label;
    public Text option2Label;
    public Text option3Label;

    public TextAsset startScene; //the text file to read in as the start scene
    public TextAsset allCharacters; //the text file to read in all characters in the story
    public TextAsset allRooms; //the text file to read in all rooms in the story
    public TextAsset startDialogue; //the intro dialogue


    private List<Room> fullRoomList = new List<Room>(); 
    private List<Character> fullCharacterList = new List<Character>();

    private List<Room> currentRoomList = new List<Room>(); //currently accessible rooms
    private int currentRoomIndex; //the index in the list of the room you are in
    private string currentCharacter; //the name of the current character

    private Button[] buttons = new Button[4]; //array of the 4 buttons
    private Text[] buttonLabels = new Text[4]; //array of the 4 labels

    private bool typing = false; //is the dialogue currently being typed?
    private bool waiting = false; //is the game waiting for user input?

    private string optionType; //is the button selecting a room, character, or dialogue
    private string[] buttonOptions = new string[4]; //the current 4 options displayed on the buttons


	// Use this for initialization
	void Start () {

        //assigning all the buttons and labels to their spot in the array
        buttons[0] = option0.GetComponent<Button>();
        buttons[1] = option1.GetComponent<Button>();
        buttons[2] = option2.GetComponent<Button>();
        buttons[3] = option3.GetComponent<Button>();
        buttonLabels[0] = option0Label;
        buttonLabels[1] = option1Label;
        buttonLabels[2] = option2Label;
        buttonLabels[3] = option3Label;

        SendMessage("readinCharacters", allCharacters); //load in all the characters
        SendMessage("readinRooms", allRooms); //load in all the rooms
        SendMessage("readinScene", startScene); //load in the current scene
        
        SendMessage("readDialogue", startDialogue); //start running the intro dialogue
	}
	
	// Update is called once per frame
	void Update () {
		
        //if the game is typing and the player presses the right arrow key, skip to the end of the current dialogue
        if (typing && Input.GetKeyDown(KeyCode.RightArrow)) {
            skipDialogue();
        }

        //if the game is waiting for player input and the player presses the right arrow key, continue
        if (waiting && Input.GetKeyDown(KeyCode.RightArrow)) {
            SendMessage("next");
            waiting = false;
            clickToContinue.gameObject.SetActive(false);
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
        //for each string in the list of characters
        foreach (string s in c) {
            //split the name and dialogue into segments
            string[] curr = s.Split('~');
            //check each character in the character list
            for (int i = 0; i < fullCharacterList.Count; i++) {
                //if the first segment (the name) matches the name of the currently selected character (from the for)
                if (curr[0] == fullCharacterList[i].getName()) {
                    //step through each remaining index of 
                    for (int k = 1; k < curr.Length; k++) {
                        fullCharacterList[i].addLine(curr[k].Split(';')[0], curr[k].Split(';')[1]);
                    }
                }

            }

        }

    }

    public void changeBackground (string n) {
        room.SendMessage("changeSprite", n);
    }

    public void changeCharacter (string n) {
        character.SendMessage("changeSprite", n);
        currentCharacter = n;
    }

    //for entering a room and generating a list of available characters to talk to
    public void changeRoom (string n) {
        //step through the list of available rooms
        for (int i = 0; i < currentRoomList.Count; i++) {
            //if the current room at the loop index (i) is equal to the name of the new room (string n)
            if (currentRoomList[i].getName() == n) {
                
                character.SendMessage("changeSprite", "none"); //set the character sprite to be the blank sprite
                changeBackground(currentRoomList[i].getName()); //set the room sprite to be the sprite with the same name as the room
                List<string> options = currentRoomList[i].getCharacters(); //get the available list of characters from the room
                currentRoomIndex = i; //set the current room index to the current index of the for loop (i)
                loadRoomOptions(options); 
                return;

            }
        }
        
    }

    
    public void showDialogue () {
        dialogue.gameObject.SetActive(true); //set the dialogue textbox to be active/visible
    }

    public void hideDialogue () {
        dialogue.gameObject.SetActive(false); //set the dialogue textbox to be inactive/invisible
    }
    

    public void loadDialogue (string fileName) {
        SendMessage("readDialogue", fileName);
    }

    public void isTyping (bool b) {
         typing = b;
    }

    public void isWaiting () {
        waiting = true;
        clickToContinue.gameObject.SetActive(true); //makes the click to continue text visible
    }

    public void skipDialogue () {
        SendMessage("finish");
    }
    
    public void showOptions (int o) {
        //activate all buttons below
        for (int i = 0; i < 4; i++) {
            buttons[i].gameObject.SetActive(i<o);
        }
    }

    public void hideOptions () {
        //hide all the buttons
        for (int i = 0; i < 4; i++) {
            buttons[i].gameObject.SetActive(false);
        }
    }

    public void loadRoomOptions ( List<string> options) {
        //when entering a new room, make sure dialogue is hidden and the room name is displayed
        hideDialogue();
        currentSpeaker.text = currentRoomList[currentRoomIndex].getName();
        
        string prefix = "Talk to "; //prefix for button labels
        string exit = "Change Rooms";
        
        //if there are less than 4 characters to choose from
        if (options.Count < 4) {
            showOptions(options.Count + 1); //show the room options plus the exit option

            //for each option, set the label and onClick for the associated button
            for (int i = 0; i < options.Count; i++) { 
                buttonLabels[i].text = prefix + options[i];
                buttonOptions[i] = options[i];
                //buttons[i].onClick.AddListener(delegate { selectCharacter(options[i]); });
            }

            //add the exit button and option type
            buttonLabels[options.Count].text = exit;
            buttonOptions[options.Count] = exit;
            //buttons[options.Count].onClick.AddListener(delegate { selectCharacter(exit); });
        }

        optionType = "character";

    }

    public void loadCharacterOptions (string n) {
        //when displaying character's options, make sure dialogue is hidden and the character name is displayed
        hideDialogue();
        currentSpeaker.text = n;

        List<Dialogue> d = new List<Dialogue>();
        //check the character list for the one with a matching name, then get that character's dialogue options
        foreach (Character c in fullCharacterList) {
            if (c.getName() == n) {
                d = c.getAvailableLines(); 
            }
        }

        //if there are less than 4 dialogue options
        if (d.Count < 4) {
            showOptions(d.Count+1); //show buttons equal to the number of dialogue options and an exit button

            //for each option, set the label and onClick for the associated button
            for (int i = 0; i < d.Count; i++) {
                buttonLabels[i].text = d[i].buttonText;
                buttonOptions[i] = d[i].fileName;
                //buttons[i].onClick.AddListener(delegate { selectDialogue(d[i].fileName); });
            }

            //set the exit button and option type
            buttonLabels[d.Count].text = "Goodbye";
            buttonOptions[d.Count] = "Goodbye";
            //buttons[d.Count].onClick.AddListener(delegate { selectDialogue("Goodbye"); });
        }

        optionType = "dialogue";
        
    }

    public void loadRoomList () {
        //when displaying available rooms, make sure dialogue is hidden and the label is changed
        hideDialogue();
        currentSpeaker.text = "Room Directory";

        //if there are less than 5 rooms
        if (currentRoomList.Count < 5) {
            showOptions(currentRoomList.Count); //show the number of buttons equal to the number of rooms
            //set each button and label
            for (int i = 0; i < currentRoomList.Count; i++) {
                buttonLabels[i].text = currentRoomList[i].getName();
                buttonOptions[i] = currentRoomList[i].getName();
                //buttons[i].onClick.AddListener(delegate { changeRoom(currentRoomList[i].getName()); });

            }
            
        }

        //set the option type
        optionType = "room";

    }

    public void selectCharacter (string n) {
        if (n == "Change Rooms") {
            loadRoomList();
        } else {
            //select the character with name n
            currentCharacter = n;
            changeCharacter( n + "N");
            loadCharacterOptions(n);
        }
    }

    public void selectDialogue (string fileName) {
        if (fileName == "Goodbye") {
            changeRoom(currentRoomList[currentRoomIndex].getName());
        } else {
            //load the dialogue of the associated fileName
            hideOptions();
            showDialogue();
            loadDialogue(fileName);
        }
    }

    public void makeSelection (int bNum) {

        switch (optionType) { //based on the option type, input the button selection and load the proper segment
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
        string cName = unparsed.Split('_')[0]; //get the character name by parsing around _
        string[] d = unparsed.Split('_')[1].Split(';'); //split the second half of the _ segment by the ;
        string b = d[0]; //sets the button text
        string f = d[1]; //sets the file name to load when the button is clicked

        //find the appropriate character
        for (int i = 0; i < fullCharacterList.Count; i++) {
            if (fullCharacterList[i].getName() == cName) {
                fullCharacterList[i].addLine(b, f); //add the dialogue to the character
            }
        }
    }

    //same as addDialogue, but remove from the character instead of add
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
