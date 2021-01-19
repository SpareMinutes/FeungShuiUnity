using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class Species : ScriptableObject{
    [SerializeField]
    private Type typeP, typeS;
    //0-health, 1-mana, 2-attack, 3-defense, 4-intelligence, 5-resistance, 6-speed, 7-exp
    [SerializeField]
    private int[] baseStats;
    public Sprite battleSprite;
    [SerializeField]
    private Learnset LearnedMoves;
    public Dictionary<int,Move> moveSet = new Dictionary<int, Move>();

    public Species(Type typePIn, Type typeSIn, int[] baseStatsIn) {
        typeP = typePIn;
        typeS = typeSIn;
        baseStats = baseStatsIn;
    }

    public void OnEnable () {
        //init the learnset dict
        moveSet = new Dictionary<int, Move>();
        for (int i = 0; i < LearnedMoves.Levels.Count; i++) {
            moveSet.Add(LearnedMoves.Levels[i], LearnedMoves.Moves[i]);
        }
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

    public Move GetLevelupMove(int level) {
        return null;
    }

    [Serializable]
    public class Learnset {
        /* [SerializeField]
        private Move[] LevelupMoves = new Move[100]; */

        public List<int> Levels = new List<int>();
        public List<Move> Moves = new List<Move>();
    }
}