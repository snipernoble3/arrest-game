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
	
	// Update is called once per frame
	void Update () {
		
	}

    public void readinCharacters (TextAsset t) {
        List<Character> c = new List<Character>();
        string[] characters = t.text.Split('|');
        
        foreach (string character in characters) {
            string[] components = character.Split(':');

            string[] spritePaths = new string[3];
            string[] availSprites = components[1].Split('*');
            for (int i = 0; i < availSprites.Length; i++) {
                spritePaths[i] = "sprites\\" + availSprites[i];
            }
            
            c.Add(new Character(components[0], Resources.Load<Sprite>(spritePaths[0]), Resources.Load<Sprite>(spritePaths[1]), Resources.Load<Sprite>(spritePaths[2])));
        }
        
        SendMessage("updateCharacterList", c);
    }

    /*
    public void readinCharacters (TextAsset t) {

        List<Character> c = new List<Character>();
        string[] characters;
        string[] components;
        string name;
        string[] sprites;
        string[] dialogue;

        characters = t.text.Split('|');
        
        foreach (string character in characters) {
            components = character.Split(';');
            name = components[0];
            sprites = components[1].Split('*');
            dialogue = components[2].Split('*');

            for (int i = 0; i < sprites.Length; i++) {
                sprites[i] = "sprites\\" + sprites[i];
            }

            //Add in name and sprites to a temp character
            Character thisC = new Character(name, Resources.Load<Sprite>(sprites[0]), Resources.Load<Sprite>(sprites[1]), Resources.Load<Sprite>(sprites[2]));

            foreach (string d in dialogue) {
                string[] line = d.Split('_');
                thisC.addLine(line[0], line [1]);
            }

            c.Add(thisC);
        }


        SendMessage("updateCharacters", c);
    }
    */

    public void readinRooms (TextAsset t) {
        List<Room> r = new List<Room>();

        string[] rooms;
        //string[] currRoom;

        rooms = t.text.Split('|');

        foreach (string room in rooms) {
            //currRoom = room.Split(':');
            Room thisRoom = new Room(room, Resources.Load<Sprite>("sprites\\" + room));
            r.Add(thisRoom);
        }
        
        SendMessage("updateRoomList", r);
    }

    public void readinScene (TextAsset t) {

        List<Room> r = new List<Room>();
        List<string> c = new List<string>();
        
        string[] rooms;
        string[] currRoom;
        string[] characters;
        string[] currCharacter;

        rooms = t.text.Split('|');
        
        foreach (string room in rooms) {
            currRoom = room.Split(':');
            Room thisRoom = new Room(currRoom[0], Resources.Load<Sprite>("sprites\\" + currRoom[0]));
            characters = currRoom[1].Split('*');
            
            for (int i = 0; i < characters.Length; i++) {
                currCharacter = characters[i].Split('~');
                thisRoom.addCharacterToRoom(currCharacter[0]);
                c.Add(characters[i]);
                
            }
            r.Add(thisRoom);
        }


        SendMessage("updateCurrentRooms", r);
        SendMessage("updateCurrentCharacters", c);
    }

    /*
    public void readinEvidence (TextAsset t) {

        List<Evidence> e = new List<Evidence>();
        
        SendMessage("updateEvidence", e);
    }
    */

    public void readDialogue (string fileName) {
        
        TextAsset t = Resources.Load("textFiles\\" + fileName) as TextAsset;
        
        string[] text = t.text.Split('|');
        
        StartCoroutine(assessText(text));
    }

    public void readDialogue( TextAsset t) {
        
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
                
                currentSpeaker.text = segment.Substring(1);
                cont = true;
                break;
            case '*':
                //Debug.Log("Found *");
                if (segment[1] == 'r') {
                    //Debug.Log("Changing Background");
                    SendMessage("changeBackground", segment.Substring(2));
                } else if (segment[1] == 'c') {
                    //Debug.Log("Changing Character");
                    SendMessage("changeCharacter", segment.Substring(2));
                }

                cont = true;
                
                break;
            case '"':
                
                StartCoroutine(type(segment.Substring(1)));
                
                typing = typeSpeed;
                
                break;
            case '+':
                if (segment[1] == 'd') {
                    Debug.Log("Adding Dialogue");
                    SendMessage("addDialogue", segment.Substring(2));
                }
                cont = true;
                break;
            case '-':
                if (segment[1] == 'd') {
                    Debug.Log("Removing Dialogue");
                    SendMessage("removeDialogue", segment.Substring(2));
                }
                cont = true;
                break;
            case '~':
                //Debug.Log("Found ~");
                if (segment[1] == 'r') {
                    //Debug.Log("Changing Room");
                    SendMessage("changeRoom", segment.Substring(2));
                } else if (segment[1] == 's') {
                    //Debug.Log("Updating Scene");
                    readinScene(Resources.Load("textFiles\\" + segment.Substring(2)) as TextAsset);
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
