using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour{
    //name of the creature, will default to species name if none is given
    public string displayName;

    //stats
    public float maxActiveHealth;
    public float maxCriticalHealth;
    public float currentCriticalHealth;
    public float currentActiveHealth;

    public float baseAttack;
    public float baseDefense;
    public float baseIntelligence; //Special attack
    public float baseResistance; //Special defense
    public float baseSpeed;

    private float currentAttack;
    private float currentDefense;
    private float currentIntelligence;
    private float currentResistance;
    private float currentSpeed;
     
    private float friendship = 0; //0 should be neutral, and less <0 is bad >0 is good

    private float preDefenseMoveDefense;
    private float preDefenseMoveResistance;
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
            return this.currentAttack;
        } else {
            return this.currentIntelligence;
        }
    }

    public float getDefense (bool Physical) {
        if (Physical) {
            return this.currentDefense;
        } else{
            return this.currentResistance;
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
        return currentSpeed;
    }

    public bool isPlayerOwned () {
        return playerOwned;
    }

    public void doDefenseMove () {
        //for now just increase defense and special defense by a bunch (very techincal i know)
        //these variables are if you buff your defenses other than this move (eg harden in pokemon)
        preDefenseMoveDefense = currentDefense;
        preDefenseMoveResistance = currentResistance;

        //multiplies the defenses by 2 (up for change)
        this.currentDefense *= 2; 
        this.currentResistance *= 2;
    }

    public void relieveDefenseMove () {
        if (preDefenseMoveDefense != null && preDefenseMoveResistance != null) { //ensures no weird things happen
            currentDefense = preDefenseMoveDefense;
            currentResistance = preDefenseMoveResistance;
        }
    }
}
