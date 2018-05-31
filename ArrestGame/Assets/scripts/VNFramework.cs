using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VNFramework {

    public class Room : Component {

        private Sprite background;
        private string name;
        private List<Character> charactersInRoom = new List<Character>();
        private List<Evidence> evidenceInRoom = new List<Evidence>();

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }

        public void setName(string n) {
            name = n;
        }

        public string getName() {
            return name;
        }

        public void addCharacterToRoom (Character c){
            charactersInRoom.Add(c);
        }

        public void removeCharacterFromRoom(Character c) {
            charactersInRoom.Remove(c);
        }

        public void addEvidenceToRoom (Evidence e) {
            evidenceInRoom.Add(e);
        }

        public void removeEvidenceFromRoom (Evidence e) {
            evidenceInRoom.Remove(e);
        }

        public List<string> getContents() { //returns a list of all characters and evidence in the room (for selecting options)
            List<string> inRoom = new List<string>();
            foreach (Character c in charactersInRoom) {
                inRoom.Add(c.getName());
            }
            foreach (Evidence e in evidenceInRoom) {
                inRoom.Add(e.name);
            }

            return inRoom;
        }

    }

    public class Character : Component {

        private string name;
        private Sprite characterNeutral;
        private Sprite characterShocked;
        private Sprite characterHappy;
        private List<Dialogue> lines = new List<Dialogue>();

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }

        public void setName(string n) {
            name = n;
        }

        public string getName() {
            return name;
        }

        public void addLine(string k, string u, string r) { //if the line is not already of type Dialogue, convert it (idk how I'll pass it yet)
            Dialogue d;
            d.key = k;
            d.userText = u;
            d.responseText = r;
            lines.Add(d);
        }

        public void addLine(Dialogue d) {
            lines.Add(d);
        }

        public List<Dialogue> getAvailableLines(string[] currKeys) { //returns all lines that can be accessed by the player
            List<Dialogue> currLines = new List<Dialogue>();

            //for each key, check it against the list of dialogue lines
            foreach (string key in currKeys) {
                foreach (Dialogue d in lines) {
                    if (key.Equals(d.key)) {
                        currLines.Add(d);
                    }
                }
            }
            return currLines;
        }

    }
	
    public struct Dialogue {
        public string key; //this will determine if the dialogue is accessible at the moment
        public string userText;
        public string responseText;
    }

    public struct Evidence {
        public string name;
        public Sprite s;
        public int numSteps; //for displaying step 1/x, or at least tracking when the path is complete
        public int currStep; //the current quest step
        public string[] hints;
    }

}
