using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VNFramework {

    public class Room : Component {

        private Sprite background;
        new private string name;
        private List<string> charactersInRoom = new List<string>();

        public Room (string n, Sprite b) { //only require the name and sprite for the constructor
            name = n;
            background = b;
        }
        
        public void setName(string n) {
            name = n;
        }

        public string getName() {
            return name;
        }

        public void setSprite (Sprite s) {
            background = s;
        }

        public Sprite getSprite() {
            return background;
        }

        public void addCharacterToRoom (string c){
            charactersInRoom.Add(c);
        }

        public void removeCharacterFromRoom(string c) {
            charactersInRoom.Remove(c);
        }

        public List<string> getCharacters() { //returns a list of all characters
            List<string> inRoom = new List<string>();
            foreach (string c in charactersInRoom) {
                inRoom.Add(c);
            }
            return inRoom;
        }
        
    }

    public class Character : Component {

        new private string name;
        private Sprite characterNeutral;
        private Sprite characterShocked;
        private Sprite characterHappy;
        private List<Dialogue> lines = new List<Dialogue>();

        public Character (string n, Sprite cN, Sprite cS = null, Sprite cH = null) { //only require the name and neutral sprite for the constructor
            name = n;
            characterNeutral = cN;
            characterShocked = cS;
            characterHappy = cH;
        }

        public void setName(string n) {
            name = n;
        }

        public string getName() {
            return name;
        }

        public Sprite getCN () {
            return characterNeutral;
        }

        public Sprite getCS () {
            return characterShocked;
        }

        public Sprite getCH () {
            return characterHappy;
        }

        public void addLine( string b, string f) { //if the line is not already of type Dialogue, convert it
            Dialogue d;
            d.buttonText = b;
            d.fileName = f;
            lines.Add(d);
        }

        public void addLine(Dialogue d) {
            lines.Add(d);
        }

        public void removeLine (string b, string f) {
            Dialogue d;
            d.buttonText = b;
            d.fileName = f;
            lines.Remove(d);
        }

        public void removeLine (Dialogue d) {
            lines.Remove(d);
        }

        public List<Dialogue> getAvailableLines() { 
            return lines;
        }

    }
	
    public struct Dialogue {
        public string buttonText;
        public string fileName;
    }

}
