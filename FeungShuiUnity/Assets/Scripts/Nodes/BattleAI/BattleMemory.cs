using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleMemory : MonoBehaviour{
    private List<KnownSpirit> allies, enemies;
    private bool isWild;
    
    public bool GetIsWild() {
        return isWild;
    }

    public class KnownSpirit {
        private Species species;
        private List<Move> moves;
        private float[] statChanges;
        private StatusEffect statusEffect;
    }
}
