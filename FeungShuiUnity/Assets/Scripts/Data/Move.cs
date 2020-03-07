using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MoveName {
    //bit nicer than a string for a name (also works as a drop down in hte inspector)
    Scratch, Pound, Surf, Recover, HealPulse, Explosion, RazorLeaf, Howl
}

[CreateAssetMenu()]
public class Move : ScriptableObject{
    // this class is to store all the moves in the game and relevant methods
    // moves that have a bonus effect will bge subclasses of this one
    public Target AttackTarget;

    [SerializeField]
    private int Power;
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


    /* public Move (int power, float accuracy, float cost, Type type, float critChance, bool attacktype, Target attackTarget, Dictionary<string, List<string>> effects) {
        this.Power = power;
        this.Accuracy = accuracy;
        this.Cost = cost;
        this.Type = type;
        this.CritChance = critChance;
        this.AttackType = attacktype;
        this.AttackTarget = attackTarget;
        this.Effects = effects;
    } */

    public void execute (CreatureBattleStatusController attacker, CreatureBattleStatusController defender) {
        this.Attacker = attacker;
        this.Defender = defender;
        if (Effects != null)
            foreach (Effect effect in Effects) {
                //object[] things = {effect.damage, effect.useCurrent};
                this.GetType().GetMethod(effect.getStringName()).Invoke(this, new object[] {effect.damage, effect.useCurrent});
            }
    }

    public float getModifier() {
        Type attackingType = Attacker.GetCreature().getType();
        Type defendingType = Defender.GetCreature().getType();

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

    public void Damage(float _1, bool _2){
        float relevantAttackStat = Attacker.getAttack(MakesContact);
        float relevantDefenseStat = Defender.getDefense(MakesContact);
        float modifier = getModifier(); // used for type advantages and stuff

        //pokemon way (with magic numbers) 
        //float attackerLevel = attacker.getLevel();
        //float numerator =((2*attackerLevel)/5)*this.Power*(relevantAttackStat/relevantDefenseStat);
        //float damageToTake = ((numerator/50) + 2)*modifier;

        //my way
        float damageToTake = Mathf.Floor((this.Power * relevantAttackStat*(3/2) * modifier) / (relevantDefenseStat));
        ApplyDamage(damageToTake);
    }

    public void FixedDamage(float damageToTake, bool _2){
        ApplyDamage(damageToTake);
    }

    public void PercentDamage(float damagePcnt, bool useCurrent){
        float damageToTake = damagePcnt * (useCurrent ? Defender.GetCreature().currentActiveHealth : Defender.GetCreature().maxActiveHealth);
        ApplyDamage(damageToTake);
    }
    
    private void ApplyDamage(float damageToTake){
        if(Defender.GetCreature().currentActiveHealth > damageToTake){
            Defender.GetCreature().currentActiveHealth -= damageToTake;
        }else {
            damageToTake -= Defender.GetCreature().currentActiveHealth;
            Defender.GetCreature().currentActiveHealth = 0;
            Defender.GetCreature().currentCriticalHealth = Mathf.Max(0, Defender.GetCreature().currentCriticalHealth - damageToTake);
        }
        Defender.GetCreature().currentActiveHealth = Mathf.Min(Defender.GetCreature().currentActiveHealth, Defender.GetCreature().maxActiveHealth);
    }

    public void Buff(float stat, bool _2/* , float modifier */){
        //placeholder
        Debug.Log("used Buff (WIP)");
    }

    public void RecoilDamage(float percentDamage, bool _2) {
        //damages the user for a percentage of their health
            //used as a secondary effect, no move SHOULD just hurt the user and do nothing else
        CreatureBattleStatusController oldAttacker = Attacker;
        Defender = Attacker;
        float damageToTake = Defender.GetCreature().currentActiveHealth*percentDamage;
        ApplyDamage(damageToTake);
        Attacker = oldAttacker; //just to set it right again
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
