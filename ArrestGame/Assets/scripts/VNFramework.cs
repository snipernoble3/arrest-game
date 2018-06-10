using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VNFramework {

    public class Room : Component {

        private Sprite background;
        new private string name;
        private List<string> charactersInRoom = new List<string>();
        //private List<string> evidenceInRoom = new List<string>();

        public Room (string n, Sprite b) {
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

        /*
        public void addEvidenceToRoom (string e) {
            evidenceInRoom.Add(e);
        }

        public void removeEvidenceFromRoom (string e) {
            evidenceInRoom.Remove(e);
        }
        */

        public List<string> getCharacters() { //returns a list of all characters
            List<string> inRoom = new List<string>();
            foreach (string c in charactersInRoom) {
                inRoom.Add(c);
            }
            return inRoom;
        }

        /*
        public List<string> getEvidence() {
            List<string> inRoom = new List<string>();
            foreach (string e in evidenceInRoom) {
                inRoom.Add(e);
            }
            return inRoom;
        }
        */
    }

    public class Character : Component {

        new private string name;
        private Sprite characterNeutral;
        private Sprite characterShocked;
        private Sprite characterHappy;
        private List<Dialogue> lines = new List<Dialogue>();

        public Character (string n, Sprite cN, Sprite cS = null, Sprite cH = null) {

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

        public void addLine(/*string k,*/ string b, string f) { //if the line is not already of type Dialogue, convert it (idk how I'll pass it yet)
            Dialogue d;
            //d.key = k;
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

        public List<Dialogue> getAvailableLines() { //returns all lines that can be accessed by the player
            
            /*
            //for each key, check it against the list of dialogue lines
            foreach (string key in currKeys) {
                foreach (Dialogue d in lines) {
                    if (key.Equals(d.key)) {
                        currLines.Add(d);
                    }
                }
            }
            */
            

            return lines;
            
        }

    }
	
    public struct Dialogue {
        //public string key; //this will determine if the dialogue is accessible at the moment
        public string buttonText;
        public string fileName;
    }

    public struct Evidence {
        public string name;
        public Sprite s;
        public int numSteps; //for displaying step 1/x, or at least tracking when the path is complete
        public int currStep; //the current quest step
        public string[] hints;
    }

}
