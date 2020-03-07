using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType {
    Damage, FixedDamage, PercentageDamage, Buff, /*RecoilDamage*/
}


[System.Serializable]
public class Effect {
    
    public EffectType effectType;
    public float damage;
    public bool useCurrent;


    public string getStringName () {
        //just to get the string form of the the effect type
        switch (effectType) {
            case EffectType.Damage : {
                return "Damage";
            } case EffectType.FixedDamage : {
                return "FixedDamage";
            } case EffectType.PercentageDamage : {
                return "PercentageDamage";
            } case EffectType.Buff : {
                return "Buff";
            }
            default : {
                return "None";
            }
        }
    }

}
