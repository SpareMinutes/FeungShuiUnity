using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    // this class is to store all the moves in the game and relevant methods
    // moves that have a bonus effect will bge subclasses of this one
    public int Power;
    public float Accuracy;
    public float Cost;
    public List<string> Types; // allows for polytyping moves. probably will only ever be at most 2 types
    public bool attackType;

    public Move (int power, int accuracy, float cost, List<string> types, bool attacktype) {
        this.Power = power;
        this.Accuracy = accuracy;
        this.Cost = cost;
        this.Types = types;
        this.attackType = attacktype;
    }
    public void execute (Creature attacker, Creature attackee) {
        // will be called whenever the move is used
        int attackerLevel = attacker.getLevel();
        float relevantAttackStat = attacker.getAttack(attackType);
        float relevantDefenseStat = attackee.getDefense(attackType);
        float modifier = 1; // used for type advantages and stuff probably its own method to calculate it
        float damageToTake = ((((2*attackerLevel)/5)*this.Power*(relevantAttackStat/relevantDefenseStat))/50 + 2)*modifier;
        
    }
}
