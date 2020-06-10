using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType {
    Damage, FixedDamage, PercentageDamage, Status, BuffSelf, BuffTarget
}
public enum StatusEffect {
    None=0, Burn, FrostBite, Rabid, Poison, Paralysis
}
public enum Stat {
    None = 0, HP, Attack, Defense, Intelligence, Resistance, Speed
}


[System.Serializable]
public class Effect {
    
    public EffectType effectType;
    public StatusEffect statusEffect;
    public Stat stat;
    /* [Tooltip(@"usage (set to -ve for healing moves):
    Damage:             the power value of the move
    FixedDamage:        the fixed amount of damage for the move
    Percentage:         will take power as value from 0-1 for the percentage of health that the move will deal as damage
    Status:             the additive value for the chance that the status will wear off (ie. setting value to 0 will never wear off, nad setting it to 0.1 will increase the chance by 10% every turn)
    BuffSelf/Targer:    the magnitude of the buff as a percentage")] */
    public float power;
    public float chance;
    [Tooltip("Only used for the PercentageDamage effectType")]
    public bool useCurrentHealth;

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
        //Debug.Log(damageModifier);
        float damageToTake = Mathf.Floor((power * Mathf.Pow(relevantAttackStat, 3/2f) * damageModifier) / (relevantDefenseStat * 15));
        user.ApplyDamage(damageToTake, target);
    }

    private void FixedDamage () {
        //no filters on the damage, will deal the exact power as damage points to the target
        user.ApplyDamage(power, target);
    }

    private void PercentageDamage () {
        float damageToTake = power * (useCurrentHealth ? target.GetCreature().currentActiveHealth : target.GetCreature().maxActiveHealth);
        user.ApplyDamage(damageToTake, target);
    }

    private void Buff () {
        Debug.Log("used Buff (WIP)");
        //usage:
            //power: how much the stat is changed by (%)
            //chance: the chance of the stat change happening
    }

    private void Status () {
        //will apply a status effect to the target
        //uses:
            //chance : for the chance that the move will apply the status effect to the target
            //power : for how much it increases the change of wearing off each time (handled in BattleMenu.cs)
                //meaning that having it at 1 means that at the end of next turn it will wear off (chance = 100%)
        float randNum = Random.Range(0.0f, 1.0f);
        if (randNum != 0 && randNum <= chance) {
            //then the status effect gets applied
            target.statusEffect = statusEffect;

            float changeAmount = 0.25f; //overall how much it changes the stat by
            switch (statusEffect) {
                case StatusEffect.Burn : {
                    //lower attack
                    target.ChangeStatFromStatus(Stat.Attack, 1 - changeAmount);
                    break;
                } case StatusEffect.Paralysis : {
                    //lower speed
                    target.ChangeStatFromStatus(Stat.Speed, 1 - changeAmount);
                    break;
                } case StatusEffect.Rabid : {
                    //raise attack
                    target.ChangeStatFromStatus(Stat.Attack, 1 + changeAmount);
                    break;
                }
            }

            //Debug.Log("gave " + target.statusEffect.ToString() + " to " + target.Target.displayName);
        }
    }
}
