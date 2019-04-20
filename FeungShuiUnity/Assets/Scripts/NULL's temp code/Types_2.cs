using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Types_2
{
    // holds the types and rudimentary type effectiveness in a dictionary
    static Dictionary<string,List<string>> strongTypeEffectiveness = new Dictionary<string,List<string>>{
        {"Fire", new List<string>{"Plant","Beast"}},
        {"Water", new List<string>{"Fire","Earth","Stone"}},
        {"Earth", new List<string>{"Fire","Electricity"}},
        {"Wind", new List<string>{"Plant"}}
    }; //this is for types that the key type DEALS extra damage TO

    static Dictionary<string,List<string>> weakTypeEffectiveness = new Dictionary<string,List<string>>{
        {"Fire", new List<string>{"Earth", "Water"}},
        {"Water", new List<string>{"Plant", "Ice"}},
        {"Earth", new List<string>{"Water", "Ice"}},
        {"Wind", new List<string>{"Electricity", "Stone"}}
    }; //this is for types that the key type TAKES extra damage FROM


    //dunno if you guys want to change this (probably) 
    //could just use a text file

    public static Dictionary<string,List<string>> getEffectiveness (bool Strong) {
        if (Strong) { // its true
            return strongTypeEffectiveness;
        } else {
            return weakTypeEffectiveness;
        }
    }

}
