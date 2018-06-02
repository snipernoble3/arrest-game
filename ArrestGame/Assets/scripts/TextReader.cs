using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VNFramework;


public class TextReader : MonoBehaviour {

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

}
