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
    private float spAttack; // i see Mega had intelligence (im guessing)
    private float spDefense; // Mega had resistance for the name of this stat
    private float speed;
    private string type;

    //Type effectiveness dictionaries
    private static Dictionary<string,List<string>> strongTypeEffectiveness = new Dictionary<string,List<string>>{
        {"Fire", new List<string>{"Plant","Beast"}},
        {"Water", new List<string>{"Fire","Earth","Stone"}},
        {"Earth", new List<string>{"Fire","Electricity"}},
        {"Wind", new List<string>{"Plant"}}
        //add more
    }; //this is for types that the key type DEALS extra damage TO
    private static Dictionary<string,List<string>> weakTypeEffectiveness = new Dictionary<string,List<string>>{
        {"Fire", new List<string>{"Earth", "Water"}},
        {"Water", new List<string>{"Plant", "Ice"}},
        {"Earth", new List<string>{"Water", "Ice"}},
        {"Wind", new List<string>{"Electricity", "Stone"}}
        //add more
    }; //this is for types that the key type TAKES extra damage FROM
    

    public string getType() {
        return this.type;
    }
    //test comment for commiting from VS code
}
