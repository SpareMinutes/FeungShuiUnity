using System.Collections.Generic;
using UnityEngine;

//Handles Creature status for reference in parties
[System.Serializable]
public class Creature{
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

    public void Start() {
    }
    
    public int getLevel (){
        return (int)Mathf.Pow(totalExp,1/3);
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

    public bool isPlayerOwned () {
        return playerOwned;
    }
}
