using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VNFramework;


public class TextReader : MonoBehaviour {

    public Text dialogueBox;
    public Text currentSpeaker;
    public float typeSpeed;

    private bool typing = false;
    private bool cont = false;
    //private bool eof = false;

    // Use this for initialization
    void Start () {
		
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
                sprites[i] = "Assets/sprites/" + sprites[i];
            }

            //Add in name and sprites to a temp character
            Character thisC = new Character(name, Resources.Load(sprites[0]) as Sprite, Resources.Load(sprites[1]) as Sprite, Resources.Load(sprites[2]) as Sprite);

            foreach (string d in dialogue) {
                string[] line = d.Split('_');
                thisC.addLine(line[0], line [1], line[2]);
            }

            c.Add(thisC);
        }


        SendMessage("updateCharacters", c);
    }

    public void readinEvidence (TextAsset t) {

        List<Evidence> e = new List<Evidence>();


        SendMessage("updateEvidence", e);
    }

    public void readDialogue (/*string fileName*/ TextAsset t) {
        Debug.Log("Started reading file");
        /*TextAsset t = Resources.Load("Assets/textFiles/"+fileName) as TextAsset;*/
        Debug.Log("Loaded file");
        string[] text = t.text.Split('|');
        Debug.Log("Split initial file into paragraphs");

        //eof = false;
        
        StartCoroutine(assessText(text));
            
        

        /*
        int curr = 0;
        
        while (!eof) {
            
            typing = true;
            string[] segments = text[curr].Split('_');

            StartCoroutine(assessText(segments));
            
            foreach (string segment in segments) {

                switch (segment[0]) {
                    case ':':
                        currentSpeaker.text = segment.Substring(1);
                        break;
                    case '*':
                        SendMessage("changeBackground", segment.Substring(1));
                        break;
                    case '"':
                        //type current segment
                        setTyping(true);
                        type(segment.Substring(1));
                        yield return new WaitUntil(() => cont);
                        break;
                }
            }
            
            curr++;
        }
        */
        //eof = false;
        
    }

    IEnumerator assessText (string[] s) {

        foreach (string segment in s) {

            switch (segment[0]) {
                case ':':
                    currentSpeaker.text = segment.Substring(1);
                    break;
                case '*':
                    SendMessage("changeBackground", segment.Substring(1));
                    SendMessage("isWaiting");
                    yield return new WaitUntil(() => cont);
                    break;
                case '"':
                    //type current segment
                    float speed = typeSpeed;
                    StartCoroutine(type(segment.Substring(1)));
                    yield return new WaitUntil(() => cont);
                    dialogueBox.text = "";
                    typeSpeed = speed;
                    break;
                case '~':
                    //eof = true;
                    StopCoroutine("assessText");
                    break;
            }
            cont = false;
        }

        //eof = true;
    }

    private void setTyping (bool b) {
        SendMessage("isTyping", b);
    }

    public void next () {
        cont = true;
    }

    IEnumerator type(string s) {
        setTyping(true);
        foreach (char c in s) {
            dialogueBox.text = dialogueBox.text + c;
            yield return new WaitForSecondsRealtime(typeSpeed);
        }
        setTyping(false);
        SendMessage("isWaiting");
    }

    public void finish () {
        typeSpeed = 0.0f;
    }

}
