using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovesMaster{
    private static Dictionary<string, Move> Master = new Dictionary<string, Move>{
        {"Scratch", new Move(2, 1, 1,"Beast", 0.01f, true, Move.Target.Single, new Dictionary<string,List<string>>{{"Damage", new List<string> {}}})},
        {"Pound", new Move(5, 1, 1, "Beast", 0.01f, true, Move.Target.Single, new Dictionary<string,List<string>>{{"Damage", new List<string> {}}})},
        {"Surf", new Move(90, 1, 1, "Water", 0.01f, false, Move.Target.Others, null)},
        {"Fissure", new Move(10^12, 0.3f, 1, "Earth", 0, false, Move.Target.Single, null)}, // 1,000,000,000,000 damage should be enough for a one hit (dont think we should have OHKO in our game)
        {"Howl", new Move(0, 1, 1, "Water", 0, false, Move.Target.Self, new Dictionary<string,List<string>>{{"Buff", new List<string>{"0", "0"}}})}
    };

    public static Move Find(string name){
        return Master[name];
    }
}
