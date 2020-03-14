using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType {
    Damage, FixedDamage, PercentageDamage, Buff, /*RecoilDamage*/
}


[System.Serializable]
public class Effect {
    
    public EffectType effectType;
    public float power;
    public bool useCurrent;

    private CreatureBattleStatusController attacker, defender;
    private float damageModifier;
    private bool MakesContact;

    public void execute (CreatureBattleStatusController attacker, CreatureBattleStatusController defender, float damageModifier, bool MakesContact) {
        this.attacker = attacker;
        this.defender = defender;
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
            case EffectType.Buff : {
                Buff();
                break;
            }
        }
    }

    private void Damage () {
        float relevantAttackStat = attacker.getAttack(MakesContact);
        float relevantDefenseStat = defender.getDefense(MakesContact);

        //calculate damage formula
        float damageToTake = Mathf.Floor((power * relevantAttackStat*(3/2) * damageModifier) / (relevantDefenseStat));
        ApplyDamage(damageToTake, defender);
    }

    private void FixedDamage () {
        ApplyDamage(power, defender);
    }

    private void PercentageDamage () {
        float damageToTake = power * (useCurrent ? defender.GetCreature().currentActiveHealth : defender.GetCreature().maxActiveHealth);
        ApplyDamage(damageToTake, defender);
    }

    private void Buff () {
        Debug.Log("used Buff (WIP)");
        //will probably make an enum so it knows which stat to buff, then buff it by the power amount
    }

    private void ApplyDamage (float damageToTake, CreatureBattleStatusController damageTarget) {
        if(damageTarget.GetCreature().currentActiveHealth > damageToTake){
            damageTarget.GetCreature().currentActiveHealth -= damageToTake;
        }else {
            damageToTake -= damageTarget.GetCreature().currentActiveHealth;
            damageTarget.GetCreature().currentActiveHealth = 0;
            damageTarget.GetCreature().currentCriticalHealth = Mathf.Max(0, damageTarget.GetCreature().currentCriticalHealth - damageToTake);
        }
        damageTarget.GetCreature().currentActiveHealth = Mathf.Min(damageTarget.GetCreature().currentActiveHealth, damageTarget.GetCreature().maxActiveHealth);
    }
}
