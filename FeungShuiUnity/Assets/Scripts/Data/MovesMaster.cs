using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovesMaster{
    //Power, Accuracy, Cost, Type, CritChance, AttackType (true=physical), Effects
    private static Dictionary<string, Move> Master = new Dictionary<string, Move>{
        {"Scratch", new Move(2, 1, 1,"Beast", 0.01f, true, Move.Target.Single, new Dictionary<string,List<string>>{{"Damage", new List<string> {}}})},
        {"Pound", new Move(5, 1, 1, "Beast", 0.01f, true, Move.Target.Single, new Dictionary<string,List<string>>{{"Damage", new List<string> {}}})},
        {"Surf", new Move(9, 1, 1, "Water", 0.01f, false, Move.Target.Others, new Dictionary<string, List<string>>{{"Damage", new List<string> {}}})},
        {"Recover", new Move(0, 1, 1, "Wood", 0, false, Move.Target.Self, new Dictionary<string,List<string>>{{"FixedDamage", new List<string>{"-5"}}})},
        {"HealPulse", new Move(0, 1, 1, "Wood", 0f, false, Move.Target.Ally, new Dictionary<string,List<string>>{{"PercentDamage", new List<string> {"-0.5", "false"}}})},
        {"Explosion", new Move(10, 1, 1, "Fire", 0, true, Move.Target.All, new Dictionary<string, List<string>>{{"Damage", new List<string> {}}})},
        {"RazorLeaf", new Move(8, 1, 1, "Wood", 0, false, Move.Target.Double, new Dictionary<string, List<string>>{{"Damage", new List<string> {}}})},
        {"Howl", new Move(0, 1, 1, "Beast", 0, true, Move.Target.Team, new Dictionary<string, List<string>>{{"Buff", new List<string> {"", ""}}})}
    };

    public static Move Find(string name){
        return Master[name];
    }
}
