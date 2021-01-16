using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class Species:ScriptableObject{
    [SerializeField]
    private Type typeP, typeS;
    //0-health, 1-mana, 2-attack, 3-defense, 4-intelligence, 5-resistance, 6-speed, 7-exp
    [SerializeField]
    private int[] baseStats;
    [SerializeField]
    private LearnedMove[] learnset;
    public Sprite battleSprite;

    public Species(Type typePIn, Type typeSIn, int[] baseStatsIn) {
        typeP = typePIn;
        typeS = typeSIn;
        baseStats = baseStatsIn;
    }

    public int[] getStats() {
        return baseStats;
    }

    public Type getPType () {
        return typeP;
    }

    public Type getSType () {
        return typeS;
    }

    [Serializable]
    //TODO: code how to serialize this
    struct LearnedMove {
        int level;
        Move move;
    }
}