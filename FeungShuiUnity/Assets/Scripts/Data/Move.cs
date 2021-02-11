using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class Move : ScriptableObject{
    // this class is to store all the moves in the game and relevant methods
    // moves that have a bonus effect will bge subclasses of this one
    public Target AttackTarget;

    [SerializeField]
    private float Accuracy;
    [SerializeField]
    private float Cost;
    [SerializeField]
    private Type Type;
    [SerializeField]
    private float CritChance;
    [SerializeField]
    private bool MakesContact;
    [SerializeField]
    private List<Effect> Effects;

    private CreatureBattleStatusController Attacker, Defender;

    public bool execute (CreatureBattleStatusController attacker, CreatureBattleStatusController defender) {
        //put a check for accuracy here:
            //if it fails the attack misses
        if (Random.Range(0.0f, 1.0f) <= Accuracy) {
            //then the move hits
            this.Attacker = attacker;
            this.Defender = defender;
            if (Effects != null) {
                foreach (Effect effect in Effects) {
                    effect.execute(attacker, defender, getModifier(), MakesContact);
                }
            }
            return true;
        } else {
            //move misses
            return false;
        }
        
    }

    public float getCost() {
        return Cost;
    }

    public float getModifier() {
        Type attackingPType = Attacker.GetCreature().getPType();
        Type attackingSType = Attacker.GetCreature().getSType();
        Type defendingPType = Defender.GetCreature().getPType();
        Type defendingSType = Defender.GetCreature().getSType();
        //Debug.Log(attackingPType);
        //Debug.Log(defendingPType);

        List<Type> strongMatchups = Matchups.getStrongTypeEffectiveness(this.Type);

        float weakTypeMultiplier = 1.0f; //default
        if (strongMatchups.Contains(defendingPType) && strongMatchups.Contains(defendingSType)) {
            //if BOTH defending types are weak to attacking move type
            weakTypeMultiplier = 4.0f;
        } else if (strongMatchups.Contains(defendingPType) || strongMatchups.Contains(defendingSType)) {
            //if ONE of the defending types is weak to the attacking move type
            weakTypeMultiplier = 2.0f;
        }

        List<Type> weakMatchups = Matchups.getWeakTypeEffectiveness(this.Type);

        float resistantTypeMultiplier = 1.0f; //default
        if (weakMatchups.Contains(defendingPType) && weakMatchups.Contains(defendingSType)) {
            //if BOTH defending types are resistant to attacking move type
            resistantTypeMultiplier = 0.25f;
        } else if (weakMatchups.Contains(defendingPType) || weakMatchups.Contains(defendingSType)) {
            //if ONE of the defending types is resistant to the attacking move type
            resistantTypeMultiplier = 0.5f;
        }

        float stabMultiplier = 1.0f; //default
        if (attackingPType == this.Type || attackingSType == this.Type) {
            //since moves only have 1 type only need to do an OR check
            stabMultiplier = 1.5f;
        }
        //dont need this 
        /* else if (Matchups.getLinkedTypes(attackingType).Contains(this.Type)) {
            stabMultiplier = 1.5f;
        } else if (Matchups.getUnlinkedTypes(attackingType).Contains(this.Type)) {
            stabMultiplier = 0.75f;
        } */ 

        float critMultiplier = 1.0f;
        float random = Random.Range(0,1);
        if (this.CritChance > random) {
            critMultiplier = 2.0f;
        }
        Debug.Log("modifier values: " + resistantTypeMultiplier.ToString() + " " + weakTypeMultiplier.ToString() + " "
        + stabMultiplier.ToString() + " " + critMultiplier.ToString());
        
        return  resistantTypeMultiplier * weakTypeMultiplier * stabMultiplier* critMultiplier;
    }

    public enum Target{
        Self,
        Ally,
        Single,
        Double,
        Team,
        Others,
        All
    }
}
