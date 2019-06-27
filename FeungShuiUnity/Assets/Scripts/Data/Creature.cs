using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    //this is (mostly) what Brace and I (NULL1FY3R) were discussing
    //so this class holds all the essential things that every spirit has in common (functionality wise)

    //name of the creature, will default to species name if none is given
    public string displayName;

    //stats
    public float maxActiveHealth;
    public float currentActiveHealth;
    public float maxCriticalHealth;
    public float currentCriticalHealth;
    public float attack;
    public float defense;
    public float intelligence; //Special attack
    public float resistance; //Special defense
    public float speed;
    private float friendship = 0; //0 should be neutral, and less <0 is bad >0 is good

    //xp stuff
    private float totalExp = 0;

    //misc stats
    public bool playerOwned;
    public string type; // only single type for now
    private string personality;
    private List<float> statModifiers;
    //testing only
    public List<string> moveNames;
    private List<Move> Moves;

    private static Dictionary<Creature, float> friendshipDict = new Dictionary<Creature, float>();
    private float trainerFriendship; // because the player isnt of type Creature

    /*
    public void Start() {
        //pick a random nature
        this.personality = Natures2.personalityKeys[Random.Range(0,Natures2.personalityKeys.Count)];
        this.statModifiers = Natures2.personalityDict[this.personality];

        //this is where the stats will be generated based off (real thing)

        //for testing purposes i'll add moves manually here
        foreach (string name in moveNames) {
            Moves.Add(MovesMaster.Master[name]);
        }
    }
    */
    public int getLevel (){
        return (int)Mathf.Pow(totalExp,1/3);
    }

    public float getAttack (bool Physical) {
        if (Physical) {
            return this.attack;
        } else {
            return this.intelligence;
        }
    }

    public float getDefense (bool Physical) {
        if (Physical) {
            return this.defense;
        } else{
            return this.resistance;
        }
    }

    public string getType (){
        return this.type;
    }

    public float getActiveHealth () {
        return currentActiveHealth;
    }

    public float getCriticalHealth () {
        return currentCriticalHealth;
    }

    public float getSpeed () {
        return speed;
    }

    public void takeTurn () {
        Debug.Log(displayName + " did a thing");

        //in this function a move will be chosen and items will be used
    }
}
