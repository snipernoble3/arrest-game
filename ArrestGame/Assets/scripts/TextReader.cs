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

    public void readinRooms (TextAsset t) {

        List<Room> r = new List<Room>();
        string[] rooms;
        string[] currRoom;

        rooms = t.text.Split('|');

        foreach (string room in rooms) {
            currRoom = room.Split(';');
            string path = "Assets/sprites/" + room[1];
            r.Add(new Room(currRoom[0], Resources.Load(path) as Sprite));
        }


        SendMessage("updateRooms", r);
    }

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

    public void readinEvidence (TextAsset t) {

        List<Evidence> e = new List<Evidence>();
        
        SendMessage("updateEvidence", e);
    }

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
                
                if (segment[1] == 'b') {
                    SendMessage("changeBackground", segment.Substring(2));
                } else if (segment[1] == 'c') {
                    SendMessage("changeCharacter", segment.Substring(2));
                }

                cont = true;
                
                break;
            case '"':
                
                StartCoroutine(type(segment.Substring(1)));
                
                typing = typeSpeed;
                
                break;
            case '~':
                //if (segment[1] != '~') {
                //    string[] buttons = segment.Substring(1).Split(';');
                //    SendMessage("loadOptions", buttons);
                //}
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
