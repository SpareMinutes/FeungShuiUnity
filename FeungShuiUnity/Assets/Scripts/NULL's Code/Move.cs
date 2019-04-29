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
    public float CritChance;
    public bool AttackType;


    private Types_2 moveTypes = new Types_2();

    public Move (int power, float accuracy, float cost, List<string> types,float critChance, bool attacktype) {
        this.Power = power;
        this.Accuracy = accuracy;
        this.Cost = cost;
        this.Types = types;
        this.CritChance = critChance;
        this.AttackType = attacktype;
    }
    public float execute (Creature attacker, Creature defender) {
        // will be called whenever the move is used
        int attackerLevel = attacker.getLevel();
        int defenderLevel = defender.getLevel();
        float levelDiff = attackerLevel/defenderLevel; // if they're 2x your level they deal 2x damage ontop of everything else, so if you go
        //into a battle your too under level then you will die
        float relevantAttackStat = attacker.getAttack(AttackType);
        float relevantDefenseStat = defender.getDefense(AttackType);
        float modifier = getModifier(attacker.getType(), defender.getType(), attacker); // used for type advantages and stuff
        
        //pokemon way (with magic numbers) 
        //float numerator =((2*attackerLevel)/5)*this.Power*(relevantAttackStat/relevantDefenseStat);
        //float damageToTake = ((numerator/50) + 2)*modifier;

        //my way
        float damageToTake = this.Power * (relevantAttackStat/relevantDefenseStat) * levelDiff * modifier;
        return damageToTake;
        
    }

    public float getModifier(string attackingType, string defendingType, Creature attacker) {

        float typeEffectiveness; // attacking move type vs defender (creature) type
        if (moveTypes.getStrongTypeEffectiveness(attackingType).Contains(defendingType)) {
            typeEffectiveness = 2.0f;
        } else if (moveTypes.getWeakTypeEffectiveness(attackingType).Contains(defendingType)) {
            typeEffectiveness = 0.5f;
        } else {
            typeEffectiveness = 1.0f;
        }

        float stabEffectiveness; // attacker's (Creature) type vs the move type (attacking)
        if (attacker.getType() == attackingType) {
            stabEffectiveness = 2.0f;
        } else if (moveTypes.getLinkedTypes(attacker.getType()).Contains(attackingType)) {
            stabEffectiveness = 1.5f;
        } else if (moveTypes.getUnlinkedTypes(attacker.getType()).Contains(attackingType)) {
            stabEffectiveness = 0.75f;
        } else {
            stabEffectiveness = 1.0f;
        }

        float crit;
        float random = Random.Range(0,1);
        if (this.CritChance > random) {
            //crit hits
            crit = 3.0f;
        } else {
            crit = 1.0f;
        }

        return  typeEffectiveness * stabEffectiveness * crit;
    }
}
