using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Matchups{
    //Type effectiveness dictionaries
    //this is for types that the key type deals EXTRA damage to
    private static Dictionary<string,List<string>> strongTypeEffectiveness = new Dictionary<string,List<string>>{
        {"Fire", new List<string>{"Wood", "Metal", "Ice"}},
        {"Wood", new List<string>{"Water", "Earth", "Smog"}},
        {"Water", new List<string>{"Metal", "Fire", "Sky"}},
        {"Metal", new List<string>{"Earth", "Wood", "Thunder"}},
        {"Earth", new List<string>{"Fire", "Water", "Beast"}},
        {"Thunder", new List<string>{"Wood", "Beast"}},
        {"Beast", new List<string>{"Water", "Ice"}},
        {"Ice", new List<string>{"Metal", "Smog"}},
        {"Smog", new List<string>{"Earth", "Sky"}},
        {"Sky", new List<string>{"Fire", "Thunder"}},
        {"Light", new List<string>{"Dark"}},
        {"Dark", new List<string>{"Light"}}
    }; 

     //this is for types that the key type deals LESS damage to
    private static Dictionary<string,List<string>> weakTypeEffectiveness = new Dictionary<string,List<string>>{
        {"Fire", new List<string>{"Earth", "Water", "Sky", "Fire"}},
        {"Wood", new List<string>{"Fire", "Metal", "Thunder", "Wood"}},
        {"Water", new List<string>{"Wood", "Earth", "Beast", "Water"}},
        {"Metal", new List<string>{"Water", "Fire", "Ice", "Metal"}},
        {"Earth", new List<string>{"Metal", "Wood", "Smog", "Earth"}},
        {"Thunder", new List<string>{"Metal", "Sky", "Thunder"}},
        {"Beast", new List<string>{"Earth", "Thunder", "Beast"}},
        {"Ice", new List<string>{"Fire", "Beast", "Ice"}},
        {"Smog", new List<string>{"Wood", "Ice", "Smog"}},
        {"Sky", new List<string>{"Water", "Smog", "Sky"}},
        {"Light", new List<string>{"Light"}},
        {"Dark", new List<string>{"Dark"}}
    };

    //used mainly for STAB bonuses maybe also for semi type effectiveness (?)
    //linked is for secondary STAB bonuses
    private static Dictionary<string, List<string>> linkedTypes = new Dictionary<string, List<string>>{
        {"Fire", new List<string>{"Lightning","Earth"}},
        {"Earth", new List<string>{"Sky","Metal"}},
        {"Metal", new List<string>{"Stone","Water"}},
        {"Water", new List<string>{"Ice","Wood"}},
        {"Wood", new List<string>{"Beast","Fire"}},
        {"Lighting", new List<string>{"Fire"}},
        {"Sky", new List<string>{"Earth"}},
        {"Stone", new List<string>{"Metal"}},
        {"Ice", new List<string>{"Water"}},
        {"Beast", new List<string>{"Wood"}},
        {"Light", new List<string>{}},
        {"Dark", new List<string>{}}
    };
    //unlinked is for secondary STAB losses
    private static Dictionary<string,List<string>> unlinkedTypes = new Dictionary<string,List<string>> {
        {"Fire", new List<string>{"Metal","Water"}},
        {"Earth", new List<string>{"Water","Wood"}},
        {"Metal", new List<string>{"Fire","Wood"}},
        {"Water", new List<string>{"Fire","Erth"}},
        {"Wood", new List<string>{"Metal","Earth"}},
        {"Lightning", new List<string>{}},
        {"Sky", new List<string>{}},
        {"Stone", new List<string>{}},
        {"Ice", new List<string>{}},
        {"Beast", new List<string>{}},
        {"Light", new List<string>{"Dark"}},
        {"Dark", new List<string>{"Light"}}
    };

    public static List<string> getStrongTypeEffectiveness (string type) {
        return strongTypeEffectiveness[type];
    }

    public static List<string> getWeakTypeEffectiveness (string type) {
        return weakTypeEffectiveness[type];
    }

    public static List<string> getLinkedTypes (string type) {
        return linkedTypes[type];
    }

    public static List<string> getUnlinkedTypes (string type) {
        return unlinkedTypes[type];
    }
}
