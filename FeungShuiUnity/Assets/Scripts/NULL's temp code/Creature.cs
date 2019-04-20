using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour
{
    //this is (mostly) what Brace and I (NULL1FY3R) were discussing
    //so this class holds all the essential things that every spirit has in common (functionality wise)

    //stats
    public float activeHealth;
    public float criticalHealth;
    public float attack;
    public float defense;
    public float spAttack; // i see Mega had intelligence (im guessing)
    public float spDefense; // Mega had resistance for the name of this stat
    public float speed;
    public Types_2 Type;

    public void takeDamage (Creature other) {
        //call the getEffectiveness function and see if the type matches the current spirits to get the effectiveness multiplier
    }

    // etc.. i have to go to sleep now (1:41 am AEST)

}
