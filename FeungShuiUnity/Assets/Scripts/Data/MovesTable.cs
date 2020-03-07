using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class MovesTable : ScriptableObject {
    //Power, Accuracy, Cost, Type, CritChance, AttackType (true=physical), Effects
    public List<MoveName> keys = new List<MoveName>();
    public List<Move> values = new List<Move>();

    private static Dictionary<MoveName, Move> Master = new Dictionary<MoveName, Move>();

    public static Move Find(MoveName name){
        return Master[name];
    }

    public void Init () {
        //this should just populate the dictionary to make lookups easier
        foreach (MoveName key in keys) {
            Master.Add(key, values[keys.IndexOf(key)]); //assuming that both lists are made equally
        }
    }
}
