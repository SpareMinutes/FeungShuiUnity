using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    //this is (mostly) what Brace and I (NULL1FY3R) were discussing
    //so this class holds all the essential things that every spirit has in common (functionality wise)

    //stats
    private float activeHealth;
    private float criticalHealth;
    private float attack;
    private float defense;
    private float intelligence; //Special attack
    private float resistance; //special defense
    private float speed;

    private string type;
    private float friendship = 0; //0 should be neutral, and less <0 is bad >0 is good
    private string personality;
    private List<float> statModifiers;

    private static Dictionary<Creature, float> friendshipDict = new Dictionary<Creature, float>();
    private float trainerFriendship; // because the player isnt of type Creature

    private List<string> personalityKeys = new List<string>{"Hardy","Lonely","Brave","Adamant"}; // just the pokemon ones for placeholders
    private static Dictionary<string, List<float>> personalityDict = new Dictionary<string, List<float>>{
        // the list of modifiers are in order that they are stated above (activehealth,crithealth,atk,def,int,res,spd)
        //since they are multipliers 1 means no change >1 means increase, <1 means decrease (gives potential to nullify a stat, eg shedinja always has 1 health, or we
        //      could make it not have any defense(no reduction in physical damage)) 
        {"Hardy", new List<float>{1, 1, 1, 1, 1, 1, 1}}, //no modifiers
        {"Lonely", new List<float>{1, 1.25f, 0.75f, 1, 1, 1, 1}}, // +Atk, - spA
        {"Brave", new List<float>{1, 1.25f, 1, 1, 1, 1, 0.75f}}, // +Atk, - Spd
        {"Adamant", new List<float>{1, 1.25f, 1, 0.75f, 1, 1, 1}} // +Atk, - spD
    };
    //
    //personality stuff
    //
    //
    public void Start() {
        //this is where the stats will be generated

        //pick a random nature
        this.personality = personalityKeys[Random.Range(0,this.personalityKeys.Count)];
        this.statModifiers = personalityDict[this.personality];
        Debug.Log(this.personality);
        Debug.Log(this.statModifiers);
    }
}
