using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType {
    Damage, FixedDamage, PercentageDamage, Status, BuffSelf, BuffTarget
}
public enum StatusEffect {
    None=0, Burn, FrostBite, Rabid, Poison, Blind, Paralysis, WaterLogged, Tangle, Exhaustion, Shaken, WindBlown, Sleep
}
public enum Stat {
    None = 0, HP, Attack, Defense, Intelligence, Resistance, Speed
}


[System.Serializable]
public class Effect {
    
    public EffectType effectType;
    public StatusEffect statusEffect;
    public Stat stat;
    public float power;
    public float statusChance;
    public bool useCurrent;

    private CreatureBattleStatusController user, target;
    private float damageModifier;
    private bool MakesContact;

    public void execute (CreatureBattleStatusController attacker, CreatureBattleStatusController defender, float damageModifier, bool MakesContact) {
        user = attacker;
        target = defender;
        this.damageModifier = damageModifier;
        this.MakesContact = MakesContact;

        switch (effectType) {
            case EffectType.Damage : {
                Damage();
                break;
            }
            case EffectType.FixedDamage : {
                FixedDamage();
                break;
            }
            case EffectType.PercentageDamage : {
                PercentageDamage();
                break;
            }
            case EffectType.Status : {
                Status();
                break;
            }
        }
    }

    private void Damage () {
        float relevantAttackStat = user.getAttack(MakesContact);
        float relevantDefenseStat = target.getDefense(MakesContact);

        //calculate damage formula
        float damageToTake = Mathf.Floor((power * relevantAttackStat*(3/2) * damageModifier) / (relevantDefenseStat));
        ApplyDamage(damageToTake, target);
    }

    private void FixedDamage () {
        ApplyDamage(power, target);
    }

    private void PercentageDamage () {
        float damageToTake = power * (useCurrent ? target.GetCreature().currentActiveHealth : target.GetCreature().maxActiveHealth);
        ApplyDamage(damageToTake, target);
    }

    private void Buff () {
        Debug.Log("used Buff (WIP)");
        //will probably make an enum so it knows which stat to buff, then buff it by the power amount
    }

    private void Status () {
        //will apply a status effect to the target
        //uses:
            //chance : for the chance that the move will apply the status effect to the target
            //power : for how much it increases the change of wearing off each time (handled in BattleMenu.cs)
                //meaning that having it at 1 means that at the end of next turn it will wear off (chance = 100%)

        if (Random.Range(0.0001f, 1.0f) <= statusChance) {
            //then the status effect gets applied
            target.statusEffect = statusEffect;
            target.statusPower = power;
            Debug.Log("gave " + target.statusEffect.ToString() + "to " + target.Target.displayName);
        }
    }

    private void ApplyDamage (float damageToTake, CreatureBattleStatusController damageTarget) {
        if(damageTarget.GetCreature().currentActiveHealth > damageToTake){
            damageTarget.GetCreature().currentActiveHealth -= damageToTake;
        } else {
            damageToTake -= damageTarget.GetCreature().currentActiveHealth;
            damageTarget.GetCreature().currentActiveHealth = 0;
            damageTarget.GetCreature().currentCriticalHealth = Mathf.Max(0, damageTarget.GetCreature().currentCriticalHealth - damageToTake);
        }
        damageTarget.GetCreature().currentActiveHealth = Mathf.Min(damageTarget.GetCreature().currentActiveHealth, damageTarget.GetCreature().maxActiveHealth);
    }
}
