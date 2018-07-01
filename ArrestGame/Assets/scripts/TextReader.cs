using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VNFramework;


public class TextReader : MonoBehaviour {

    public Text dialogueBox;
    public Text currentSpeaker;
    public float typeSpeed;

    private float typing;
    private bool cont = false;
    

    // Use this for initialization
    void Start () {
        typing = typeSpeed;
    }
	
    //for reading in the initial character list
    public void readinCharacters (TextAsset t) {
        List<Character> c = new List<Character>();

        //split text apart by character
        string[] characters = t.text.Split('|');
        
        //add each character by parsing its components
        foreach (string character in characters) {

            //split the name and sprites apart
            string[] components = character.Split(':');

            //split the sprites apart, and set the path for them
            string[] spritePaths = new string[3];
            string[] availSprites = components[1].Split('*');
            for (int i = 0; i < availSprites.Length; i++) {
                spritePaths[i] = "sprites\\" + availSprites[i];
            }
            
            //add all the pieces as a new character in the list
            c.Add(new Character(components[0], Resources.Load<Sprite>(spritePaths[0]), Resources.Load<Sprite>(spritePaths[1]), Resources.Load<Sprite>(spritePaths[2])));
        }
        
        SendMessage("updateCharacterList", c);
    }

    //for reading in the full room list
    public void readinRooms (TextAsset t) {
        List<Room> r = new List<Room>();
        string[] rooms;

        //split the text into room names
        rooms = t.text.Split('|');

        foreach (string room in rooms) {
            //make sure the sprite has the same name that you want as the room name
            Room thisRoom = new Room(room, Resources.Load<Sprite>("sprites\\" + room));
            r.Add(thisRoom);
        }
        
        SendMessage("updateRoomList", r);
    }

    //read in a new playable scene
    public void readinScene (TextAsset t) {

        List<Room> r = new List<Room>();
        List<string> c = new List<string>();
        
        string[] rooms;
        string[] currRoom;
        string[] characters;
        string[] currCharacter;

        //split text into the playable rooms
        rooms = t.text.Split('|');
        
        //for each room, and the characters that are in it and what dialogue they have
        foreach (string room in rooms) {
            //the first component will be the name of the room
            currRoom = room.Split(':');
            Room thisRoom = new Room(currRoom[0], Resources.Load<Sprite>("sprites\\" + currRoom[0]));
            //the second component will be the characters in the room
            characters = currRoom[1].Split('*');
            
            for (int i = 0; i < characters.Length; i++) {
                currCharacter = characters[i].Split('~');
                //add the name of the character to the room list
                thisRoom.addCharacterToRoom(currCharacter[0]);
                //add the character (and their dialogue) to the character list
                c.Add(characters[i]);
            }
            r.Add(thisRoom);
        }


        SendMessage("updateCurrentRooms", r);
        SendMessage("updateCurrentCharacters", c);
    }
    

    public void readDialogue (string fileName) {
        
        TextAsset t = Resources.Load("textFiles\\" + fileName) as TextAsset;
        //split the text into its components
        string[] text = t.text.Split('|');
        
        StartCoroutine(assessText(text));
    }

    public void readDialogue( TextAsset t) {
        //split the text into its components
        string[] text = t.text.Split('|');
        
        StartCoroutine(assessText(text));
    }


    IEnumerator assessText (string[] s) {
        
        cont = false;
        typing = typeSpeed;
        
        string segment = s[0];
        segment.Trim();
        
        switch (segment[0]) {
            case ':':
                // : sets the speaker box (sized to ~16 characters)
                currentSpeaker.text = segment.Substring(1);
                cont = true;
                break;
            case '*':
                // * to indicate a sprite change
                if (segment[1] == 'r') {
                    // r for change room/background
                    SendMessage("changeBackground", segment.Substring(2));
                } else if (segment[1] == 'c') {
                    // c for change character
                    SendMessage("changeCharacter", segment.Substring(2));
                }

                cont = true;
                
                break;
            case '"':
                // " indicates dialogue
                StartCoroutine(type(segment.Substring(1)));
                
                typing = typeSpeed;
                
                break;
            case '+':
                // + to add a component (only dialogue atm)
                if (segment[1] == 'd') {
                    Debug.Log("Adding Dialogue");
                    SendMessage("addDialogue", segment.Substring(2));
                }
                cont = true;
                break;
            case '-':
                // - to remove a component (only dialogue atm)
                if (segment[1] == 'd') {
                    Debug.Log("Removing Dialogue");
                    SendMessage("removeDialogue", segment.Substring(2));
                }
                cont = true;
                break;
            case '~':
                // ~ to signal the end of the file
                if (segment[1] == 'r') {
                    // r to drop the player in a room
                    SendMessage("changeRoom", segment.Substring(2));
                } else if (segment[1] == 'c') {
                    // c to drop the player in a dialogue choice with a character
                    SendMessage("selectCharacter", segment.Substring(2));
                } else if (segment[1] == 'd') {
                    // d to continue to other dialogue
                    SendMessage("selectDialogue", segment.Substring(2));
                } else if (segment[1] == 's') {
                    // s to load a new scene
                    readinScene(Resources.Load("textFiles\\" + segment.Substring(2)) as TextAsset);
                } else if (segment[1] == 'e') {
                    // e to load the end screen (currently to be continued)
                    SendMessage("changeScene", "TBC");
                }
                StopCoroutine("assessText");
                break;
        }
        
        yield return new WaitUntil(() => cont);

        cont = false;

        string[] loop = new string[s.Length-1];
        for (int i = 1; i < s.Length; i++) {
            loop[i - 1] = s[i];
        }

        StartCoroutine(assessText(loop));

    }

    private void setTyping (bool b) {
        SendMessage("isTyping", b);
    }

    public void next () {
        cont = true;
        dialogueBox.text = "";
    }

    //for typing dialogue one character at a time
    IEnumerator type(string s) {
        setTyping(true);
        foreach (char c in s) {
            dialogueBox.text = dialogueBox.text + c;
            yield return new WaitForSecondsRealtime(typing);
        }
        setTyping(false);
        SendMessage("isWaiting");
    }

    public void finish () {
        typing = 0.0f;
    }

}
