using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move {
    // this class is to store all the moves in the game and relevant methods
    // moves that have a bonus effect will bge subclasses of this one
    public Target AttackTarget;

    private int Power;
    private float Accuracy;
    private float Cost;
    private string Type; // allows for polytyping moves. probably will only ever be at most 2 types
    private float CritChance;
    private bool AttackType;
    Dictionary<string, List<string>> Effects;
    private Creature Attacker, Defender;


    public Move (int power, float accuracy, float cost, string type, float critChance, bool attacktype, Target attackTarget, Dictionary<string, List<string>> effects) {
        this.Power = power;
        this.Accuracy = accuracy;
        this.Cost = cost;
        this.Type = type;
        this.CritChance = critChance;
        this.AttackType = attacktype;
        this.AttackTarget = attackTarget;
        this.Effects = effects;
    }

    public void execute (Creature attacker, Creature defender) {
        this.Attacker = attacker;
        this.Defender = defender;
        if (Effects != null)
            foreach (KeyValuePair<string, List<string>> effect in Effects)
                this.GetType().GetMethod(effect.Key).Invoke(this, effect.Value.ToArray());
    }

    public float getModifier() {
        string attackingType = Attacker.getType();
        string defendingType = Defender.getType();

        float typeMultiplier = 1.0f;
        if (Matchups.getStrongTypeEffectiveness(this.Type).Contains(defendingType)) {
            typeMultiplier = 2.0f;
        } else if (Matchups.getWeakTypeEffectiveness(this.Type).Contains(defendingType)) {
            typeMultiplier = 0.5f;
        }

        float stabMultiplier = 1.0f;
        if (attackingType == Type) {
            stabMultiplier = 2.0f;
        } else if (Matchups.getLinkedTypes(attackingType).Contains(this.Type)) {
            stabMultiplier = 1.5f;
        } else if (Matchups.getUnlinkedTypes(attackingType).Contains(this.Type)) {
            stabMultiplier = 0.75f;
        }

        float critMultiplier = 1.0f;
        float random = Random.Range(0,1);
        if (this.CritChance > random) {
            critMultiplier = 3.0f;
        }

        return  typeMultiplier * stabMultiplier* critMultiplier;
    }

    public void Damage(){
        float relevantAttackStat = Attacker.getAttack(AttackType);
        float relevantDefenseStat = Defender.getDefense(AttackType);
        float modifier = getModifier(); // used for type advantages and stuff

        //pokemon way (with magic numbers) 
        //float attackerLevel = attacker.getLevel();
        //float numerator =((2*attackerLevel)/5)*this.Power*(relevantAttackStat/relevantDefenseStat);
        //float damageToTake = ((numerator/50) + 2)*modifier;

        //my way
        float damageToTake = this.Power * (relevantAttackStat / relevantDefenseStat) * modifier;
        if(Defender.currentActiveHealth > damageToTake){
            Defender.currentActiveHealth -= damageToTake;
        }else {
            damageToTake -= Defender.currentActiveHealth;
            Defender.currentActiveHealth = 0;
            Defender.currentCriticalHealth = Mathf.Max(0, Defender.currentCriticalHealth - damageToTake);
        }
    }

    public void Buff(string stat, string modifier){
        //placeholder
    }

    public enum Target{
        Self,
        Ally,
        Single,
        Double,
        Others,
        All
    }
}
