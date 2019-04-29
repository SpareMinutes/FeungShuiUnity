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
    private float friendship = 0; //0 should be neutral, and less <0 is bad >0 is good

    //xp stuff
    private float totalExp = 0;

    //misc stats
    private string type; // only single type for now
    private string personality;
    private List<float> statModifiers;

    private static Dictionary<Creature, float> friendshipDict = new Dictionary<Creature, float>();
    private float trainerFriendship; // because the player isnt of type Creature
    
    //variables that store all possible natures and moves
    private Natures2 nature = new Natures2();
    private MovesMaster allMoves = new MovesMaster();

    public void Start() {
        //pick a random nature
        this.personality = nature.personalityKeys[Random.Range(0,this.nature.personalityKeys.Count)];
        this.statModifiers = nature.personalityDict[this.personality];

        //this is where the stats will be generated

    }

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
}
