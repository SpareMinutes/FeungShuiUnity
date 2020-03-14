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

    public void execute (CreatureBattleStatusController attacker, CreatureBattleStatusController defender) {
        this.Attacker = attacker;
        this.Defender = defender;
        if (Effects != null)
            foreach (Effect effect in Effects) {
                effect.execute(attacker, defender, getModifier(), MakesContact);
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
